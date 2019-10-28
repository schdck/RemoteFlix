using RemoteFlix.Base.Classes;
using System;
using System.Collections.Generic;

namespace RemoteFlix.Base.Players
{
    public abstract class BasePlayer
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract IntPtr? GetHandle();
        public abstract IEnumerable<PlayerCommand> Commands { get; }
    }
}
