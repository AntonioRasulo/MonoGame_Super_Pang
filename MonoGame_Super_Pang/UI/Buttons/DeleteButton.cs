using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;

namespace MonoGame_Super_Pang.UI;

public class DeleteButton : Button
{
    private SpriteRuntime _sprite;

    public DeleteButton()
    {
        CreateSprite();

        Click += PlaySound;

        GotFocus += MoveFocus;
        this.GamepadTabbingFocusBehavior = TabbingFocusBehavior.SkipOnTab;
    }

    private void CreateSprite()
    {

        TextureAtlas book2Atlas = TextureAtlas.FromFile(Core.Content, "images/UI/Book2_atlas.xml");

        TextureRegion x_icon = book2Atlas.GetRegion("x-icon");
        Texture2D texture = x_icon.Texture;
        Rectangle sourceRectangle = x_icon.SourceRectangle;

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

        visual.Children.Insert(0, _sprite);
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

    private void PlaySound(object sender, EventArgs e)
    {
        Core.Audio.PlaySoundEffect(TitlePanelManager.uiSoundEffect);
    }

    private void MoveFocus(object sender, EventArgs args)
    {
        this.HandleKeyboardFocusUpdate();
    }

}