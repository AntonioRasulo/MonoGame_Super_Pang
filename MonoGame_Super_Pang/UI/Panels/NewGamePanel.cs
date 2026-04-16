using Gum.Forms.Controls;
using MonoGameGum;

namespace MonoGame_Super_Pang.UI;

public class NewGamePanel : PangPanel
{
    private TextBox _newGameNametextBox;
    private AnimatedButton _newGameBackButton;
    private AnimatedButton confirmButton;

    public NewGamePanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        _newGameNametextBox = new TextBox();
        _newGameNametextBox.Width = 200;
        _newGameNametextBox.Anchor(Gum.Wireframe.Anchor.Center);
        _newGameNametextBox.Placeholder = "";
        _panel.AddChild(_newGameNametextBox);

        confirmButton = new AnimatedButton(_GUIatlas);
        confirmButton.Text = "CONFIRM";
        confirmButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        confirmButton.X = 28f;
        confirmButton.Y = -10f;
        confirmButton.Click += TitlePanelManager.handleConfirmNameClicked;
        confirmButton.IsFocused = true;
        _panel.AddChild(confirmButton);

        _newGameBackButton = new AnimatedButton(_GUIatlas);
        _newGameBackButton.Text = "BACK";
        _newGameBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _newGameBackButton.X = -28f;
        _newGameBackButton.Y = -10f;
        _newGameBackButton.Click += TitlePanelManager.HandleStartClicked;
        _panel.AddChild(_newGameBackButton);
    }

    public string GetNewGameTextBoxText()
    {
        return _newGameNametextBox.Text;
    }

    public void ClearNewGameTextBox()
    {
        _newGameNametextBox.Text = "";
    }

    public override void Update()
    {
        if(_panel.IsVisible &&
        confirmButton.IsFocused == false &&
        _newGameBackButton.IsFocused == false &&
        _newGameNametextBox.IsFocused == false)
        {
            confirmButton.IsFocused = true;
        }
    }

}