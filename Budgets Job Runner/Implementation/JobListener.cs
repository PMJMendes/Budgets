using Quartz;

namespace Krypton.Budgets.JobRunner.Implementation;

internal class JobListener : IJobListener
{
    private static readonly ThreadLocal<string> jobName = new();

    public static string CurrentJob => jobName.Value ?? "<unknown>";

    string IJobListener.Name => GetType().Name;

    Task IJobListener.JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    Task IJobListener.JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        jobName.Value = context.JobDetail.JobType.Name;

        return Task.CompletedTask;
    }

    Task IJobListener.JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
