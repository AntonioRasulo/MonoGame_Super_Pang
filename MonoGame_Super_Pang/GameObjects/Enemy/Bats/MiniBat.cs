using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using System;

namespace MonoGame_Super_Pang.GameObjects;

public class MiniBat : Bat
{
    private const int NUM_LIVES = 1;
    private const int ENEMY_SCORE = 3;

    private const float MOVEMENT_SPEED = 5.0f;

    public MiniBat(Vector2 position):
    base(position)
    {
        _lives = NUM_LIVES;
        _score = ENEMY_SCORE;

        RandomizeVelocity();
    }

    /// <summary>
    /// Randomizes the velocity of the bat.
    /// </summary>
    public void RandomizeVelocity()
    {
        // Generate a random angle
        float angle = (float)(Random.Shared.NextDouble() * MathHelper.TwoPi);

        // Convert the angle to a direction vector
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed to get the
        // final velocity
        _velocity = direction * MOVEMENT_SPEED;
    }

    protected override void UpdateMovement(GameTime gameTime)
    {
        // Update the position of the bat based on the velocity.
        _position += _velocity;

        Rectangle batBounds = GetBounds();
        Rectangle _roomBounds = Core.GraphicsDevice.PresentationParameters.Bounds;
        if (batBounds.Top < _roomBounds.Top)
        {
            Bounce(Vector2.UnitY);
        }
        else if (batBounds.Bottom > _roomBounds.Bottom)
        {
            Bounce(-Vector2.UnitY);
        }

        if (batBounds.Left < _roomBounds.Left)
        {
            Bounce(Vector2.UnitX);
        }
        else if (batBounds.Right > _roomBounds.Right)
        {
            Bounce(-Vector2.UnitX);
        }
    }

    /// <summary>
    /// Handles a bounce event when the bat collides with a wall or boundary.
    /// </summary>
    /// <param name="normal">The normal vector of the surface the bat is bouncing against.</param>
    private void Bounce(Vector2 normal)
    {
        Vector2 newPosition = _position;

        // Adjust the position based on the normal to prevent sticking to walls.
        if (normal.X != 0)
        {
            // We are bouncing off a vertical wall (left/right).
            // Move slightly away from the wall in the direction of the normal.
            newPosition.X += normal.X * (_idleAnimation.Width * 0.1f);
        }

        if (normal.Y != 0)
        {
            // We are bouncing off a horizontal wall (top/bottom).
            // Move slightly way from the wall in the direction of the normal.
            newPosition.Y += normal.Y * (_idleAnimation.Height * 0.1f);
        }

        // Apply the new position
        _position = newPosition;

        // Normalize before reflecting
        normal.Normalize();

        // Apply reflection based on the normal.
        _velocity = Vector2.Reflect(_velocity, normal);

    }

    protected override void LoadContent()
    {
        TextureAtlas miniBatAtlas = TextureAtlas.FromFile(Core.Content, "images/enemies/mini_bats/mini_bat.xml");

        _idleAnimation = miniBatAtlas.CreateAnimatedSprite("idle-animation");
        _fallAnimation = miniBatAtlas.CreateAnimatedSprite("fall-animation");
        _landAnimation = miniBatAtlas.CreateAnimatedSprite("land-animation");
        _deathSprite = miniBatAtlas.CreateSprite("land7");
    }

}