using System;
using MonoGameLibrary;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame_Super_Pang.GameObjects;

public class Harpoon
{

    private AnimatedSprite _harpoonAnimation;

    private const float SCALE = 4.0f;

    private Vector2 _position;

    public Harpoon(AnimatedSprite harpoonAnimation, float positionX, float positionY)
    {
        _harpoonAnimation = harpoonAnimation;

        _harpoonAnimation.Origin = new Vector2(_harpoonAnimation.Width *0.5f,_harpoonAnimation.Height);

        _position = new Vector2(positionX, positionY);

    }

    public void Initialize(float positionX)
    {

    }

    public void Update(GameTime gameTime)
    {
        _harpoonAnimation.Update(gameTime);
        _harpoonAnimation.Origin = new Vector2(_harpoonAnimation.Region.Width * 0.5f, _harpoonAnimation.Region.Height);
    }

    public void Draw()
    {
        _harpoonAnimation.Scale = new Vector2(SCALE, SCALE);
        _harpoonAnimation.Draw(Core.SpriteBatch, _position);
    }

    public Rectangle getBounds()
    {
        int scaledWidth  = (int)(_harpoonAnimation.Region.Width  * SCALE);
        int scaledHeight = (int)(_harpoonAnimation.Region.Height * SCALE);

        return new Rectangle(
            (int)(_position.X - scaledWidth * 0.5f),  // centered horizontally
            (int)(_position.Y - scaledHeight),         // extends upward
            scaledWidth,
            scaledHeight
        );
    }

    public bool IsAnimationComplete => _harpoonAnimation.IsComplete;

}
