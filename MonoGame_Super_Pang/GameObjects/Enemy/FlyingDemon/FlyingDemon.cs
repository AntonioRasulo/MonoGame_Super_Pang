using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.GameObjects;

public enum FlyingDemonState
{
    Idle,
    Hurt,
    Attack,
    Death,
    Frozen
}
public class FlyingDemon : Enemy
{
    private AnimatedSprite _idleAnimation;
    private AnimatedSprite _hurtAnimation;
    private AnimatedSprite _attackAnimation;
    private AnimatedSprite _deathAnimation;
    private Animation _fireBallAnimation;

    private FlyingDemonState _state;
    private FlyingDemonState _stateBeforeFrozen;

    private Vector2 _target;

    private readonly int _minPosX;
    private readonly int _maxPosX;
    private readonly int _minPosY;
    private readonly int _maxPosY;

    private float _fireballTimer = 0f;

    private float FIREBALL_TIME = 3.0f;
    private const float FIREBALL_DELAY = 5.0f;
    private const float MOVEMENT_SPEED = 100f;
    private const int NUM_LIVES = 20;
    private const int SCORE = 100;

    public FlyingDemon(Vector2 position): base(position)
    {
        LoadContent();
        _idleAnimation.Scale = new Vector2(SCALE, SCALE);
        _idleAnimation.CenterOrigin();

        _hurtAnimation.Scale = new Vector2(SCALE, SCALE);
        _hurtAnimation.CenterOrigin();

        _attackAnimation.Scale = new Vector2(SCALE, SCALE);
        _attackAnimation.CenterOrigin();

        _deathAnimation.Scale = new Vector2(SCALE, SCALE);
        _deathAnimation.CenterOrigin();

        _state = FlyingDemonState.Idle;

        _minPosX = (int)(_idleAnimation.Width * 0.5f);
        _minPosY = (int)(_idleAnimation.Height * 0.5f);
        _maxPosX = Core.GraphicsDevice.PresentationParameters.BackBufferWidth - (int)(_idleAnimation.Width * 0.5f);
        _maxPosY = (int)(Core.GraphicsDevice.PresentationParameters.BackBufferHeight * 0.5f - (_idleAnimation.Height * 0.5f));

        UpdateTargetPosition();

        _movementSpeed = MOVEMENT_SPEED;

        _lives = NUM_LIVES;

        _score = SCORE;

        _fireballTimer = FIREBALL_TIME;

    }

    public override void Update(GameTime gameTime)
    {
        switch (_state)
        {
            case FlyingDemonState.Idle:
                _idleAnimation.Update(gameTime);
                UpdateMovement(gameTime);
                UpdateFireBallTimer(gameTime);
                if(_fireballTimer <= 0)
                {
                    _state = FlyingDemonState.Attack;
                    _idleAnimation.Reset();
                }
                CheckFreeze();
            break;
            case FlyingDemonState.Hurt:
                _hurtAnimation.Update(gameTime);
                UpdateFireBallTimer(gameTime);
                UpdateFireballTimeVariable();
                ToggleVisibility(gameTime);
                if (_hurtAnimation.IsComplete)
                {
                    _state = FlyingDemonState.Idle;
                    _hurtAnimation.Reset();
                    _blinkTimer = 0;
                    _isVisible = true;
                }
                if(_fireballTimer <= 0)
                {
                    _state = FlyingDemonState.Attack;
                    _hurtAnimation.Reset();
                }
                CheckFreeze();
            break;
            case FlyingDemonState.Attack:
                _attackAnimation.Update(gameTime);
                if (_attackAnimation.IsComplete)
                {
                    GenerateFireball();
                    _state = FlyingDemonState.Idle;
                    _fireballTimer = FIREBALL_TIME;
                    _attackAnimation.Reset();
                }
            break;
            case FlyingDemonState.Death:
                _deathAnimation.Update(gameTime);
                if (_deathAnimation.IsComplete)
                {
                    _toRemove = true;
                    _deathAnimation.Reset();
                }
            break;
            case FlyingDemonState.Frozen:
                if (_toggleVisibility)
                {
                    ToggleVisibility(gameTime);
                }
                if (FreezeHandler.freezeTimer <= 0)
                {
                    _state = _stateBeforeFrozen;
                    _isVisible = true;
                }
                break;
        }

        foreach(Bullet fireball in _bullets)
        {
            fireball.Update(gameTime);
        }

        _bullets.RemoveAll(fireball => fireball._isToRemove);

    }

