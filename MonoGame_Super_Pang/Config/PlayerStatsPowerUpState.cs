using MonoGame_Super_Pang.UI;

namespace MonoGame_Super_Pang.Config;

public class PlayerStatsPowerUpState
{
    public PowerUpButtonState harpoonLevel;

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
                }
            };
        }

        return returnState;
    }
}
