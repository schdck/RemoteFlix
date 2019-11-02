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
            var processes = Process.GetProcessesByName("chrome");

            foreach (var process in processes)
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    return process.MainWindowHandle;
                }
            }

            return null;
        }
    }
}
