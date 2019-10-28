using RemoteFlix.Base.Classes;
using System;
using System.Collections.Generic;

namespace RemoteFlix.Base.Players
{
    public class PlayerNetflix : BasePlayer
    {
        public override string Id => "netflix";
        public override string Name => "Netflix";

        public override IntPtr? GetHandle()
        {
            return null;
        }

        public override IEnumerable<PlayerCommand> Commands => new PlayerCommand[] 
        {
            new PlayerCommand("play_pause", "Play / Pause", "{ENTER}"),
            new PlayerCommand("volume_up", "Volume UP", "{UP}"),
            new PlayerCommand("volume_down", "Volume Down", "{DOWN}"),
            new PlayerCommand("mute_unmute", "Mute / Unmute", "m"),
            new PlayerCommand("enter_full_screen", "Full screen", "f"),
            new PlayerCommand("exit_full_screen", "Exit full screen", "{ESC}"),
            new PlayerCommand("go_back", "Go back", "{LEFT}"),
            new PlayerCommand("go_forward", "Go forward", "{RIGHT}")
        };
    }
}