    public override void Draw()
    {
        AnimatedSprite spriteToDraw = null;
        switch (_state)
        {
            case FlyingDemonState.Idle:
                spriteToDraw = _idleAnimation;
                break;
            case FlyingDemonState.Hurt:
                if(!_isVisible)
                    return;
                spriteToDraw = _hurtAnimation;
                break;
            case FlyingDemonState.Attack:
                spriteToDraw = _attackAnimation;
                break;
            case FlyingDemonState.Death:
                spriteToDraw = _deathAnimation;
                break;
            case FlyingDemonState.Frozen:
                if(!_isVisible)
                    return;
                if (_stateBeforeFrozen == FlyingDemonState.Hurt)
                    goto case FlyingDemonState.Hurt;
                else if(_stateBeforeFrozen == FlyingDemonState.Idle)
                    goto case FlyingDemonState.Idle;
                else if(_stateBeforeFrozen == FlyingDemonState.Attack)
                    goto case FlyingDemonState.Attack;
                break;
        }
        FlipSprite(ref spriteToDraw);

        spriteToDraw.Draw(Core.SpriteBatch, _position);
        foreach(Bullet fireball in _bullets)
        {
            fireball.Draw();
        }
    }

    public override Rectangle GetBounds()
    {
        float width = 0;
        float height = 0;
        float positionX = 0;
        float positionY = 0;

        switch (_state)
        {
            case FlyingDemonState.Idle:
                width = _idleAnimation.Width;
                height = _idleAnimation.Height;
                positionX = _position.X - width * 0.5f;
                positionY = _position.Y - height * 0.5f;
                break;
            case FlyingDemonState.Attack:
                width = _attackAnimation.Width;
                height = _attackAnimation.Height;
                positionX = _position.X - width * 0.5f;
                positionY = _position.Y - height * 0.5f;
            break;
            case FlyingDemonState.Frozen:
                {
                    if (_stateBeforeFrozen == FlyingDemonState.Idle)
                    {
                        goto case FlyingDemonState.Idle;
                    }
                    else if (_stateBeforeFrozen == FlyingDemonState.Attack)
                    {
                        goto case FlyingDemonState.Attack;
                    }
                }
                break;
        }

        // Creating a bounding rectangle for the enemy
        return new Rectangle(
            (int)positionX,
            (int)positionY,
            (int)width,
            (int)height
        );   
    }

    protected override void LoadContent()
    {
        TextureAtlas attackAtlas = TextureAtlas.FromFile(Core.Content, "images/enemies/flying_demon/attack_atlas.xml");
        TextureAtlas deathAtlas = TextureAtlas.FromFile(Core.Content, "images/enemies/flying_demon/death_atlas.xml");
        TextureAtlas flyingAtlas = TextureAtlas.FromFile(Core.Content, "images/enemies/flying_demon/flying_atlas.xml");
        TextureAtlas hurtAtlas = TextureAtlas.FromFile(Core.Content, "images/enemies/flying_demon/hurt_atlas.xml");

        _idleAnimation = flyingAtlas.CreateAnimatedSprite("flying-animation");
        _hurtAnimation = hurtAtlas.CreateAnimatedSprite("hurt-animation");
        _attackAnimation = attackAtlas.CreateAnimatedSprite("attack-animation");
        _deathAnimation = deathAtlas.CreateAnimatedSprite("death-animation");

        // Retrieve fireball frames
        List<TextureRegion> fireballFrames = new List<TextureRegion>();
        for (int fireballIndex = 1; fireballIndex <= 5; fireballIndex++)
        {
            String fireballImagePath = "images/enemies/flying_demon/fireball/FB00" + fireballIndex;
            Texture2D fireball2DTexture = Core.Content.Load<Texture2D>(fireballImagePath);
            TextureRegion harpoonRegion = new TextureRegion(fireball2DTexture, 0, 0, fireball2DTexture.Width, fireball2DTexture.Height);
            fireballFrames.Add(harpoonRegion);
        }

        _fireBallAnimation = new Animation(fireballFrames, TimeSpan.FromMilliseconds(FIREBALL_DELAY));

    }

