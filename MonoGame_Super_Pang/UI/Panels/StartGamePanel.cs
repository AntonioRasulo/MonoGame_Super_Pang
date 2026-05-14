using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Input;
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

    private bool _isLastFocusedBackButton = false;

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
        _startGameButton.KeyDown += updateFlagButton;
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
        _backButton.KeyDown += updateFlagButton;
        _panel.AddChild(_backButton);
    }

    public override void Update()
    {
        if(_panel.IsVisible == true &&
        _startGameButton.IsFocused == false &&
        _shopButton.IsFocused == false &&
        _backButton.IsFocused == false)
        {
            if (_isLastFocusedBackButton)
            {
                _startGameButton.IsFocused = true;
            }
            else
            {
                _backButton.IsFocused = true;
            }
        }
    }

    private void StartGame(object sender, EventArgs e)
    {
        // Change to the game scene to start the game.
        Core.ChangeScene(new GameScene(LevelConfig.STARTING_LEVEL));
    }

    private void updateFlagButton(Object sender, KeyEventArgs e)
    {
        if(sender == _startGameButton)
        {
            if (e.Key == Keys.Up || e.Key == Keys.Left)
            {
                _isLastFocusedBackButton = false;
            }
        }
        else if(sender == _backButton)
        {
            if (e.Key == Keys.Down || e.Key == Keys.Right)
            {
                _isLastFocusedBackButton = true;
            }
        }
    }

}