using MonoGame_Super_Pang.UI;

namespace MonoGame_Super_Pang.Config;

public class PlayerStatsPowerUpState
{
    /* Character power up */
    public PowerUpButtonState harpoonLevel;
    public PowerUpButtonState speedLevel;
    public PowerUpButtonState livesLevel;

    /* Collectibles power up */
    public PowerUpButtonState collLivesLevel;
    public PowerUpButtonState invincibilityLevel;
    public PowerUpButtonState bombLevel;

    public static PlayerStatsPowerUpState getCurrentStatePowerUpLevel()
    {

        PlayerStatsPowerUpState returnState = null;

        if (PlayerStatsManager.currentStats != null)
        {
            returnState = new PlayerStatsPowerUpState
            {
                harpoonLevel = PlayerStatsManager.currentStats.HarpoonNum switch
                {
                    1 => PowerUpButtonState.Level1,
                    2 => PowerUpButtonState.Level2,
                    3 => PowerUpButtonState.Level3
                },
                speedLevel = PlayerStatsManager.currentStats.Speed switch
                {
                    5.0f => PowerUpButtonState.Level1,
                    6.5f => PowerUpButtonState.Level2,
                    8.0f => PowerUpButtonState.Level3 
                },
                livesLevel = PlayerStatsManager.currentStats.Lives switch
                {
                    1 => PowerUpButtonState.Level1,
                    2 => PowerUpButtonState.Level2,
                    3 => PowerUpButtonState.Level3
                },
                collLivesLevel = PlayerStatsManager.currentStats.LivesProb switch
                {
                    0 => PowerUpButtonState.Level0,
                    3 => PowerUpButtonState.Level1,
                    4 => PowerUpButtonState.Level2,
                    5 => PowerUpButtonState.Level3
                },
                invincibilityLevel = PlayerStatsManager.currentStats.InvincibilityProb switch
                {
                    0 => PowerUpButtonState.Level0,
                    13 => PowerUpButtonState.Level1,
                    14 => PowerUpButtonState.Level2,
                    15 => PowerUpButtonState.Level3
                },
                bombLevel = PlayerStatsManager.currentStats.bombProbability switch
                {
                    0 => PowerUpButtonState.Level0,
                    18 => PowerUpButtonState.Level1,
                    19 => PowerUpButtonState.Level2,
                    20 => PowerUpButtonState.Level3
                }
            };
        }

        return returnState;
    }
}