    protected override void UpdateMovement(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 direction = _target - _position;
        float distance = direction.Length();

        if(distance <= _movementSpeed * delta)
        {
            _position = _target;
            UpdateTargetPosition();
        }
        else
        {
            direction.Normalize();
            _position += direction * _movementSpeed * delta;
        }

    }

    private void UpdateFireBallTimer(GameTime gameTime)
    {
        if (_state == FlyingDemonState.Frozen)
            return;

        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_fireballTimer > 0f)
        {
            _fireballTimer -= delta;
        }

    }

    private void UpdateFireballTimeVariable()
    {
        if(_lives < 5)
        {
            FIREBALL_TIME = 1f;
        }
        else if(_lives < 10)
        {
            FIREBALL_TIME = 1.5f;
        }
        else if (_lives < 15)
        {
            FIREBALL_TIME = 2.5f;
        }
    }

    private void UpdateTargetPosition()
    {
        int targetY = Random.Shared.Next(_minPosY, _maxPosY);

        int targetX = Random.Shared.Next(_minPosX, _maxPosX);
        
        _target = new Vector2(targetX, targetY);
    }

    private void GenerateFireball()
    {
        Rectangle enemyBound = GetBounds();
        Vector2 bulletPosition = new Vector2(_position.X, enemyBound.Bottom);
        _bullets.Add(new Fireball(_fireBallAnimation, bulletPosition));
    }

    public override int TakeHit()
    {
        int score = base.TakeHit();

        if(_lives == 0)
        {
            _state = FlyingDemonState.Death;
            Core.Input.GamePads[(int)PlayerIndex.One].SetVibration(1f, TimeSpan.FromMilliseconds(600));
        }
        else
        {
            if(_state == FlyingDemonState.Idle)
            {
                _state = FlyingDemonState.Hurt;
            }
        }

        HandleHitVibration();

        _toggleVisibility = true;
        _blinkDuration = BLINK_DURATION;
        return score;
    }

    protected override void CheckFreeze()
    {
        if(FreezeHandler.freezeTimer > 0)
        {
            _stateBeforeFrozen = _state;
            _state = FlyingDemonState.Frozen;
            _toggleVisibility = false;
        }
    }

    private void FlipSprite(ref AnimatedSprite sprite)
    {
        if(_state == FlyingDemonState.Frozen)
            return;

        if(Character._characterPosition.X > _position.X)
        {
            sprite.Effects = SpriteEffects.FlipHorizontally;
        }
        else
        {
            sprite.Effects = SpriteEffects.None;
        }
    }

    private void HandleHitVibration()
    {
        if(_lives == 15)
        {
            Core.Input.GamePads[(int)PlayerIndex.One].SetVibration(0.4f, TimeSpan.FromMilliseconds(300));
        }
        else if(_lives == 10)
        {
            Core.Input.GamePads[(int)PlayerIndex.One].SetVibration(0.6f, TimeSpan.FromMilliseconds(300));
        }
        else if(_lives == 5)
        {
            Core.Input.GamePads[(int)PlayerIndex.One].SetVibration(0.8f, TimeSpan.FromMilliseconds(300));
        }
    }

}