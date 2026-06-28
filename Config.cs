using Exiled.API.Interfaces;

namespace Flasetclass
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string WelcomeMessage { get; set; } = "<color=#849eb4>Hello my firend</color>";
        public byte MessageDuration { get; set; } = 5;
    }
}