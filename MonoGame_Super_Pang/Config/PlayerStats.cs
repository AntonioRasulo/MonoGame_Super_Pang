using System;
using System.IO;
using System.Text.Json;

namespace MonoGame_Super_Pang.Config;

public class PlayerStats
{
    public string Name{ get; set;}
    public int Money{ get; set;}
    public string Path{get;set;}

    /* Character stats */
    public int HarpoonNum{get; set;}

    public float Speed{get; set;}

    public int Lives{get; set;}

    /* Collectibles probability */
    public int LivesProb{get; set;}

    public int InvincibilityProb{get; set;}

    public int bombProbability{get; set;}
    public int clockProbability{get; set;}

    public PlayerStats(string name, string path)
    {
        Name = name;
        Money = 0;
        Path = path;
        HarpoonNum = 1;
        Speed = 5.0f;
        Lives = 1;
        LivesProb = 0;
        InvincibilityProb = 0;
        bombProbability = 0;
        clockProbability = 0;
    }

    public static PlayerStats LoadGame(string PATH)
    {
        string backupPath = PATH + ".bak";
        string jsonPath = PATH + ".json";
        PlayerStats stats = null;

        foreach (string candidate in new[] { jsonPath, backupPath })
        {
            if (!File.Exists(candidate)) continue;

            string raw = File.ReadAllText(candidate);

            // Reject null-byte corrupted files before handing to the deserializer
            if (string.IsNullOrWhiteSpace(raw) || raw.Contains('\0'))
            {
                continue;
            }

            try
            {
                // Try encrypted first, fall back to plain JSON (for old saves)
                string json;
                if (raw.TrimStart().StartsWith('{'))
                    json = raw;                          // legacy plain-text save
                else
                    json = SaveEncryption.Decrypt(raw);  // new encrypted save

                stats = JsonSerializer.Deserialize<PlayerStats>(json);
                break;
            }
            catch
            {
                // Corrupt or tampered — try next candidate
                continue;
            }

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
            string encrypted = SaveEncryption.Encrypt(json);
            File.WriteAllText(tempPath, encrypted);

            // Verify the temp file is valid before promoting it
            string written = File.ReadAllText(tempPath);
            string decrypted = SaveEncryption.Decrypt(written.Trim());
            JsonSerializer.Deserialize<PlayerStats>(decrypted); // throws if corrupt or null bytes

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