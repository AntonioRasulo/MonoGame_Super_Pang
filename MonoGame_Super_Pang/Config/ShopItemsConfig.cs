using MonoGame_Super_Pang.UI;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.Config;

public enum ShopItems
{
    HARPOON,
    SPEED,
    LIVES,
    COLL_LIVES
}

public class ShopItemsConfig
{
    public static readonly Dictionary<ShopItems, Dictionary<PowerUpButtonState, int>> prizes = new()
    {
        {
            ShopItems.HARPOON,
            new()
            {
                {PowerUpButtonState.Level1, 200},
                {PowerUpButtonState.Level2, 400}
            }
        },
        {
            ShopItems.SPEED,
            new()
            {
                {PowerUpButtonState.Level1, 100},
                {PowerUpButtonState.Level2, 300}
            }
        },
        {
            ShopItems.LIVES,
            new()
            {
                {PowerUpButtonState.Level1, 200},
                {PowerUpButtonState.Level2, 400}
            }
        },
        {
            ShopItems.COLL_LIVES,
            new()
            {
                {PowerUpButtonState.Level0, 100},
                {PowerUpButtonState.Level1, 200},
                {PowerUpButtonState.Level2, 300}
            }
        }
    };

    public static readonly Dictionary<ShopItems, string> itemsText = new ()
    {
        {ShopItems.HARPOON, "harpoon"},
        {ShopItems.SPEED, "speed"},
        {ShopItems.LIVES, "lives"},
        {ShopItems.COLL_LIVES, "lives probability"}
    };

}