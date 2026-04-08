using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Super_Pang.Backgrounds;
using MonoGameLibrary;

namespace MonoGame_Super_Pang.Config;

public static class BackgroundRegistry
{
    public static List<Background> backgrounds;

    private static readonly List<LevelBackgrounds> AllBackgrounds = new List<LevelBackgrounds>
    {
        new LevelBackgrounds
        {
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds1/1",
                "images/backgrounds/clouds1/2",
                "images/backgrounds/clouds1/3",
                "images/backgrounds/clouds1/4"
            }
        },
        new LevelBackgrounds
        {
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds2/1",
                "images/backgrounds/clouds2/2",
                "images/backgrounds/clouds2/3",
                "images/backgrounds/clouds2/4"
            }
        },
        new LevelBackgrounds
        {
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds3/1",
                "images/backgrounds/clouds3/2",
                "images/backgrounds/clouds3/3",
                "images/backgrounds/clouds3/4"
            }
        },
        new LevelBackgrounds
        {
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds4/1",
                "images/backgrounds/clouds4/2",
                "images/backgrounds/clouds4/3",
                "images/backgrounds/clouds4/4"
            }
        },
        new LevelBackgrounds
        {
            backgroundStr = new List<string>
            {
                "images/backgrounds/clouds5/1",
                "images/backgrounds/clouds5/2",
                "images/backgrounds/clouds5/3",
                "images/backgrounds/clouds5/4",
                "images/backgrounds/clouds5/5",
            }
        }
    };

    public static void LoadContent()
    {
        backgrounds = new List<Background>();
        foreach(LevelBackgrounds levelBg in AllBackgrounds)
        {
            List<Texture2D> clouds = new List<Texture2D>();

            foreach (string backgroundStr in levelBg.backgroundStr)
            {
                clouds.Add(Core.Content.Load<Texture2D>(backgroundStr));
            }

            backgrounds.Add(new Background(clouds));
        }
    }
}