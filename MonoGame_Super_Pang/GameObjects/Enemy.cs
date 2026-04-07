using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.GameObjects;

public enum EnemyType
{
    BIG_BAT,
    MINI_BAT,
    LAST_ENEMY
}

abstract public class Enemy
{
    // Tracks the position of the character.
    protected Vector2 _position;

    protected const float SCALE = 4.0f;

    protected float _movementSpeed;

    // The velocity of the bat that defines the direction and how much in that
    // direction to update the bats position each update cycle.
    protected Vector2 _velocity;

    protected int _lives;

    protected int _score;

    protected bool _toRemove;

    /// <summary>
    /// Creates a new Bat using the specified animated sprite and sound effect.
    /// </summary>
    /// <param name="position">The initial position of the enemy.</param>
    public Enemy(Vector2 position)
    {
        _position = position;
    }

    /// <summary>
    /// Returns a Rectangle value that represents collision bounds of the bat.
    /// </summary>
    /// <returns>A Rectangle value.</returns>
    public abstract Rectangle GetBounds();

    /// <summary>
    /// Updates the bat.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
    public abstract void Update(GameTime gameTime);

    /// <summary>
    /// Draws the enemy.
    /// </summary>
    public abstract void Draw();

    public virtual int TakeHit()
    {
        _lives--;
        if(_lives == 0)
        {
            return _score;
        }
        return 0;
    }

    public bool isToRemove()
    {
        return _toRemove;
    }

}