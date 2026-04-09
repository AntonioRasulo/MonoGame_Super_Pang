using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Super_Pang.GameObjects;

class Invicible : Sprite
{
    private const float SCALE = 4.0f;

    public bool isActive {get; set;}

    public Invicible(TextureRegion region) : base(region)
    {
        Scale = new Vector2(SCALE, SCALE);
        CenterOrigin();
    }

}