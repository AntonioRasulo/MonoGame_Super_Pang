using Gum.DataTypes;
using Gum.Managers;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary.Graphics;
using System;

namespace MonoGame_Super_Pang.UI;

public class DeleteGamePanel : PangPanel
{
    AnimatedButton confirmButton;
    AnimatedButton cancelButton;
    public DeleteGamePanel()
    {
        _panel.Anchor(Gum.Wireframe.Anchor.Center);
        _panel.IsVisible = false;
        _panel.AddToRoot();
        _panel.WidthUnits = DimensionUnitType.Absolute;
        _panel.HeightUnits = DimensionUnitType.Absolute;
        _panel.Width = 264.0f;
        _panel.Height = 70.0f;

        TextureRegion backgroundRegion = _GUIatlas.GetRegion("panel-background");

        NineSliceRuntime background = new NineSliceRuntime();
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Texture = backgroundRegion.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.TextureHeight = backgroundRegion.Height;
        background.TextureWidth = backgroundRegion.Width;
        background.TextureTop = backgroundRegion.SourceRectangle.Top;
        background.TextureLeft = backgroundRegion.SourceRectangle.Left;
        _panel.AddChild(background);

        TextRuntime text = new TextRuntime();
        text.Anchor(Gum.Wireframe.Anchor.Top);
        text.Text = "Delete saving?";
        text.UseCustomFont = true;
        text.FontScale = 0.25f;
        text.CustomFontFile = @"fonts/04b_30.fnt";
        text.IsEnabled = false;
        text.Y = 5f;
        _panel.AddChild(text);

        confirmButton = new AnimatedButton(_GUIatlas);
        confirmButton.Text = "CONFIRM";
        confirmButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        confirmButton.X = 9f;
        confirmButton.Y = -9f;
        confirmButton.Click += handleConfirmDeleteGameClicked;
        _panel.AddChild(confirmButton);

        cancelButton = new AnimatedButton(_GUIatlas);
        cancelButton.Text = "CANCEL";
        cancelButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        cancelButton.X = -9f;
        cancelButton.Y = -9f;
        cancelButton.Click += TitlePanelManager.HandleStartClicked;
        _panel.AddChild(cancelButton);
    }

    private void handleConfirmDeleteGameClicked(object sender, EventArgs e)
    {
        TitlePanelManager.handleConfirmDeleteGameClicked(sender);
    }

    public new void SetIsVisible(bool isVisible)
    {
        base.SetIsVisible(isVisible);
        cancelButton.IsFocused = isVisible;
    }

}