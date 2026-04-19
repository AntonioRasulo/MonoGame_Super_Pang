using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using System;

namespace MonoGame_Super_Pang.UI;

public class NewGamePanel : PangPanel
{
    private TextBox _newGameNametextBox;
    private AnimatedButton _newGameBackButton;
    private AnimatedButton confirmButton;
    private bool _isLastFocusedBackButton = false;

    public NewGamePanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        confirmButton = new AnimatedButton(_GUIatlas);
        confirmButton.Text = "CONFIRM";
        confirmButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        confirmButton.X = 28f;
        confirmButton.Y = -10f;
        confirmButton.Click += TitlePanelManager.handleConfirmNameClicked;
        confirmButton.KeyDown += updateFlagButton;
        _panel.AddChild(confirmButton);

        _newGameNametextBox = new TextBox();
        _newGameNametextBox.Width = 200;
        _newGameNametextBox.Anchor(Gum.Wireframe.Anchor.Center);
        _newGameNametextBox.Placeholder = "";
        _panel.AddChild(_newGameNametextBox);

        _newGameBackButton = new AnimatedButton(_GUIatlas);
        _newGameBackButton.Text = "BACK";
        _newGameBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _newGameBackButton.X = -28f;
        _newGameBackButton.Y = -10f;
        _newGameBackButton.Click += TitlePanelManager.HandleStartClicked;
        _newGameBackButton.KeyDown += updateFlagButton;
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
            if (_isLastFocusedBackButton)
            {
                confirmButton.IsFocused = true;
            }
            else
            {
                _newGameBackButton.IsFocused = true;
            }
        }
    }

    private void updateFlagButton(Object sender, KeyEventArgs e)
    {
        if(sender == confirmButton)
        {
            if (e.Key == Keys.Up || e.Key == Keys.Left)
            {
                _isLastFocusedBackButton = false;
            }
        }
        else if(sender == _newGameBackButton)
        {
            if (e.Key == Keys.Down || e.Key == Keys.Right)
            {
                _isLastFocusedBackButton = true;
            }
        }
    }

}