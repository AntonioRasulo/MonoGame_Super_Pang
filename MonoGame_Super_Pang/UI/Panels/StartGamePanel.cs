using Gum.DataTypes;
using Gum.Managers;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGame_Super_Pang.Scenes;
using MonoGame_Super_Pang.Config;
using System;

namespace MonoGame_Super_Pang.UI;

public class StartGamePanel : PangPanel
{
    private AnimatedButton _startGameButton;
    private AnimatedButton _shopButton;
    private AnimatedButton _backButton;
    TextRuntime _text;

    public StartGamePanel()
    {
        _panel.Anchor(Gum.Wireframe.Anchor.Center);
        _panel.IsVisible = false;
        _panel.AddToRoot();
        _panel.WidthUnits = DimensionUnitType.Absolute;
        _panel.HeightUnits = DimensionUnitType.Absolute;
        _panel.Width = 264.0f;
        _panel.Height = 70.0f;

        TextureRegion backgroundRegion = _GUIatlas.GetRegion("panel-background");

        NineSliceRuntime background = new NineSliceRuntime();
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Texture = backgroundRegion.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.TextureHeight = backgroundRegion.Height;
        background.TextureWidth = backgroundRegion.Width;
        background.TextureTop = backgroundRegion.SourceRectangle.Top;
        background.TextureLeft = backgroundRegion.SourceRectangle.Left;
        _panel.AddChild(background);

        _text = new TextRuntime();
        _text.Anchor(Gum.Wireframe.Anchor.Top);
        _text.Text = "Ready to start?\n";
        _text.UseCustomFont = true;
        _text.FontScale = 0.25f;
        _text.CustomFontFile = @"fonts/04b_30.fnt";
        _text.IsEnabled = false;
        _text.Y = 5f;
        _panel.AddChild(_text);

        _startGameButton = new AnimatedButton(_GUIatlas);
        _startGameButton.Text = "START";
        _startGameButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _startGameButton.X = 9f;
        _startGameButton.Y = -9f;
        _startGameButton.Click += StartGame;
        _startGameButton.GotFocus += SetText;
        _panel.AddChild(_startGameButton);

        _shopButton = new AnimatedButton(_GUIatlas);
        _shopButton.Text = "SHOP";
        _shopButton.Anchor(Gum.Wireframe.Anchor.Bottom);
        _shopButton.X = 0f;
        _shopButton.Y = -9f;
        _shopButton.Click += TitlePanelManager.GoToShopPanel;
        _shopButton.GotFocus += SetText;
        _panel.AddChild(_shopButton);

        _backButton = new AnimatedButton(_GUIatlas);
        _backButton.Text = "BACK";
        _backButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _backButton.X = -9f;
        _backButton.Y = -9f;
        _backButton.Click += TitlePanelManager.HandleBackStartGameClicked;
        _backButton.GotFocus += SetText;
        _panel.AddChild(_backButton);
    }

    private void StartGame(object sender, EventArgs e)
    {
        // Change to the game scene to start the game.
        Core.ChangeScene(new GameScene(LevelConfig.STARTING_LEVEL));
    }

    private void SetText(object sender, EventArgs e)
    {
        if(sender == _startGameButton)
        {
            _text.Text = "Ready to start?";
        }
        else if(sender == _shopButton)
        {
            _text.Text = "You can find some interesting\n";
            _text.Text += "power up in the shop";
        }
        else if(sender == _backButton)
        {
            _text.Text = "Go back to load another saving!";
        }
    }

    public new void SetIsVisible(bool isVisible)
    {
        base.SetIsVisible(isVisible);
        _shopButton.IsFocused = isVisible;
    }
}