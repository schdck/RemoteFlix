namespace RemoteFlix.Base.Classes
{
    public class PlayerCommand
    {
        public string Id { get; }
        public string ActionName { get; }
        public string ActionShortcut { get; }

        public PlayerCommand(string id, string actionName, string actionShortcut)
        {
            Id = id;
            ActionName = actionName;
            ActionShortcut = actionShortcut;
        }
    }
}
