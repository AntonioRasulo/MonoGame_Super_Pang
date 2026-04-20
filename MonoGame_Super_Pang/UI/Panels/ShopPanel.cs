using Gum.Forms.Controls;
using MonoGameGum;
using System;

namespace MonoGame_Super_Pang.UI;

class ShopPanel : PangPanel
{
    private AnimatedButton _backButton;

    public ShopPanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        _backButton = new AnimatedButton(_GUIatlas);
        _backButton.Text = "BACK";
        _backButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _backButton.X = -28f;
        _backButton.Y = -10f;
        _backButton.Click += GoToStartGamePanel;
        _panel.AddChild(_backButton);  
    }

    public override void Update()
    {
        
    }

    private void GoToStartGamePanel(object sender, EventArgs e)
    {
        TitlePanelManager.GoToStartGamePanel();
    }
}