using RemoteFlix.Base.Players;
using System.Collections.Generic;

namespace RemoteFlix.Base
{
    public static class RemoteFlixPlayers
    {
        public static IEnumerable<BasePlayer> AvaliablePlayers = new BasePlayer[]
        {
            new PlayerNetflix(),
            new PlayerNetflixChrome(),
            new PlayerVlc(),
            new PlayerButter(),
            new PlayerYoutube()
        };
    }
}
