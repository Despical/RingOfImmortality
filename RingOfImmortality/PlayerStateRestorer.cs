using StardewValley;
using StardewModdingAPI.Utilities;


namespace RingOfImmortality
{
    internal class PlayerStateRestorer
    {

        internal class PlayerDataTracker
        {

            public int money;

            public PlayerDataTracker(int money) => this.money = money;
        }

        internal static readonly PerScreen<PlayerDataTracker> statePassout = new PerScreen<PlayerDataTracker>(createNewState: () => null);

        public static void Save() => statePassout.Value = new PlayerDataTracker(Game1.player.Money);

        public static void Load()
        {
            Farmer player = Game1.player;

            player.Money = statePassout.Value.money;

            foreach (Item item in player.itemsLostLastDeath)
            {
                if (player.isInventoryFull() == true)
                {
                    player.dropItem(item);
                }
                else
                {
                    player.addItemToInventory(item);
                }
            }

            player.itemsLostLastDeath.Clear();
        }
    }
}