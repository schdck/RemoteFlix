using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteFlix.UI.Desktop.Helpers
{
    public static class CmdHelper
    {
        public static string RunAsAdmin(string command)
        {
            var output = Path.GetTempFileName();

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {command} > {output}",
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true
            };

            var process = new Process()
            {
                  StartInfo = processStartInfo
            };

            process.Start();
            process.WaitForExit();

            using(var streamReader = new StreamReader(output))
            {
                return streamReader.ReadToEnd().Trim();
            }
        }
    }
}
