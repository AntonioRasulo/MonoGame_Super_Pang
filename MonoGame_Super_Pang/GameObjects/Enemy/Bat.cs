using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using System;

namespace MonoGame_Super_Pang.GameObjects;

public enum BatState
{
    Idle,
    Hurt,
    Fall,
    Land,
    Death
}

public class Bat : Enemy
{
    private AnimatedSprite _idleAnimation;
    private AnimatedSprite _hurtAnimation;
    private AnimatedSprite _fallAnimation;
    private AnimatedSprite _landAnimation;
    private Sprite _deathSprite;

    private BatState _state;

    private const float FALL_SPEED = 5.0f;
    private const int NUM_LIVES = 3;

    private const int ENEMY_SCORE = 7;

    private const int DEATH_TIME = 5;

    private Vector2 _target;

    private float _deathTimer = 0f;

    private Random _positionRand;

    private readonly int minPosX;
    private readonly int maxPosX;
    private readonly int minPosY;
    private readonly int maxPosY;

    public Bat(AnimatedSprite idleAnimation, AnimatedSprite hurtAnimation, AnimatedSprite fallAnimation, AnimatedSprite landAnimation, Sprite deathSprite, Vector2 position):
    base(position)
    {
        _idleAnimation = idleAnimation;
        _idleAnimation.Scale = new Vector2(SCALE, SCALE);
        _idleAnimation.CenterOrigin();

        _hurtAnimation = hurtAnimation;
        _hurtAnimation.Scale = new Vector2(SCALE, SCALE);
        _hurtAnimation.CenterOrigin();

        _fallAnimation = fallAnimation;
        _fallAnimation.Scale = new Vector2(SCALE, SCALE);
        _fallAnimation.CenterOrigin();

        _landAnimation = landAnimation;
        _landAnimation.Scale = new Vector2(SCALE, SCALE);
        _landAnimation.CenterOrigin();

        _deathSprite = deathSprite;
        _deathSprite.Scale = new Vector2(SCALE, SCALE);
        _deathSprite.CenterOrigin();

        _state = BatState.Idle;
        _positionRand = new Random();
        _lives = NUM_LIVES;
        _movementSpeed = 200f;
        _score = ENEMY_SCORE;

        minPosX = (int)(_idleAnimation.Width * 0.5f);
        minPosY = (int)(_idleAnimation.Height * 0.5f);
        maxPosX = Core.GraphicsDevice.PresentationParameters.BackBufferWidth - (int)(_idleAnimation.Width * 0.5f);
        maxPosY = Core.GraphicsDevice.PresentationParameters.BackBufferHeight - (int)(_idleAnimation.Height * 0.5f);

        UpdateTargetPosition();
    }

    private void UpdateTargetPosition()
    {
        int targetY = (int)_position.Y;
        while(!(targetY > _position.Y + _landAnimation.Height*0.5f || targetY < _position.Y - _landAnimation.Height*0.5f))
        {
            targetY = _positionRand.Next(minPosY, maxPosY);
        }

        int targetX = _positionRand.Next(minPosX, maxPosX);
        
        _target = new Vector2(targetX, targetY);
    }

    public override void Update(GameTime gameTime)
    {
        switch (_state)
        {
            case BatState.Idle:
                _idleAnimation.Update(gameTime);
                UpdateMovement(gameTime);
            break;
            case BatState.Hurt:
                _hurtAnimation.Update(gameTime);
                if (_hurtAnimation.IsComplete)
                {
                    _state = BatState.Idle;
                    _hurtAnimation.Reset();
                }
            break;
            case BatState.Fall:
                _fallAnimation.Update(gameTime);
                _position.Y += FALL_SPEED;
                float screenHeight = (float)Core.GraphicsDevice.PresentationParameters.BackBufferHeight;
                float lowerBoundLimit = screenHeight - _fallAnimation.Height * 0.5f;
                if(_position.Y >= lowerBoundLimit)
                {
                    float positionY = screenHeight - _deathSprite.Height * 0.5f;
                    _state = BatState.Land;
                    _position.Y = positionY;
                }
            break;
            case BatState.Land:
                _landAnimation.Update(gameTime);
                if (_landAnimation.IsComplete)
                {
                    _state = BatState.Death;
                    _deathTimer = DEATH_TIME;
                }
            break;
            case BatState.Death:
                UpdateDisappear(gameTime);
            break;
            default:
            break;
        }

    }

    private void UpdateMovement(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 direction = _target - _position;
        float distance = direction.Length();

        if(distance <= _movementSpeed * delta)
        {
            _position = _target;
            UpdateTargetPosition();
            // UpdatePositionIndex();
            // _target = positions[_positionIndex];
        }
        else
        {
            direction.Normalize();
            _position += direction * _movementSpeed * delta;
        }
    }

    private void UpdateDisappear(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_deathTimer > 0f)
        {
            _deathTimer -= delta;

            if (_deathTimer <= 0f)
            {
                _deathTimer = 0f;
                _toRemove = true;
            }
        }
    }

    /// <summary>
    /// Returns a Rectangle value that represents collision bounds of the bat.
    /// </summary>
    /// <returns>A Rectangle value.</returns>
    public override Rectangle GetBounds()
    {

        float width;
        float height;
        float positionX;
        float positionY;
        AnimatedSprite currentSprite = new AnimatedSprite();

        switch (_state)
        {
            case BatState.Idle:
            currentSprite = _idleAnimation;
            break;
            case BatState.Hurt:
            currentSprite = _hurtAnimation;
            break;
            case BatState.Fall:
            case BatState.Land:
            case BatState.Death:
            return new Rectangle(0, 0, 0, 0);
        }

        width = currentSprite.Width;
        height = currentSprite.Height;
        positionX = _position.X - width * 0.5f;
        positionY = _position.Y - height * 0.5f;

        // Creating a bounding rectangle for the character
        Rectangle batBounds = new Rectangle(
            (int)positionX,
            (int)positionY,
            (int)width,
            (int)height
        );

        return batBounds;
    }

    public override void Draw()
    {
        switch (_state)
        {
            case BatState.Idle:
            _idleAnimation.Draw(Core.SpriteBatch, _position);
            break;
            case BatState.Hurt:
            _hurtAnimation.Draw(Core.SpriteBatch, _position);
            break;
            case BatState.Fall:
            _fallAnimation.Draw(Core.SpriteBatch, _position);
            break;
            case BatState.Land:
            _landAnimation.Draw(Core.SpriteBatch, _position);
            break;
            case BatState.Death:
            _deathSprite.Draw(Core.SpriteBatch, _position);
            break;
        }
    }

    public override int TakeHit()
    {
        int score = 0;
        if(_state == BatState.Idle)
        {
            score = base.TakeHit();
            _state = _lives == 0 ? BatState.Fall : BatState.Hurt;
        }
        return score;
    }

}