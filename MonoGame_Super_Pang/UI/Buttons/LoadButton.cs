using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;
using MonoGame_Super_Pang.Config;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.UI;

public class LoadButton : AnimatedButton
{
    private TextRuntime _textMoney;

    public bool isNewGame{get;set;}

    public DeleteButton _deleteButton;

    private AnimatedSprite _chestAnimation;
    private SpriteRuntime _chestSprite;

    public LoadButton(TextureAtlas atlas) : base(atlas)
    {
        _deleteButton = new DeleteButton();
        InitializeButton();
    }

    private void InitializeButton()
    {
        // Access the visual
        ButtonVisual visual = (ButtonVisual)this.Visual;

        this.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;
        this.HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute;

        TextRuntime textInstance = visual.TextInstance;
        textInstance.Text = "NewGame";
        textInstance.Anchor(Gum.Wireframe.Anchor.Top);
        textInstance.Y = 10;
        textInstance.Color = Color.Black;
        _textMoney = new GameText("");
        _textMoney.Color = Color.Gold;
        _textMoney.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _textMoney.X = 26.0f;
        _textMoney.Y = -4.0f;

        visual.Children.Insert(1, _textMoney);

        isNewGame = true;

        //CreateChestAnimation();

        // Add event handler for mouse hover focus.
        visual.RollOn += HandleRollOn;
    }

    public void setTextMoney(string textMoney)
    {
        _textMoney.Text = textMoney;
    }

    public void Update(GameTime gameTime)
    {
        if(_chestSprite != null)
        {
            _chestAnimation.Update(gameTime);
            TextureRegion currentChestFrame = _chestAnimation.GetCurrentFrame();
            _chestSprite.Texture = currentChestFrame.Texture;
            _chestSprite.SourceRectangle = currentChestFrame.SourceRectangle;
            _chestSprite.Width = currentChestFrame.SourceRectangle.Width;
            _chestSprite.Height = currentChestFrame.SourceRectangle.Height;
        }
    }

    private void CreateChestAnimation()
    {
        TextureAtlas chestAtlas = TextureAtlas.FromFile(Core.Content, "images/Coins/chest_atlas.xml");

        // Get the multi-frame chest animation from the atlas
        _chestAnimation = chestAtlas.CreateAnimatedSprite("chest-animation");
        _chestAnimation.Scale = new Vector2(4.0f, 4.0f);

        TextureRegion ChestFrame = _chestAnimation.GetCurrentFrame();

        CreateChestSprite(ChestFrame.Texture, ChestFrame.SourceRectangle);
    }

    private void CreateChestSprite(Texture2D texture, Rectangle sourceRectangle)
    {
        // Access the visual
        ButtonVisual visual = (ButtonVisual)this.Visual;

        // Create sprite
        _chestSprite = new SpriteRuntime
        {
            Texture = texture,
            SourceRectangle = sourceRectangle,
            WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
            Width = sourceRectangle.Width,
            Height = sourceRectangle.Height,
            TextureAddress = Gum.Managers.TextureAddress.Custom
        };

        _chestSprite.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _chestSprite.X = 10.0f;
        _chestSprite.Y = -5.0f;

        visual.Background.AddChild(_chestSprite);
    }

    public void LoadButtonStats(PlayerStats pStats, Gum.Wireframe.Anchor anchor, float deleteX, float deleteY)
    {
        Text = pStats.Name;
        setTextMoney(pStats.Money.ToString());
        isNewGame = false;
        _deleteButton.Anchor(anchor);
        _deleteButton.Text = "";
        _deleteButton.Y = deleteY;
        _deleteButton.X = deleteX;
        if(_chestSprite == null)
        {
            CreateChestAnimation();
        }
    }

    public void DeleteAnimations()
    {
        ButtonVisual visual = (ButtonVisual)this.Visual;
        visual.Background.RemoveChild(_chestSprite);
        _chestSprite = null;
    }
}