using System;
using System.Diagnostics;
using System.Threading.Tasks;
using IziLog;
using IziLog.Records;

namespace LOM
{
    class StopCommand : Command
    {
        public override string CommandName { get; set; } = "/stop";

        public async override Task Run()
        {
            Logger.Log(new InfoRecord("Запуск LOM.exe в режиме /stop"));

            if (IsAlreadyRunned()) KillProcess();              
        }

        private bool IsAlreadyRunned()
        {
            int CountProcess = 0;

            Process[] ProcessList = Process.GetProcesses();

            foreach (Process ProcessObject in ProcessList)
            {
                if (ProcessObject.ProcessName == "LOM")
                {
                    CountProcess++;
                    Logger.Log(new InfoRecord("Найдено процессов LOM: " + CountProcess));
                }
            }

            if (CountProcess == 2) return true;

            throw new Exception("Невозможно останоить процесс, он не запущен");
        }

        private void KillProcess()
        {
            Logger.Log(new InfoRecord("Запуск процесса на убийство LOM.exe"));

            int Id = Process.GetCurrentProcess().Id;
            
            Process[] ProcessList = Process.GetProcesses();

            foreach (Process ProcessObject in ProcessList)
            {
                if (ProcessObject.ProcessName == "LOM" && ProcessObject.Id != Id)
                {
                    ProcessObject.Kill();
                    Console.WriteLine("Остановка процесса LOM.exe");
                    Logger.Log(new InfoRecord("Убийство LOM.exe прошло успешно"));
                }
            }
        }
    }
}
