using System;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.GueDeriving;

namespace MonoGame_Super_Pang.UI;

public class TextureButton : Button
{
    private SpriteRuntime _sprite;
    private TextRuntime _textMoney;

    public bool isNewGame{get;set;}

    public TextureButton(Texture2D texture, Rectangle sourceRectangle)
    {
        CreateSprite(texture, sourceRectangle);
    }

    private void CreateSprite(Texture2D texture, Rectangle sourceRectangle)
    {
        // Access the visual
        ButtonVisual visual = (ButtonVisual)this.Visual;

        // Remove default background (NineSlice)
        if (visual.Background != null)
        {
            visual.Children.Remove(visual.Background);
        }

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
        textInstance.Text = "NewGame";
        textInstance.Anchor(Gum.Wireframe.Anchor.Top);
        textInstance.Y = 10;
        textInstance.Color = Color.Black;
        _textMoney = new TextRuntime();
        _textMoney.Text = "";
        _textMoney.Anchor(Gum.Wireframe.Anchor.Bottom);

        visual.Children.Insert(0, _sprite);
        visual.Children.Insert(1, _textMoney);

        isNewGame = true;

        // Add event handlers for keyboard input.
        KeyDown += HandleKeyDown;

        // // Add event handler for mouse hover focus.
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

        // Scale sprite (visual)
        _sprite.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        _sprite.HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        _sprite.Width = newWidth;
        _sprite.Height = newHeight;

    }

    public void setTextMoney(string textMoney)
    {
        _textMoney.Text = textMoney;
    }

    /// <summary>
    /// Handles keyboard input for navigation between buttons using left/right keys.
    /// </summary>
    private void HandleKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Keys.Left)
        {
            // Left arrow navigates to previous control
            HandleTab(TabDirection.Up, loop: true);
        }
        if (e.Key == Keys.Right)
        {
            // Right arrow navigates to next control
            HandleTab(TabDirection.Down, loop: true);
        }
    }

    /// <summary>
    /// Automatically focuses the button when the mouse hovers over it.
    /// </summary>
    private void HandleRollOn(object sender, EventArgs e)
    {
        IsFocused = true;
    }
}