using Gum.Forms.Controls;
using MonoGameGum;

namespace MonoGame_Super_Pang.UI;

public class TitleScreenButtonsPanel : PangPanel
{
    private AnimatedButton _optionsButton;

    public TitleScreenButtonsPanel()
    {
        // Create a container to hold all of our buttons
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.AddToRoot();

        var startButton = new AnimatedButton(_GUIatlas);
        startButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        startButton.X = 50;
        startButton.Y = -12;
        startButton.Width = 70;
        startButton.Text = "Start";
        startButton.Click += TitlePanelManager.HandleStartClicked;
        _panel.AddChild(startButton);

        _optionsButton = new AnimatedButton(_GUIatlas);
        _optionsButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _optionsButton.X = -50;
        _optionsButton.Y = -12;
        _optionsButton.Width = 70;
        _optionsButton.Text = "Options";
        _optionsButton.Click += TitlePanelManager.HandleOptionsClicked;
        _panel.AddChild(_optionsButton);

        startButton.IsFocused = true;
    }

}