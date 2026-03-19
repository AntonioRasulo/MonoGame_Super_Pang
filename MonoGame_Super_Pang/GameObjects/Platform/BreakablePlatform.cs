
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Super_Pang.GameObjects;

public enum PlatformState
{
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Delete
}

public class BreakablePlatform : Platform
{
    private List<Sprite> _sprites;
    private PlatformState _platformState;

    private SoundEffect _breakPlatformEffect;

    public BreakablePlatform(List<Sprite> sprites, Vector2 position, PlatformType platformType, PlatformState platformState, SoundEffect breakPlatformEffect) : base(position, platformType)
    {
        _sprites = sprites;
        foreach(Sprite sprite in _sprites)
        {
            sprite.Scale = new Vector2(SCALE, SCALE);
        }
        _platformState = platformState;

        _breakable = true;

        _breakPlatformEffect = breakPlatformEffect;
    }

    public override void Draw()
    {
        switch (_platformState)
        {
            case PlatformState.Stage1:
            _sprites[0].Draw(Core.SpriteBatch, _position);
            break;
            case PlatformState.Stage2:
            _sprites[1].Draw(Core.SpriteBatch, _position);
            break;
            case PlatformState.Stage3:
            _sprites[2].Draw(Core.SpriteBatch, _position);
            break;
            case PlatformState.Stage4:
            _sprites[3].Draw(Core.SpriteBatch, _position);
            break;
            case PlatformState.Stage5:
            _sprites[4].Draw(Core.SpriteBatch, _position);
            break;
            case PlatformState.Delete:
            default:
            break;
        }
    }

    public override Rectangle getBounds()
    {
        Rectangle platformBounds;
        switch (_platformState)
        {
            case PlatformState.Stage1:
                platformBounds = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_sprites[0].Width,
                (int)_sprites[0].Height
                );
                break;
            case PlatformState.Stage2:
            platformBounds = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_sprites[1].Width,
                (int)_sprites[1].Height
                );
                break;
            case PlatformState.Stage3:
            platformBounds = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_sprites[2].Width,
                (int)_sprites[2].Height
                );
                break;
            case PlatformState.Stage4:
            platformBounds = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_sprites[3].Width,
                (int)_sprites[3].Height
                );
                break;
            case PlatformState.Stage5:
            platformBounds = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_sprites[4].Width,
                (int)_sprites[4].Height
                );
                break;
            case PlatformState.Delete:
            default:
                platformBounds = new Rectangle(0,0,0,0);
                break;
        }

        return platformBounds;
    }

    public void hitPlatform()
    {
        Core.Audio.PlaySoundEffect(_breakPlatformEffect);
        switch (_platformState)
        {
            case PlatformState.Stage1:
                _platformState = PlatformState.Stage2;
            break;
            case PlatformState.Stage2:
                _platformState = PlatformState.Stage3;
            break;
            case PlatformState.Stage3:
                _platformState = PlatformState.Stage4;
            break;
            case PlatformState.Stage4:
                _platformState = PlatformState.Stage5;
            break;
            case PlatformState.Stage5:
                _platformState = PlatformState.Delete;
            break;
            case PlatformState.Delete:
            default:
            break;
        }
    }

    public PlatformState getState()
    {
        return _platformState;
    }

}