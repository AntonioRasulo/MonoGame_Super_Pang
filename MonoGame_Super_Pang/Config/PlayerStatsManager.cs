using System;
using System.IO;
using MonoGame_Super_Pang.UI;

namespace MonoGame_Super_Pang.Config;

public class PlayerStatsManager
{
    public static readonly string saveDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MonoGame_Super_Pang",
        "saves"
    );

    public static readonly string PATH1 = saveDirectory + "/pStats1";
    public static readonly string PATH2 = saveDirectory + "/pStats2";
    public static readonly string PATH3 = saveDirectory + "/pStats3";

    public static PlayerStats pStats1;
    public static PlayerStats pStats2;
    public static PlayerStats pStats3;

    public static PlayerStats currentStats;
    public static PlayerStatsPowerUpState currentStatsLevels;

    public static void LoadContent()
    {
        pStats1 = PlayerStats.LoadGame(PATH1);
        pStats2 = PlayerStats.LoadGame(PATH2);
        pStats3 = PlayerStats.LoadGame(PATH3);
        currentStats = null;
    }

    public static void SelectPlayerStats(int stats)
    {
        currentStats = stats switch
        {
            1 => pStats1,
            2 => pStats2,
            3 => pStats3,
            _ => null
        };

        currentStatsLevels = PlayerStatsPowerUpState.getCurrentStatePowerUpLevel();

        if( currentStats != null)
        {
            ShopPanel.InitializePowerUpButtons();
        }
    }

    public static void SetPlayerStats(PowerUpButtonState state, ShopItems itemType)
    {
        switch (itemType)
        {
            case ShopItems.HARPOON:
            currentStatsLevels.harpoonLevel = state;
            currentStats.HarpoonNum++;
            break;
            case ShopItems.SPEED:
            currentStatsLevels.speedLevel = state;
                switch (state)
                {
                    case PowerUpButtonState.Level2:
                        currentStats.Speed = 6.5f;
                        break;
                    case PowerUpButtonState.Level3:
                        currentStats.Speed = 8.0f;
                        break;
                }
            break;
            case ShopItems.LIVES:
            currentStatsLevels.livesLevel = state;
            currentStats.Lives++;
            break;
            case ShopItems.COLL_LIVES:
                currentStatsLevels.collLivesLevel = state;
                switch (state)
                {
                    case PowerUpButtonState.Level0:
                    currentStats.LivesProb = 0;
                    break;
                    case PowerUpButtonState.Level1:
                    currentStats.LivesProb = 3;
                    break;
                    case PowerUpButtonState.Level2:
                    currentStats.LivesProb = 4;
                    break;
                    case PowerUpButtonState.Level3:
                    currentStats.LivesProb = 5;
                    break;
                }
            break;
            case ShopItems.INVINCIBILITY:
                currentStatsLevels.invincibilityLevel = state;
                switch (state)
                {
                    case PowerUpButtonState.Level0:
                    currentStats.InvincibilityProb = 0;
                    break;
                    case PowerUpButtonState.Level1:
                    currentStats.InvincibilityProb = 13;
                    break;
                    case PowerUpButtonState.Level2:
                    currentStats.InvincibilityProb = 14;
                    break;
                    case PowerUpButtonState.Level3:
                    currentStats.InvincibilityProb = 15;
                    break;
                }
            break;
            case ShopItems.BOMB:
                currentStatsLevels.bombLevel = state;
                switch (state)
                {
                    case PowerUpButtonState.Level0:
                    currentStats.bombProbability = 0;
                    break;
                    case PowerUpButtonState.Level1:
                    currentStats.bombProbability = 18;
                    break;
                    case PowerUpButtonState.Level2:
                    currentStats.bombProbability = 19;
                    break;
                    case PowerUpButtonState.Level3:
                    currentStats.bombProbability = 20;
                    break;
                }
            break;
        }

        PlayerStats.SaveGame(currentStats);

    }

}