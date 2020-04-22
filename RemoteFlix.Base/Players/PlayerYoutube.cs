using RemoteFlix.Base.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteFlix.Base.Players
{
    public class PlayerYoutube : BasePlayer
    {
        public override string Id => "youtube";
        public override string Name => "YouTube";

        public override IntPtr? GetHandle()
        {
            return null;
        }

        public override IEnumerable<PlayerCommand> Commands => new PlayerCommand[]
        {
            new PlayerCommand("play_pause", "Play / Pause", "k"),
            new PlayerCommand("volume_up", "Volume UP", "{UP}"),
            new PlayerCommand("volume_down", "Volume Down", "{DOWN}"),
            new PlayerCommand("next_video", "Next video", "+n"),
            new PlayerCommand("previous_video", "Previous video", "+p"),
            new PlayerCommand("mute_unmute", "Mute / Unmute", "m"),
            new PlayerCommand("enter_full_screen", "Full screen", "f"),
            new PlayerCommand("exit_full_screen", "Exit full screen", "{ESC}"),
            new PlayerCommand("go_back", "Go back", "{LEFT}"),
            new PlayerCommand("go_forward", "Go forward", "{RIGHT}"),
            new PlayerCommand("faster", "Go faster", ">"),
            new PlayerCommand("slower", "Go slower", "<"),
            new PlayerCommand("closed_captions", "Enable / Disable CC", "c"),
        };
    }
}
