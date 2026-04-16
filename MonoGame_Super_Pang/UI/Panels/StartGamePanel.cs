using Gum.Forms.Controls;
using MonoGameGum;

namespace MonoGame_Super_Pang.UI;

public class StartGamePanel : PangPanel
{
    AnimatedButton _startGameButton;
    AnimatedButton _shopButton;
    AnimatedButton _backButton;

    public StartGamePanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        _startGameButton = new AnimatedButton(_GUIatlas);
        _startGameButton.Text = "START";
        _startGameButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _startGameButton.X = 28f;
        _startGameButton.Y = -10f;
        //_startGameButton.Click += ;
        _panel.AddChild(_startGameButton);

        _shopButton = new AnimatedButton(_GUIatlas);
        _shopButton.Text = "SHOP";
        _shopButton.Anchor(Gum.Wireframe.Anchor.Bottom);
        _shopButton.X = 0f;
        _shopButton.Y = -10f;
        //_shopButton.Click += ;
        _panel.AddChild(_shopButton);

        _backButton = new AnimatedButton(_GUIatlas);
        _backButton.Text = "BACK";
        _backButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _backButton.X = -28f;
        _backButton.Y = -10f;
        //_backButton.Click += ;
    }

    public override void Update()
    {
        if(_panel.IsVisible == true &&
        _startGameButton.IsFocused == false &&
        _shopButton.IsFocused == false &&
        _backButton.IsFocused == false)
        {
            _startGameButton.IsFocused = true;
        }
    }
}