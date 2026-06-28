using Exiled.Events.EventArgs.Player;

namespace Flasetclass
{
    public class EventHandlers
    {
        private readonly Flasetclass plugin;

        public EventHandlers(Flasetclass plugin)
        {
            this.plugin = plugin;
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            ev.Player.Broadcast(plugin.Config.MessageDuration, plugin.Config.WelcomeMessage);
        }
    }
}