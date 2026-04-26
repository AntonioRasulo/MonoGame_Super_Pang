using System;
using System.IO;
using System.Text.Json;

namespace MonoGame_Super_Pang.Config;

public class PlayerStats
{
    public string Name{ get; set;}
    public int Money{ get; set;}
    public string Path{get;set;}

    public int HarpoonNum{get; set;}

    public PlayerStats(string name, string path)
    {
        Money = 0;
        HarpoonNum = 1;
        Name = name;
        Path = path;
    }

    public static PlayerStats LoadGame(string PATH)
    {
        string backupPath = PATH + ".bak";
        string jsonPath = PATH + ".json";
        PlayerStats stats = null;

        foreach (string candidate in new[] { jsonPath, backupPath })
        {
            if (!File.Exists(candidate)) continue;

            string json = File.ReadAllText(candidate);

            // Reject null-byte corrupted files before handing to the deserializer
            if (string.IsNullOrWhiteSpace(json) || json.Contains('\0'))
            {
                continue;
            }

            stats = JsonSerializer.Deserialize<PlayerStats>(json);

            break;

        }

        return stats;
    }

    public static void SaveGame(PlayerStats pStats)
    {
        string finalPath  = pStats.Path + ".json";
        string tempPath   = pStats.Path + ".tmp";
        string backupPath = pStats.Path + ".bak";

        try
        {
            // Write to temp file first
            string json = JsonSerializer.Serialize<PlayerStats>(pStats);
            File.WriteAllText(tempPath, json);

            // Verify the temp file is valid before promoting it
            string written = File.ReadAllText(tempPath);
            JsonSerializer.Deserialize<PlayerStats>(written); // throws if corrupt or null bytes

            // Rotate current save → backup
            if (File.Exists(finalPath))
                File.Copy(finalPath, backupPath, overwrite: true);

            // Promote temp → final
            File.Move(tempPath, finalPath, overwrite: true);
        }
        catch (Exception)
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

}