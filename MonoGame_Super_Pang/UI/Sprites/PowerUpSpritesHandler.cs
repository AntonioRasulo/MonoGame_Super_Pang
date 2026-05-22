using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGame_Super_Pang.Config;
using System.Collections.Generic;
using MonoGameGum.GueDeriving;

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
    }

    public static TextureRegion GetTextureRegion(ShopItems shopItem)
    {
        return _textureRegions[shopItem];
    }

    public static SpriteRuntime GetSpriteRuntime(ShopItems shopItem, PlayerStats pStats = null)
    {
        return new SpriteRuntime
        {
            Texture = _textureRegions[shopItem].Texture,
            SourceRectangle = _textureRegions[shopItem].SourceRectangle,
            WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            Width = _textureRegions[shopItem].SourceRectangle.Width,
            Height = _textureRegions[shopItem].SourceRectangle.Height,
            TextureAddress = Gum.Managers.TextureAddress.Custom,
            Color = PlayerStatsManager.GetPowerUpColor(ShopItems.HARPOON, pStats)
        };
    }
}