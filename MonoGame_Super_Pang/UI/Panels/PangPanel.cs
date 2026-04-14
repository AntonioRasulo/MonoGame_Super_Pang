using Gum.Forms.Controls;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.UI;

public abstract class PangPanel
{
    protected Panel _panel;

    protected static TextureAtlas _GUIatlas;

    // public static SoundEffect _uiSoundEffect;

    // private static void LoadContent()
    // {
    //     // Load the texture atlas from the xml configuration file.
    //     _GUIatlas = TextureAtlas.FromFile(Core.Content, "images/GUI_atlas.xml");
    // }

    public bool IsVisible()
    {
        return _panel.IsVisible;
    }

    public void SetIsVisible(bool isVisible)
    {
        _panel.IsVisible = isVisible;
    }

}