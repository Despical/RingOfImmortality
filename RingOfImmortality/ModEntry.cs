using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace RingOfImmortality
{
    public class ModEntry : Mod
    {

        public static ModEntry Instance;
        public RingHandler RingHandler;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            RingHandler = new();

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.DayStarted += RingHandler.HandleDayStart;
            helper.Events.GameLoop.UpdateTicked += RingHandler.HandleTicked;
            helper.Events.Display.RenderedWorld += this.OnRenderedWorld;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            RingHandler.LoadJsonAssets();
        }

        private void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            TrueSight.DrawOverWorld(e.SpriteBatch);
        }

        public void Log(string message) => Monitor.Log(message, LogLevel.Info);
    }
}