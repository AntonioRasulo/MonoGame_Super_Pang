using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;

namespace MonoGame_Super_Pang.GameObjects;

public enum BatState
{
    Idle,
    Hurt,
    Fall,
    Land,
    Death,
    Frozen
}

abstract public class Bat : Enemy
{
    protected AnimatedSprite _idleAnimation;
    protected AnimatedSprite _hurtAnimation;
    protected AnimatedSprite _fallAnimation;
    protected AnimatedSprite _landAnimation;
    protected Sprite _deathSprite;

    protected BatState _state;
    protected BatState _stateBeforeFrozen;

    private const float FALL_SPEED = 5.0f;

    private const int DEATH_TIME = 5;

    private float _deathTimer = 0f;

    public Bat(Vector2 position): base(position)
    {
        LoadContent();
        _idleAnimation.Scale = new Vector2(SCALE, SCALE);
        _idleAnimation.CenterOrigin();

        _fallAnimation.Scale = new Vector2(SCALE, SCALE);
        _fallAnimation.CenterOrigin();

        _landAnimation.Scale = new Vector2(SCALE, SCALE);
        _landAnimation.CenterOrigin();

        _deathSprite.Scale = new Vector2(SCALE, SCALE);
        _deathSprite.CenterOrigin();

        _state = BatState.Idle;
    }

    public override void Update(GameTime gameTime)
    {
        switch (_state)
        {
            case BatState.Idle:
                _idleAnimation.Update(gameTime);
                UpdateMovement(gameTime);
                CheckFreeze();
            break;
            case BatState.Hurt:
                if(_hurtAnimation != null)
                {
                    _hurtAnimation.Update(gameTime);
                    ToggleVisibility(gameTime);
                    if (_hurtAnimation.IsComplete)
                    {
                        _state = BatState.Idle;
                        _hurtAnimation.Reset();
                        _blinkTimer = 0;
                        _isVisible = true;
                    }
                    CheckFreeze();
                }
            break;
            case BatState.Frozen:
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
            if(_hurtAnimation != null)
            {
                currentSprite = _hurtAnimation;        
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
            break;
            case BatState.Frozen:
                {
                    if(_stateBeforeFrozen == BatState.Idle)
                    {
                        currentSprite = _idleAnimation;
                    }
                    else if(_stateBeforeFrozen == BatState.Hurt && _hurtAnimation != null)
                    {
                        currentSprite = _hurtAnimation;
                    }
                    else
                    {
                        return new Rectangle(0, 0, 0, 0);
                    }
                }
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
            if(!_isVisible)
                return;
            _hurtAnimation?.Draw(Core.SpriteBatch, _position);
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
            case BatState.Frozen:
                if(!_isVisible)
                    return;
                if (_stateBeforeFrozen == BatState.Hurt)
                    _hurtAnimation?.Draw(Core.SpriteBatch, _position);
                else
                    _idleAnimation.Draw(Core.SpriteBatch, _position);
                break;
        }
    }

    public override int TakeHit()
    {
        int score = base.TakeHit();

        if(_lives == 0)
        {
            _state = BatState.Fall;
        }
        else
        {
            if(_state == BatState.Idle)
            {
                _state = BatState.Hurt;
            }
        }
        _toggleVisibility = true;
        _blinkDuration = BLINK_DURATION;
        return score;
    }

    protected override void CheckFreeze()
    {
        if(FreezeHandler.freezeTimer > 0)
        {
            _stateBeforeFrozen = _state;
            _state = BatState.Frozen;
            _toggleVisibility = false;
        }
    }

}
