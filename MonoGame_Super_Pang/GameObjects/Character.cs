using System;
using MonoGameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Super_Pang.GameObjects;

// Enum to track character state
public enum CharacterState
{
    Idle,
    Walking,
    Shooting
}

public class Character
{
    private CharacterState currentState = CharacterState.Idle;
    private KeyboardState previousKeyboardState;

    // Tracks the position of the character.
    public Vector2 _characterPosition { get; set; }

    private Sprite _idleSprite;
    private AnimatedSprite _walkAnimation;
    private AnimatedSprite _shootAnimation;

    private float _speed = 5.0f;

    private const float SCALE = 4.0f;

    public Character(Sprite idleSprite, AnimatedSprite walkAnimation, AnimatedSprite shootAnimation)
    {
        _idleSprite = idleSprite;
        _walkAnimation = walkAnimation;
        _shootAnimation = shootAnimation;
    }

    public void Initialize(float windowWidth, float windowHeight)
    {
        _characterPosition = new Vector2(    // position
            (windowWidth)*0.5f, 
            windowHeight-_idleSprite.Height*4);
        previousKeyboardState = Keyboard.GetState();
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        // Handle shooting (highest priority - interrupts other actions)
        if (currentKeyboardState.IsKeyDown(Keys.Space) &&
            previousKeyboardState.IsKeyUp(Keys.Space) &&
            currentState != CharacterState.Shooting)
        {
            currentState = CharacterState.Shooting;
            _shootAnimation.Reset();
        }

        switch (currentState)
        {
            case CharacterState.Shooting:
                _shootAnimation.Update(gameTime);

                if (_shootAnimation.IsComplete)
                {

                    // After shooting, check if moving
                    if (currentKeyboardState.IsKeyDown(Keys.A) ||
                        currentKeyboardState.IsKeyDown(Keys.D) ||
                        currentKeyboardState.IsKeyDown(Keys.Left) ||
                        currentKeyboardState.IsKeyDown(Keys.Right))
                    {
                        currentState = CharacterState.Walking;
                    }
                    else
                    {
                        currentState = CharacterState.Idle;
                    }

                }

            break;
            case CharacterState.Idle:

                // In case there's an animation for Idle, call update for the nimation here

                if (currentKeyboardState.IsKeyDown(Keys.A) ||
                        currentKeyboardState.IsKeyDown(Keys.D) ||
                        currentKeyboardState.IsKeyDown(Keys.Left) ||
                        currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    currentState = CharacterState.Walking;
                }

            break;

            case CharacterState.Walking:

                _walkAnimation.Update(gameTime);

                Vector2 newPosition = _characterPosition;

                // Check if still moving
                if (currentKeyboardState.IsKeyDown(Keys.A) ||
                    currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    currentState = CharacterState.Walking;

                    newPosition.X -= _speed;
                    _characterPosition = newPosition;
                    _walkAnimation.Effects = SpriteEffects.None;
                    _idleSprite.Effects = SpriteEffects.None;

                }else if (  currentKeyboardState.IsKeyDown(Keys.D) ||
                            currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    currentState = CharacterState.Walking;

                    newPosition.X += _speed;
                    _characterPosition = newPosition;
                    _walkAnimation.Effects = SpriteEffects.FlipHorizontally;
                    _idleSprite.Effects = SpriteEffects.FlipHorizontally;

                }
                else
                {
                    currentState = CharacterState.Idle;
                }

            break;
        }
        previousKeyboardState = currentKeyboardState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Sprite currentAnimation = currentState switch
        {
            CharacterState.Shooting => _shootAnimation,
            CharacterState.Walking => _walkAnimation,
            CharacterState.Idle => _idleSprite,
            _ => null
        };

        if (currentAnimation != null)
        {
            currentAnimation.Scale = new Vector2(SCALE, SCALE);;
            currentAnimation.Draw(spriteBatch, _characterPosition);
        }

    }

    public Rectangle getBounds()
    {
        // Creating a bounding rectangle for the character
        Rectangle characterBounds = new Rectangle(
            (int)(_characterPosition.X),
            (int)(_characterPosition.Y + (_idleSprite.Height * 0.5f)),
            (int)(_idleSprite.Width),
            (int)(_idleSprite.Height * 0.5f *SCALE)
        );

        return characterBounds;
    }

    public float getWidth()
    {
        return _idleSprite.Width;
    }

    public float getScaledWidth()
    {
        return _idleSprite.Width * SCALE;
    }

    public float getHeight()
    {
        return _idleSprite.Height;
    }

}
