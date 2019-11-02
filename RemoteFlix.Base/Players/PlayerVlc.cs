using System;
using System.Collections.Generic;
using System.Diagnostics;
using RemoteFlix.Base.Classes;

namespace RemoteFlix.Base.Players
{
    // VLC has *a lot* of hotkeys, so I only put the main ones
    // For a full list, take a look here: https://wiki.videolan.org/QtHotkeys/
    public class PlayerVlc : BasePlayer
    {
        public override string Id => "vlc";
        public override string Name => "VLC Media Player";

        public override IEnumerable<PlayerCommand> Commands => new PlayerCommand[] 
        {
            new PlayerCommand("play_pause", "Play / Pause", " "),
            new PlayerCommand("volume_up", "Volume UP", "^{UP}"),
            new PlayerCommand("volume_down", "Volume Down", "^{DOWN}"),
            new PlayerCommand("mute_unmute", "Mute / Unmute", "m"),
            new PlayerCommand("toggle_full_screen", "Toggle full screen", "f"),
            new PlayerCommand("next_track", "Next track", "N"),
            new PlayerCommand("previous_track", "Previous track", "P"),
            new PlayerCommand("go_back", "Go back", "+{LEFT}"),
            new PlayerCommand("go_forward", "Go forward", "+{RIGHT}"),
            new PlayerCommand("faster", "Go faster", "{ADD}"),
            new PlayerCommand("slower", "Go slower", "{SUBTRACT}"),
            new PlayerCommand("normal_speed", "Normal speed", "="),
        };

    public override IntPtr? GetHandle()
        {
            var processes = Process.GetProcessesByName("vlc");

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
