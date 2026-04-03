using System.IO;
using System.Text.Json;

namespace MonoGame_Super_Pang.Config;

public class PlayerStats
{
    public string Name{ get; set;}
    public int Money{ get; set;}
    public string Path{get;set;}

    public static PlayerStats LoadGame(string PATH)
    {
        string fileContent;

        if (File.Exists(PATH))
        {
            fileContent = File.ReadAllText(PATH);
            return JsonSerializer.Deserialize<PlayerStats>(fileContent);
        }

        return null;
    }

    public static void SaveGame(PlayerStats pStats)
    {
        string serializedText = JsonSerializer.Serialize<PlayerStats>(pStats);
        File.WriteAllText(pStats.Path, serializedText);
    }

}