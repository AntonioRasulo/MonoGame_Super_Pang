using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.UI;

public class ControlPanel: PangPanel
{
    private AnimatedButton _backButton;

    public ControlPanel()
    {
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        _volumeButton.Anchor(Gum.Wireframe.Anchor.TopRight);
        _panel.AddChild(_volumeButton);

        GameText MovementText = InitializeText("Movement");
        MovementText.X = 10.0f;
        MovementText.Y = 50.0f;
        _panel.AddChild(MovementText);

        GameText ShootingText = InitializeText("Shooting");
        ShootingText.X = 10.0f;
        ShootingText.Y = 125.0f;
        _panel.AddChild(ShootingText);

        GameText KeyboardText = InitializeText("Keyboard");
        KeyboardText.X = 100.0f;
        KeyboardText.Y = 10.0f;
        _panel.AddChild(KeyboardText);

        GameText ControllerText = InitializeText("Controller");
        ControllerText.X = 200.0f;
        ControllerText.Y = 10.0f;
        _panel.AddChild(ControllerText);

        _backButton = new AnimatedButton(_GUIatlas);
        _backButton.Text = "BACK";
        _backButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _backButton.X = -28f;
        _backButton.Y = -10f;
        _backButton.Click += TitlePanelManager.HandleOptionsClicked;
        _panel.AddChild(_backButton);

        Texture2D keysTexture = Core.Content.Load<Texture2D>("images/UI/All keys and controller icons");
        TextureRegion spaceRegion = new TextureRegion(keysTexture, 50, 1, 10, 13);
        TextureRegion xRegion = new TextureRegion(keysTexture, 50, 1, 10, 13);
        TextureRegion aRegion = new TextureRegion(keysTexture, 50, 1, 10, 13);
        TextureRegion dRegion = new TextureRegion(keysTexture, 50, 1, 10, 13);
        TextureRegion leftRegion = new TextureRegion(keysTexture, 50, 1, 10, 13);
        TextureRegion rightRegion = new TextureRegion(keysTexture, 50, 1, 10, 13);

        SpriteRuntime spaceRegion = 

        Rectangle rect = new Rectangle()

        SpriteRuntime _sprite = new SpriteRuntime
        {
            Texture = keysTexture,
            SourceRectangle = sourceRectangle,
            WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            Width = sourceRectangle.Width,
            Height = sourceRectangle.Height,
            TextureAddress = Gum.Managers.TextureAddress.Custom,
            Rotation = degreeRotation
        };
    }

    public new void SetIsVisible(bool isVisible)
    {
        base.SetIsVisible(isVisible);
        _backButton.IsFocused = isVisible;
    }

    private GameText InitializeText(string text)
    {
        GameText Text = new GameText(text);
        Text.FontScale = 0.27f;
        Text.Red = 0;
        Text.Blue = 0;
        Text.Green = 0;
        Text.Anchor(Gum.Wireframe.Anchor.TopLeft);
        return Text;
    }

    private SpriteRuntime GenerateSprite()
    {
        SpriteRuntime sprite = new SpriteRuntime();

        return sprite;
    }

}