using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGame_Super_Pang.Config;
using System.Collections.Generic;
using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Super_Pang.UI; 

public static class PowerUpSpritesHandler
{
    private static Dictionary<ShopItems, TextureRegion> _textureRegions; 

    public static void LoadContent()
    {
        _textureRegions = new Dictionary<ShopItems, TextureRegion>();

        TextureAtlas _itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/Items/items-atlas.xml");

        TextureRegion _harpoonRegion = _itemsAtlas.GetRegion("harpoonTexture");

        _textureRegions.Add(ShopItems.HARPOON, _harpoonRegion);

        Texture2D speed2DTexture = Core.Content.Load<Texture2D>("images/UI/white_wings");
        TextureRegion speedRegion = new TextureRegion(speed2DTexture, 0, 0, speed2DTexture.Width, speed2DTexture.Height);

        _textureRegions.Add(ShopItems.SPEED, speedRegion);
    }

    public static TextureRegion GetTextureRegion(ShopItems shopItem)
    {
        return _textureRegions[shopItem];
    }

    public static SpriteRuntime GetSpriteRuntime(ShopItems shopItem, PlayerStats pStats = null, float scale = 1.0f)
    {
        return new SpriteRuntime
        {
            Texture = _textureRegions[shopItem].Texture,
            SourceRectangle = _textureRegions[shopItem].SourceRectangle,
            WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            Width = _textureRegions[shopItem].SourceRectangle.Width * scale,
            Height = _textureRegions[shopItem].SourceRectangle.Height * scale,
            TextureAddress = Gum.Managers.TextureAddress.Custom,
            Color = PlayerStatsManager.GetPowerUpColor(shopItem, pStats)
        };
    }
}