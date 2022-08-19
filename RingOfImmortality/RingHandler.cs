using SpaceShared.APIs;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using System.IO;

namespace RingOfImmortality
{
    public class RingHandler
    {

        private readonly ModEntry Instance;
        private string ID => $"{Instance.ModManifest.UniqueID}.DidPlayerPassOutYesterday";
        
        public IJsonAssetsApi JsonAssets;
        public int RingTrueSight => this.JsonAssets.GetObjectId("Ring of Immortality");

        public RingHandler() => this.Instance = ModEntry.Instance;

        public void LoadJsonAssets()
        {
            this.JsonAssets = Instance.Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");

            if (this.JsonAssets != null)
                this.JsonAssets.LoadAssets(Path.Combine(Instance.Helper.DirectoryPath, "assets", "json-assets"));
            else
                Instance.Log("Couldn't get the Json Assets API, so the new rings won't be available.");
        }

        public void HandleTicked(object sender, UpdateTickedEventArgs args)
        {
            Farmer player = Game1.player;

            if (Game1.timeOfDay == 2600 || player.stamina <= -15)
            {
                player.modData[ID] = "true";

                if (player.currentLocation as FarmHouse == null
                    && player.currentLocation as Cellar == null
                    && PlayerStateRestorer.statePassout.Value == null)
                {
                    PlayerStateRestorer.Save();
                }
            }
        }

        public void HandleDayStart(object sender, DayStartedEventArgs args)
        {
            Farmer player = Game1.player;

            if (!player.modData.ContainsKey(ID))
            {
                player.modData.Add(ID, "false");
            }

            if (!HasRingEquipped(RingTrueSight)) return;


            if (player.modData[ID] == "true" && PlayerStateRestorer.statePassout.Value != null)
            {
                PlayerStateRestorer.Load();

                player.modData[ID] = "false";

                PlayerStateRestorer.statePassout.Value = null;
            }
            else
            {
                player.modData[ID] = "false";
            }
        }

        public bool HasRingEquipped(int id) => this.CountRingsEquipped(id) > 0;

        public int CountRingsEquipped(int id)
        {
            return (Game1.player.leftRing.Value?.GetEffectsOfRingMultiplier(id) ?? 0) +
                   (Game1.player.rightRing.Value?.GetEffectsOfRingMultiplier(id) ?? 0);
        }
    }
}