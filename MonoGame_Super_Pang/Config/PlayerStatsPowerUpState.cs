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
    public PowerUpButtonState clockLevel;

    public static PlayerStatsPowerUpState GetPlayerPowerUpState(PlayerStats playerStats)
    {

        PlayerStatsPowerUpState returnState = null;

        if (playerStats != null)
        {
            returnState = new PlayerStatsPowerUpState
            {
                harpoonLevel = playerStats.HarpoonNum switch
                {
                    1 => PowerUpButtonState.Level1,
                    2 => PowerUpButtonState.Level2,
                    3 => PowerUpButtonState.Level3,
                    _ => PowerUpButtonState.Level1
                },
                speedLevel = playerStats.Speed switch
                {
                    5.0f => PowerUpButtonState.Level1,
                    6.5f => PowerUpButtonState.Level2,
                    8.0f => PowerUpButtonState.Level3,
                    _ => PowerUpButtonState.Level1
                },
                livesLevel = playerStats.Lives switch
                {
                    1 => PowerUpButtonState.Level1,
                    2 => PowerUpButtonState.Level2,
                    3 => PowerUpButtonState.Level3,
                    _ => PowerUpButtonState.Level1
                },
                collLivesLevel = playerStats.LivesProb switch
                {
                    0 => PowerUpButtonState.Level0,
                    3 => PowerUpButtonState.Level1,
                    4 => PowerUpButtonState.Level2,
                    5 => PowerUpButtonState.Level3,
                    _ => PowerUpButtonState.Level0
                },
                clockLevel = playerStats.clockProbability switch
                {
                    0 => PowerUpButtonState.Level0,
                    8 => PowerUpButtonState.Level1,
                    9 => PowerUpButtonState.Level2,
                    10 => PowerUpButtonState.Level3,
                    _ => PowerUpButtonState.Level0
                },
                invincibilityLevel = playerStats.InvincibilityProb switch
                {
                    0 => PowerUpButtonState.Level0,
                    13 => PowerUpButtonState.Level1,
                    14 => PowerUpButtonState.Level2,
                    15 => PowerUpButtonState.Level3,
                    _ => PowerUpButtonState.Level0
                },
                bombLevel = playerStats.bombProbability switch
                {
                    0 => PowerUpButtonState.Level0,
                    18 => PowerUpButtonState.Level1,
                    19 => PowerUpButtonState.Level2,
                    20 => PowerUpButtonState.Level3,
                    _ => PowerUpButtonState.Level0
                }
            };
        }

        return returnState;
    }
}
