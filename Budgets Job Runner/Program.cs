using Krypton.Budgets.Domain._Ports.Hosting;
using Krypton.Budgets.JobRunner.Hosting;
using Krypton.Budgets.JobRunner.Implementation;
using Krypton.Budgets.JobRunner.Jobs;
using Krypton.Budgets.Logging.Hosting;
using Krypton.Budgets.Persistence.Hosting;
using Krypton.Budgets.Security.Hosting;
using Microsoft.AspNetCore.Authorization;
using Quartz;
using IHost = Krypton.Budgets.Domain._Ports.Hosting.IHost;

var host = Host.CreateDefaultBuilder(args).
    ConfigureLogging(logger => logger.ClearProviders()).
    ConfigureHostConfiguration(host => host.AddJsonFile("appsettings.json")).
    ConfigureServices((context, services) =>
    {
        services.
            AddSingleton<JobListener>().
            AddQuartz(q =>
            {
                q.AddJobListener<JobListener>();
                RegisterJob<EchoJob>(q);
            }).
            AddQuartzHostedService(q => q.WaitForJobsToComplete = true).
            AddScoped<IHost, HostImpl>().
            AddSingleton<IAuthorizationService, AuthorizationService>().
            AddBudgetsDomain().
            AddBudgetsPersistence().
            AddBudgetsSecurity().
            AddBudgetsLogging(context.Configuration.GetSection("Logging"));

        void RegisterJob<T>(IServiceCollectionQuartzConfigurator q) where T : IJob
        {
            var jobKey = new JobKey(nameof(T));

            q.AddJob<T>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(nameof(T) + "-trigger")
                .WithCronSchedule("0/5 * * * * ?"));
        }
    }).
    Build();

host.Services.GetRequiredService<IPersistenceHosting>().RunMigrations();

host.Run();
