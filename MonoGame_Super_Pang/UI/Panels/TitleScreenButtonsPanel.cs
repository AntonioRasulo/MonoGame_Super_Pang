using Gum.Forms.Controls;
using MonoGameGum;

namespace MonoGame_Super_Pang.UI;

public class TitleScreenButtonsPanel : PangPanel
{
    private AnimatedButton _optionsButton;
    private AnimatedButton _startButton;

    public TitleScreenButtonsPanel()
    {
        // Create a container to hold all of our buttons
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.AddToRoot();

        _startButton = new AnimatedButton(_GUIatlas);
        _startButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _startButton.X = 50;
        _startButton.Y = -12;
        _startButton.Width = 70;
        _startButton.Text = "Start";
        _startButton.Click += TitlePanelManager.HandleStartClicked;
        _panel.AddChild(_startButton);

        _optionsButton = new AnimatedButton(_GUIatlas);
        _optionsButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _optionsButton.X = -50;
        _optionsButton.Y = -12;
        _optionsButton.Width = 70;
        _optionsButton.Text = "Options";
        _optionsButton.Click += TitlePanelManager.HandleOptionsClicked;
        _panel.AddChild(_optionsButton);

    }

    public void SetOptionButtonFocus(bool IsFocused)
    {
        _optionsButton.IsFocused = IsFocused;
    }

    public override void Update()
    {
        if (_optionsButton.IsFocused == false &&
            _startButton.IsFocused == false &&
            _panel.IsVisible == true)
        {
            _startButton.IsFocused = true;
        }
    }

}