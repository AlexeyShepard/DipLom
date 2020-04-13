using System.Threading.Tasks;
using IziLog;
using IziLog.Records;

namespace LOM
{
    class LoadCommand : Command
    {
        public override string CommandName { get; set; } = "/load";

        public async override Task Run()
        {
            //Task.WaitAll(DatabaseUpdater.iterationExecute());
            Logger.Log(new InfoRecord("Запуск LOM.exe в режиме /load"));

            await DatabaseUpdater.iterationExecute();
        }
    }
}
