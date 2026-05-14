using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;
using MonoGameGum.GueDeriving;
using MonoGameLibrary.Graphics;

namespace MonoGame_Super_Pang.UI;

public class LoadButton : AnimatedButton
{
    private TextRuntime _textMoney;

    public bool isNewGame{get;set;}

    public DeleteButton _deleteButton;

    public LoadButton(TextureAtlas atlas) : base(atlas)
    {
        _deleteButton = new DeleteButton();
        InitializeButton();
    }

    public LoadButton(LoadButton button, TextureAtlas atlas) : base(atlas)
    {
        InitializeButton();
        this.Width = button.Width;
        this.Height = button.Height;
        Text = button.Text;
        _textMoney.Text = button._textMoney.Text;
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
        _textMoney = new TextRuntime();
        _textMoney.Text = "";
        _textMoney.Anchor(Gum.Wireframe.Anchor.Bottom);

        visual.Children.Insert(1, _textMoney);

        isNewGame = true;

        // Add event handler for mouse hover focus.
        visual.RollOn += HandleRollOn;
    }

    public void setTextMoney(string textMoney)
    {
        _textMoney.Text = textMoney;
    }
}