using MonoGame_Super_Pang.UI;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.Config;

public enum ShopItems
{
    HARPOON
}

public class ShopItemsConfig
{
    public static readonly Dictionary<ShopItems, Dictionary<PowerUpButtonState, int>> prizes = new()
    {
        {
            ShopItems.HARPOON,
            new()
            {
                {PowerUpButtonState.Level1, 100},
                {PowerUpButtonState.Level2, 300}
            }
        }
    };

    public static readonly Dictionary<ShopItems, string> itemsText = new ()
    {
        {ShopItems.HARPOON, "harpoon"}
    };

}