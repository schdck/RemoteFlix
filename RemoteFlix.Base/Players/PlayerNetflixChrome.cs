using System;
using System.Diagnostics;

namespace RemoteFlix.Base.Players
{
    public class PlayerNetflixChrome : PlayerNetflix
    {
        public override string Id => "netflix_chrome";
        public override string Name => "Netflix (on Google Chrome)";
        public override IntPtr? GetHandle()
        {
            var chromeProcesses = Process.GetProcessesByName("chrome");

            foreach (Process chrome in chromeProcesses)
            {
                if (chrome.MainWindowHandle != IntPtr.Zero)
                {
                    return chrome.MainWindowHandle;
                }
            }

            return null;
        }
    }
}
