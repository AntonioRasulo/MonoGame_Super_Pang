using MonoGameLibrary;
using MonoGame_Super_Pang.Scenes;

namespace MonoGame_Super_Pang;

public class Game1 : Core
{
    public Game1() : base("Monogame Super Pang", 1280, 720, false)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Start the game with the title scene.
        ChangeScene(new TitleScene());

    }

    protected override void LoadContent()
    {

    }

}
