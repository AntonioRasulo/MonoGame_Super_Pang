using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Managers;
using MonoGameLibrary.Graphics;
using MonoGameGum.GueDeriving;
using MonoGame_Super_Pang.Config;
using System;

namespace MonoGame_Super_Pang.UI;

public class ShopFailedBuyPanel: PangPanel
{

    public TextRuntime _text;

    public ShopFailedBuyPanel()
    {
        _panel = new Panel();
        _panel.Anchor(Gum.Wireframe.Anchor.Center);
        _panel.WidthUnits = DimensionUnitType.Absolute;
        _panel.HeightUnits = DimensionUnitType.Absolute;
        _panel.Width = 264.0f;
        _panel.Height = 70.0f;
        _panel.IsVisible = false;

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

        _text = new TextRuntime();
        _text.Text = "";
        _text.UseCustomFont = true;
        _text.CustomFontFile = "fonts/04b_30.fnt";
        _text.FontScale = 0.2f;
        _text.X = 10.0f;
        _text.Y = 10.0f;
        _panel.AddChild(_text);

        AnimatedButton okButton = new AnimatedButton(_GUIatlas);
        okButton.Text = "OK";
        okButton.Anchor(Gum.Wireframe.Anchor.Bottom);
        okButton.X = 0f;
        okButton.Y = -9.0f;

        okButton.Click += OkButtonClicked;

        _panel.AddChild(okButton);

    }

    private void OkButtonClicked(object sender, EventArgs e)
    {
        _panel.IsVisible = false;
        ShopPanel.setButtonsIsEnabled(true);
    }

    public void SetText(ShopItems item, int prize = -1)
    {
        if(prize != -1)
        {
            _text.Text = "You don't have enough money to upgrade\n" + ShopItemsConfig.itemsText[item] + ".\n";
            _text.Text += "The price of the item is " + prize.ToString();
        }
        else
        {
            _text.Text = "Reached maximum level for "+ShopItemsConfig.itemsText[item] + ".";
        }
    }
}