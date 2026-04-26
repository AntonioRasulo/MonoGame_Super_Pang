using MonoGame_Super_Pang.UI;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.Config;

public enum ShopItems
{
    HARPOON,
    SPEED
}

public class ShopItemsConfig
{
    public static readonly Dictionary<ShopItems, Dictionary<PowerUpButtonState, int>> prizes = new()
    {
        {
            ShopItems.HARPOON,
            new()
            {
                {PowerUpButtonState.Level1, 300},
                {PowerUpButtonState.Level2, 600}
            }
        },
        {
            ShopItems.SPEED,
            new()
            {
                {PowerUpButtonState.Level1, 200},
                {PowerUpButtonState.Level2, 400}
            }
        }
    };

    public static readonly Dictionary<ShopItems, string> itemsText = new ()
    {
        {ShopItems.HARPOON, "harpoon"},
        {ShopItems.SPEED, "speed"}
    };

}