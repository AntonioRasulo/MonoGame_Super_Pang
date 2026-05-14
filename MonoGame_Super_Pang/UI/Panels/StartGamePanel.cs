using MonoGameGum;
using MonoGameLibrary;
using MonoGame_Super_Pang.Scenes;
using MonoGame_Super_Pang.Config;
using System;

namespace MonoGame_Super_Pang.UI;

public class StartGamePanel : PangPanel
{
    private AnimatedButton _startGameButton;
    private AnimatedButton _shopButton;
    private AnimatedButton _backButton;

    public StartGamePanel()
    {
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        _volumeButton.Anchor(Gum.Wireframe.Anchor.TopRight);
        _panel.AddChild(_volumeButton);

        _startGameButton = new AnimatedButton(_GUIatlas);
        _startGameButton.Text = "START";
        _startGameButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _startGameButton.X = 28f;
        _startGameButton.Y = -10f;
        _startGameButton.Click += StartGame;
        _panel.AddChild(_startGameButton);

        _shopButton = new AnimatedButton(_GUIatlas);
        _shopButton.Text = "SHOP";
        _shopButton.Anchor(Gum.Wireframe.Anchor.Bottom);
        _shopButton.X = 0f;
        _shopButton.Y = -10f;
        _shopButton.Click += TitlePanelManager.GoToShopPanel;
        _panel.AddChild(_shopButton);

        _backButton = new AnimatedButton(_GUIatlas);
        _backButton.Text = "BACK";
        _backButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _backButton.X = -28f;
        _backButton.Y = -10f;
        _backButton.Click += TitlePanelManager.HandleBackStartGameClicked;
        _panel.AddChild(_backButton);
    }

    private void StartGame(object sender, EventArgs e)
    {
        // Change to the game scene to start the game.
        Core.ChangeScene(new GameScene(LevelConfig.STARTING_LEVEL));
    }

    public new void SetIsVisible(bool isVisible)
    {
        base.SetIsVisible(isVisible);
        _shopButton.IsFocused = isVisible;
    }
}