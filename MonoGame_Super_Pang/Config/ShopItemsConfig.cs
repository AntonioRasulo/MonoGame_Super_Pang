using MonoGame_Super_Pang.UI;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.Config;

public enum ShopItems
{
    HARPOON,
    SPEED,
    LIVES,
    COLL_LIVES,
    INVINCIBILITY,
    BOMB,
    CLOCK
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
        },
        {
            ShopItems.INVINCIBILITY,
            new()
            {
                {PowerUpButtonState.Level0, 100},
                {PowerUpButtonState.Level1, 200},
                {PowerUpButtonState.Level2, 300}
            }
        },
        {
            ShopItems.BOMB,
            new()
            {
                {PowerUpButtonState.Level0, 100},
                {PowerUpButtonState.Level1, 200},
                {PowerUpButtonState.Level2, 300}
            }
        },
        {
            ShopItems.CLOCK,
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
        {ShopItems.COLL_LIVES, "lives probability"},
        {ShopItems.INVINCIBILITY, "invincibility probability"},
        {ShopItems.BOMB, "bomb probability"},
        {ShopItems.CLOCK, "clock probability"}
    };

    public static readonly Dictionary<ShopItems, string> itemsDescriptions = new()
    {
        {ShopItems.HARPOON, "Increase the number of harpoon that the\nchracter can shoot simultaneously."},
        {ShopItems.SPEED, "Increase the speed of the character."},
        {ShopItems.LIVES, "Increase the number of initial lives."},
        {ShopItems.COLL_LIVES, "Probability that baloons and monsters drop\nlive power up when get shot."},
        {ShopItems.INVINCIBILITY, "Probability that baloons and monsters drop\ninvincibility power up when get shot.\nThis power up makes the character\ninvincible for 3 seconds."},
        {ShopItems.BOMB, "Probability that baloons and monsters drop\nbomb power up when get shot.\nThis power up hits all the baloons and monsters\non the fields."},
        {ShopItems.CLOCK, "Probability that baloons and monsters drop\nclock power up when get shot.\nThis power up freezes all the baloons and monsters\nfor 4 seconds."}
    };

}