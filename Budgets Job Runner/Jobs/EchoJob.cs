using Krypton.Budgets.Domain.Global.Echo;
using Krypton.Budgets.JobRunner.Implementation;
using Quartz;

namespace Krypton.Budgets.JobRunner.Jobs
{
    internal class EchoJob : IJob
    {
        private readonly IEcho echo;

        public EchoJob(IEcho echo)
        {
            this.echo = echo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.Out.WriteLine(await echo.Fetch(EmptyArgs.Instance, result => result).Select(r => r.Name).FirstOrDefaultAsync());
        }
    }
}
