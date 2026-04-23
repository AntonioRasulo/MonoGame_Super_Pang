using System;
using System.IO;

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
    }

}