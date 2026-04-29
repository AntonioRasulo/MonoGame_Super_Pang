using MonoGame_Super_Pang.UI;

namespace MonoGame_Super_Pang.Config;

public class PlayerStatsPowerUpState
{
    public PowerUpButtonState harpoonLevel;
    public PowerUpButtonState speedLevel;
    public PowerUpButtonState livesLevel;

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
                }
            };
        }

        return returnState;
    }
}
