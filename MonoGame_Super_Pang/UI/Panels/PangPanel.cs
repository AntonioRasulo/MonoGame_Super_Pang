using Gum.Forms.Controls;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.UI;

public abstract class PangPanel
{
    protected Panel _panel;

    protected static TextureAtlas _GUIatlas;

    public static void LoadContent()
    {
        // Load the texture atlas from the xml configuration file.
        _GUIatlas = TextureAtlas.FromFile(Core.Content, "images/UI/GUI_atlas.xml");
    }

    public bool IsVisible()
    {
        return _panel.IsVisible;
    }

    public void SetIsVisible(bool isVisible)
    {
        _panel.IsVisible = isVisible;
    }

    public void AddChild(LoadButton child)
    {
        _panel.AddChild(child);
    }

    public virtual void Update()
    {
        
    }

    public Gum.Wireframe.InteractiveGue Visual()
    {
        return _panel.Visual;
    }

}