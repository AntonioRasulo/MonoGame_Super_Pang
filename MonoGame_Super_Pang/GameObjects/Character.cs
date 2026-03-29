using System;
using MonoGameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

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

    private readonly Vector2 SCALE = new Vector2(4.0f, 4.0f);

    private const int HARPOON_DELAY = 5;

    private const int MAX_HARPOON = 1;

    private float _immunityDuration = 3.0f; // seconds of immunity
    private float _immunityTimer = 0f;
    private float _blinkInterval = 0.1f;    // how fast it blinks
    private float _blinkTimer = 0f;
    private bool _isVisible = true;

    private Invicible _invinciblePowerUp;

    public bool IsImmune => _immunityTimer > 0f;

    private SoundEffect _hitSoundEffect;

    private int _lives = 3;

    public Character()
    {
        LoadContent();

        _harpoons = new List<Harpoon>();

        float windowWidth = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;
        float windowHeight = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;

        _characterPosition = new Vector2(
            windowWidth*0.5f, 
            windowHeight-_idleSprite.Height);

        previousKeyboardState = Keyboard.GetState();
    }

    private void LoadContent()
    {
        TextureAtlas characterAtlas = TextureAtlas.FromFile(Core.Content, "images/character_atlas.xml");
        TextureAtlas itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/items-atlas.xml");

        _idleSprite = characterAtlas.CreateSprite("characterStanding");
        _idleSprite.Scale = SCALE;

        _walkAnimation = characterAtlas.CreateAnimatedSprite("walk-animation");
        _walkAnimation.Scale = SCALE;

        _shootAnimation = characterAtlas.CreateAnimatedSprite("shooting-animation");
        _shootAnimation.Scale = SCALE;

        // Retrieve harpoons frames
        List<TextureRegion> harpoonFrames = new List<TextureRegion>();
        for (int harpoonIndex = 100; harpoonIndex <= 170; harpoonIndex++)
        {
            String harpoonImagePath = "images/items_" + harpoonIndex;
            Texture2D harpoon2DTexture = Core.Content.Load<Texture2D>(harpoonImagePath);
            TextureRegion harpoonRegion = new TextureRegion(harpoon2DTexture, 0, 0, harpoon2DTexture.Width, harpoon2DTexture.Height);
            harpoonFrames.Add(harpoonRegion);
        }

        _harpoonAnimation = new Animation(harpoonFrames, TimeSpan.FromMilliseconds(HARPOON_DELAY));

        _invinciblePowerUp = new Invicible(itemsAtlas.GetRegion("invincibilitySprite"));

        _hitSoundEffect = Core.Content.Load<SoundEffect>("audio/Boss hit 1");

    }

    public void Update(GameTime gameTime)
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        // Handle shooting (highest priority - interrupts other actions)
        if (currentKeyboardState.IsKeyDown(Keys.Space) &&
            previousKeyboardState.IsKeyUp(Keys.Space) &&
            currentState != CharacterState.Shooting &&
            _harpoons.Count < MAX_HARPOON)
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

        // Create a bounding rectangle for the screen.
        Rectangle screenBounds = new Rectangle(
            0,
            0,
            Core.GraphicsDevice.PresentationParameters.BackBufferWidth,
            Core.GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        // Getting the bounding rectangle for the character
        Rectangle characterBounds = getBounds();

        Vector2 newCharPosition = _characterPosition;

        // Use distance based checks to determine if the character is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // move it back inside.
        if (characterBounds.Left < screenBounds.Left)
        {
            newCharPosition.X = screenBounds.Left;
            _characterPosition = newCharPosition;
        }
        else if (characterBounds.Right > screenBounds.Right)
        {
            newCharPosition.X = screenBounds.Right - getWidth();
            _characterPosition = newCharPosition;
        }
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

    private void shootBullet()
    {
        AnimatedSprite newHarpoon = new AnimatedSprite(_harpoonAnimation);

        _harpoons.Add(new Harpoon(newHarpoon, _characterPosition.X + (_shootAnimation.Width*0.5f), 720));
    }

    public void activateImmunity(bool fromPowerUp = false)
    {
        _immunityTimer = _immunityDuration;
        if (fromPowerUp)
        {
            _invinciblePowerUp.isActive = true;
        }
        else
        {
            _lives--;
            Core.Audio.PlaySoundEffect(_hitSoundEffect);
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

    public bool isAlive()
    {
        return _lives > 0;
    }

    public void increaseLives()
    {
        _lives++;
    }

    public int getLives()
    {
        return _lives;
    }

}
