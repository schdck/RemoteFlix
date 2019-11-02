using System;
using System.Collections.Generic;
using System.Diagnostics;
using RemoteFlix.Base.Classes;

namespace RemoteFlix.Base.Players
{
    public class PlayerButter : BasePlayer
    {
        public override string Id => "butter";
        public override string Name => "PopCorn Time (Butter)";

        public override IEnumerable<PlayerCommand> Commands => new PlayerCommand[]
        {
            new PlayerCommand("play_pause", "Play / Pause", "p"),
            new PlayerCommand("volume_up", "Volume UP", "{UP}"),
            new PlayerCommand("volume_down", "Volume Down", "{DOWN}"),
            new PlayerCommand("mute_unmute", "Mute / Unmute", "m"),
            new PlayerCommand("toggle_full_screen", "Toggle full screen", "f"),
            new PlayerCommand("go_back", "Go back", "{LEFT}"),
            new PlayerCommand("go_forward", "Go forward", "{RIGHT}"),
            new PlayerCommand("faster", "Go faster", "l"),
            new PlayerCommand("slower", "Go slower", "j"),
            new PlayerCommand("normal_speed", "Normal speed", "k"),
        };

        public override IntPtr? GetHandle()
        {
            var processes = Process.GetProcessesByName("PopCorn-Time");

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
