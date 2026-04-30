using Gum.Forms.Controls;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using System;

namespace MonoGame_Super_Pang.UI;

public class DeleteGamePanel : PangPanel
{
    AnimatedButton confirmButton;
    AnimatedButton cancelButton;
    public DeleteGamePanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        TextRuntime text = new TextRuntime();
        text.Anchor(Gum.Wireframe.Anchor.Top);
        text.Text = "Delete saving?";
        text.UseCustomFont = true;
        text.FontScale = 0.25f;
        text.CustomFontFile = @"fonts/04b_30.fnt";
        text.IsEnabled = false;
        _panel.AddChild(text);

        confirmButton = new AnimatedButton(_GUIatlas);
        confirmButton.Text = "CONFIRM";
        confirmButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        confirmButton.X = 28f;
        confirmButton.Y = -10f;
        confirmButton.Click += handleConfirmDeleteGameClicked;
        _panel.AddChild(confirmButton);

        cancelButton = new AnimatedButton(_GUIatlas);
        cancelButton.Text = "CANCEL";
        cancelButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        cancelButton.X = -28f;
        cancelButton.Y = -10f;
        cancelButton.Click += TitlePanelManager.HandleStartClicked;
        _panel.AddChild(cancelButton);
    }

    public override void Update()
    {
        if (_panel.IsVisible &&
        cancelButton.IsFocused == false &&
        confirmButton.IsFocused == false)
        {
            cancelButton.IsFocused = true;
        }
    }

    private void handleConfirmDeleteGameClicked(object sender, EventArgs e)
    {
        TitlePanelManager.handleConfirmDeleteGameClicked(sender);
    }
}