using System;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;

namespace Flasetclass
{
    public class Flasetclass : Plugin<Config>
    {
        public EventHandlers EventHandlers { get; private set; }

        public override string Name => "Flasetclass";
        public override string Author => "https://gitlab.com/FluffyPal";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 14, 2);

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.Verified += EventHandlers.OnVerified;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= EventHandlers.OnVerified;
            EventHandlers = null;
            base.OnDisabled();
        }
    }
}