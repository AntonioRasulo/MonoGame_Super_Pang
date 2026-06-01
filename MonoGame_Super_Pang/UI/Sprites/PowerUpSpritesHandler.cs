using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGame_Super_Pang.Config;
using System.Collections.Generic;
using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Super_Pang.UI; 

public static class PowerUpSpritesHandler
{
    private static Dictionary<ShopItems, TextureRegion> _textureRegions;
    private static TextureRegion _Xicon;
    private static TextureRegion _Vicon;

    public static void LoadContent()
    {
        _textureRegions = new Dictionary<ShopItems, TextureRegion>();

        TextureAtlas book2Atlas = TextureAtlas.FromFile(Core.Content, "images/UI/Book2_atlas.xml");

        _Xicon = book2Atlas.GetRegion("x-icon");
        _Vicon = book2Atlas.GetRegion("v-icon");

        TextureAtlas _itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/Items/items-atlas.xml");

        TextureRegion _harpoonRegion = _itemsAtlas.GetRegion("harpoonTexture");

        _textureRegions.Add(ShopItems.HARPOON, _harpoonRegion);

        Texture2D speed2DTexture = Core.Content.Load<Texture2D>("images/UI/white_wings");
        TextureRegion speedRegion = new TextureRegion(speed2DTexture, 0, 0, speed2DTexture.Width, speed2DTexture.Height);

        _textureRegions.Add(ShopItems.SPEED, speedRegion);

        TextureRegion livesRegion = _itemsAtlas.GetRegion("livesSprite");
        _textureRegions.Add(ShopItems.LIVES, livesRegion);
        _textureRegions.Add(ShopItems.COLL_LIVES, livesRegion);
    }

    public static TextureRegion GetTextureRegion(ShopItems shopItem)
    {
        return _textureRegions[shopItem];
    }

    public static (SpriteRuntime, PowerUpButtonState) GetSpriteRuntime(ShopItems shopItem, PlayerStats pStats = null, float scale = 1.0f)
    {
        PowerUpButtonState iconState;
        SpriteRuntime spriteReturn = new SpriteRuntime
        {
            Texture = _textureRegions[shopItem].Texture,
            SourceRectangle = _textureRegions[shopItem].SourceRectangle,
            WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            Width = _textureRegions[shopItem].SourceRectangle.Width * scale,
            Height = _textureRegions[shopItem].SourceRectangle.Height * scale,
            TextureAddress = Gum.Managers.TextureAddress.Custom,
        };
        (spriteReturn.Color, iconState )= PlayerStatsManager.GetPowerUpStatus(shopItem, pStats);
        return (spriteReturn, iconState);
    }

    public static SpriteRuntime GetXSprite()
    {
        Texture2D texture = _Xicon.Texture;
        Rectangle sourceRectangle = _Xicon.SourceRectangle;

        // Create sprite
        return new SpriteRuntime
        {
            Texture = texture,
            SourceRectangle = sourceRectangle,
            WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            Width = sourceRectangle.Width,
            Height = sourceRectangle.Height,
            TextureAddress = Gum.Managers.TextureAddress.Custom
        };
    }

    public static SpriteRuntime GetVSprite()
    {
        Texture2D texture = _Vicon.Texture;
        Rectangle sourceRectangle = _Vicon.SourceRectangle;

        // Create sprite
        return new SpriteRuntime
        {
            Texture = texture,
            SourceRectangle = sourceRectangle,
            WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            Width = sourceRectangle.Width,
            Height = sourceRectangle.Height,
            TextureAddress = Gum.Managers.TextureAddress.Custom
        };
    }
}