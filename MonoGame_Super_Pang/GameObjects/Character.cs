using System;
using MonoGameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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

    private Animation _harpoonAnimation;

    private List<Harpoon> _harpoons;

    private float _speed = 5.0f;

    private const float SCALE = 4.0f;

    private float _immunityDuration = 3.0f; // seconds of immunity
    private float _immunityTimer = 0f;
    private float _blinkInterval = 0.1f;    // how fast it blinks
    private float _blinkTimer = 0f;
    private bool _isVisible = true;

   private Invicible _invinciblePowerUp;

    public bool IsImmune => _immunityTimer > 0f;
    public Character(Sprite idleSprite, AnimatedSprite walkAnimation, AnimatedSprite shootAnimation, Animation harpoonAnimation, TextureRegion invincibleRegion)
    {
        _idleSprite = idleSprite;
        _walkAnimation = walkAnimation;
        _shootAnimation = shootAnimation;
        _harpoonAnimation = harpoonAnimation;
        _harpoons = new List<Harpoon>();
        _invinciblePowerUp = new Invicible(invincibleRegion);
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

        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_immunityTimer > 0f)
        {
            _immunityTimer -= delta;

            _blinkTimer += delta;
            if (_blinkTimer >= _blinkInterval)
            {
                _blinkTimer = 0f;
                _isVisible = !_isVisible; // toggle visibility
            }

            if (_immunityTimer <= 0f)
            {
                _immunityTimer = 0f;
                _isVisible = true; // ensure visible when immunity ends
                _invinciblePowerUp.isActive = false;
            }
        }

        switch (currentState)
        {
            case CharacterState.Shooting:
                _shootAnimation.Update(gameTime);

                if (_shootAnimation.IsComplete)
                {

                    shootBullet();

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

        foreach(Harpoon bullet in _harpoons)
        {
            bullet.Update(gameTime);
        }

        _harpoons.RemoveAll(bullet => bullet.IsAnimationComplete);

    }

    public void Draw(SpriteBatch spriteBatch)
    {

        foreach(Harpoon bullet in _harpoons)
        {
            bullet.Draw();
        }

        if (!_isVisible && !_invinciblePowerUp.isActive) return;

        if(_invinciblePowerUp.isActive && _isVisible)
        {
            Vector2 invinciblePowerUpPos = new Vector2(_characterPosition.X, _characterPosition.Y - 20.0f);
            _invinciblePowerUp.Draw(Core.SpriteBatch, invinciblePowerUpPos);
        }

        Sprite currentAnimation = currentState switch
        {
            CharacterState.Shooting => _shootAnimation,
            CharacterState.Walking => _walkAnimation,
            CharacterState.Idle => _idleSprite,
            _ => null
        };

        if (currentAnimation != null)
        {
            currentAnimation.Scale = new Vector2(SCALE, SCALE);
            currentAnimation.Draw(spriteBatch, _characterPosition);
        }

    }

    public Rectangle getBounds()
    {
        // Creating a bounding rectangle for the character
        Rectangle characterBounds = new Rectangle(
            (int)_characterPosition.X,
            (int)_characterPosition.Y,
            (int)_idleSprite.Width,
            (int)_idleSprite.Height
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

    private void shootBullet()
    {
        AnimatedSprite newHarpoon = new AnimatedSprite(_harpoonAnimation);

        _harpoons.Add(new Harpoon(newHarpoon, _characterPosition.X + (_shootAnimation.Width*0.5f), 720));
    }

    public void TakeHit()
    {
        activateImmunity();
        _blinkTimer = 0f;
        _isVisible = true;
    }

    public void activateImmunity(bool fromPowerUp = false)
    {
        _immunityTimer = _immunityDuration;
        if (fromPowerUp)
        {
            _invinciblePowerUp.isActive = true;
        }
        _blinkTimer = 0f;
        _isVisible = true;
    }

    public void removeHarpoons(List<Harpoon> toRemoveHarpoons)
    {
        _harpoons.RemoveAll(harpoon => toRemoveHarpoons.Contains(harpoon));
    }

    public List<Harpoon> getHarpoons()
    {
        return _harpoons;
    }

}
