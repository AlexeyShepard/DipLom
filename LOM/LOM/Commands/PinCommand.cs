using System.Threading.Tasks;
using IziLog;
using IziLog.Records;

namespace LOM
{
    class PinCommand : Command
    {
        public override string CommandName { get; set; } = "/pin";

        public async override Task Run()
        {
            Logger.Log(new InfoRecord("Запуск LOM.exe в режиме /pin"));

            await PinCodeGenerator.IterationExecute();
        }
    }
}
