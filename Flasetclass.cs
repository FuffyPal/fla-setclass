using System;
using Exiled.API.Features;

namespace Flasetclass
{
    public class Flasetclass : Plugin<Config>
    {
        public override string Name => "Flasetclass";
        public override string Author => "https://gitlab.com/FluffyPal";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 14, 2);

        public override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
        }
    }
}