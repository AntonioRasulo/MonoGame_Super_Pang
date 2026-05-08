using Gum.Forms.Controls;
using MonoGameGum;

namespace MonoGame_Super_Pang.UI;

public class CreditsPanel : PangPanel
{
    AnimatedButton _backButton;

    public CreditsPanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        _backButton = new AnimatedButton(_GUIatlas);
        _backButton = new AnimatedButton(_GUIatlas);
        _backButton.Text = "BACK";
        _backButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _backButton.X = -28f;
        _backButton.Y = -10f;
        _backButton.Click += TitlePanelManager.HandleOptionsButtonBack;
        _panel.AddChild(_backButton);
    }

    public new void SetIsVisible(bool isVisible)
    {
        base.SetIsVisible(isVisible);
        _backButton.IsFocused = true;
    }
}