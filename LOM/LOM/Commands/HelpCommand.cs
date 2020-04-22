using System;
using System.Reflection;
using System.Threading.Tasks;
using IziLog;
using IziLog.Records;

namespace LOM
{
    class HelpCommand : Command
    {
        public override string CommandName { get; set; } = "/help";

        public async override Task Run()
        {           
            Logger.Log(new InfoRecord("Запуск LOM.exe в режиме /help"));

            Console.WriteLine("/help - вызов справки\n/silent - запуск программы в штатном режиме\n/pin - генерация пин-кодв\n/load - Выгрузка базы данных\n/stop - остановка LOM\nВерсия - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " 2020");
        }
    }
}
