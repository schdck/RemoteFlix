using System;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace RemoteFlix.Base.Helpers
{
    // This class was adapted from https://stackoverflow.com/a/97517/5686352
    public static class KeyboardSimulationHelper
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void SendKeys(Process process, string keys)
        {
            SendKeys(process.MainWindowHandle, keys);
        }

        public static void SendKeys(IntPtr handle, string keys)
        {
            SetForegroundWindow(handle);

            System.Windows.Forms.SendKeys.SendWait(keys);
        }
    }
}
