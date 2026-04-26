using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;
using MonoGameLibrary.Graphics;
using MonoGame_Super_Pang.Config;

namespace MonoGame_Super_Pang.UI;

public enum PowerUpButtonState
{
    Level1,
    Level2,
    Level3
}

public class PowerUpButton : AnimatedButton
{
    private SpriteRuntime _sprite;

    private float _scale;

    private PowerUpButtonState _state;

    private ShopItems _item;

    private const int WIDTH = 30;
    private const int HEIGHT = 20;

    public PowerUpButton(Texture2D texture, Rectangle sourceRectangle, TextureAtlas atlas, ShopItems item) : base(atlas)
    {
        CreateSprite(texture, sourceRectangle);
        SetState(PowerUpButtonState.Level1);
        Width = WIDTH;
        Height = HEIGHT;
        _item = item;
    }

    public PowerUpButton(PowerUpButton button, TextureAtlas atlas) : base(atlas)
    {
        CreateSprite(button._sprite.Texture, button._sprite.SourceRectangle);
        SetState(button._state);
        SetScale(button._scale);
        Width = WIDTH;
        Height = HEIGHT;
        _item = button._item;
    }

    private void CreateSprite(Texture2D texture, Rectangle sourceRectangle)
    {
        // Access the visual
        ButtonVisual visual = (ButtonVisual)this.Visual;

        // Create sprite
        _sprite = new SpriteRuntime
        {
            Texture = texture,
            SourceRectangle = sourceRectangle,
            WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            Width = sourceRectangle.Width,
            Height = sourceRectangle.Height,
            TextureAddress = Gum.Managers.TextureAddress.Custom
        };

        this.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        this.HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute;

        this.Width = sourceRectangle.Width;
        this.Height = sourceRectangle.Height;

        TextRuntime textInstance = visual.TextInstance;
        textInstance.Text = "";

        _sprite.Anchor(Gum.Wireframe.Anchor.Center);
        visual.Background.AddChild(_sprite);

        // Add event handlers for keyboard input.
        KeyDown += HandleKeyDown;

        // Add event handler for mouse hover focus.
        visual.RollOn += HandleRollOn;
    }

    public void SetScale(float scale)
    {
        var rect = _sprite.SourceRectangle;

        float newWidth = rect.Width * scale;
        float newHeight = rect.Height * scale;

        // Scale button (IMPORTANT for input)
        this.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        this.HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        this.Width = newWidth;
        this.Height = newHeight;

        ButtonVisual visual = (ButtonVisual)this.Visual;
        visual.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        visual.HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        visual.Width = newWidth;
        visual.Height = newHeight;

        // Scale sprite (visual)
        _sprite.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        _sprite.HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        _sprite.Width = newWidth;
        _sprite.Height = newHeight;

        _scale = scale;
    }

    public void LevelUp()
    {
        switch (_state)
        {
            case PowerUpButtonState.Level1:
            case PowerUpButtonState.Level2:
                _state++;
            break;
            case PowerUpButtonState.Level3:
            break;
        }
        SetSpriteColor();
    }

    public void SetState(PowerUpButtonState state)
    {
        _state = state;
        SetSpriteColor();
    }

    private void SetSpriteColor()
    {
        switch (_state)
        {
            case PowerUpButtonState.Level1:
                _sprite.Color = Color.SandyBrown;
            break;
            case PowerUpButtonState.Level2:
                _sprite.Color = Color.Silver;
            break;
            case PowerUpButtonState.Level3:
                _sprite.Color = Color.Gold;
            break;
        }
    }

    public int GetPowerUpPrize()
    {
        return ShopItemsConfig.prizes[_item][_state];
    }

    public PowerUpButtonState GetState()
    {
        return _state;
    }

    public ShopItems GetItem()
    {
        return _item;
    }

}