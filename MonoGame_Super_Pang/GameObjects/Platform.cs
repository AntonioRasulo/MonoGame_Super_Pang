using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.GameObjects;

public enum PlatformType
{
    HORIZONTAL_GRAY,
    BREAKABLE_LARGE_HORIZONTAL_BLUE
};

abstract public class Platform
{
    protected Vector2 _position;

    protected PlatformType _platformType;

    protected const float SCALE = 4f;

    protected bool _breakable;

    protected static TextureRegion _grayHorizontalPlatform;

    protected static List<TextureRegion> _horizontalBreakableBlueSprites;

    public Platform(Vector2 position, PlatformType platformType)
    {
        LoadSprite();
        _position = position;
        _platformType = platformType;
    }

    public abstract void Draw();

    public abstract Rectangle getBounds();

    public bool isBreakable()
    {
        return _breakable;
    }

    public static void LoadContent()
    {
        TextureAtlas platformAtlas = TextureAtlas.FromFile(Core.Content, "images/terrain_atlas.xml");
        TextureAtlas itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/items-atlas.xml");

        _grayHorizontalPlatform = platformAtlas.GetRegion("horizontalGrayPlatform");

        _horizontalBreakableBlueSprites = new List<TextureRegion>();
        for(int indexPlatform = 1; indexPlatform<=5; indexPlatform++)
        {
            String spriteName = "largeBreakableBluePlatform"+indexPlatform;
            _horizontalBreakableBlueSprites.Add(itemsAtlas.GetRegion(spriteName));
        }

    }

    protected abstract void LoadSprite();

}