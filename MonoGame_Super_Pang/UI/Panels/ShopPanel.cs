using Gum.Forms.Controls;
using MonoGame_Super_Pang.Config;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;

namespace MonoGame_Super_Pang.UI;

class ShopPanel : PangPanel
{
    private AnimatedButton _backButton;
    private TextureAtlas _itemsAtlas;

    private static PowerUpButton harpoonButton;

    private PowerUpButton lastButtonPressed;

    private ConfirmUpgradeItemPanel _confirmUpgradePanel;

    public ShopPanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        _confirmUpgradePanel = new ConfirmUpgradeItemPanel();
        _confirmUpgradePanel.confirmButton.Click += ConfirmBuy;
        _panel.AddChild(_confirmUpgradePanel.Visual());

        _backButton = new AnimatedButton(_GUIatlas);
        _backButton.Text = "BACK";
        _backButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _backButton.X = -28f;
        _backButton.Y = -10f;
        _backButton.Click += GoToStartGamePanel;
        _panel.AddChild(_backButton);

        _itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/Items/items-atlas.xml");

        TextureRegion harpoonText = _itemsAtlas.GetRegion("harpoonTexture");

        harpoonButton = new PowerUpButton(harpoonText.Texture, harpoonText.SourceRectangle, _GUIatlas, ShopItems.HARPOON);
        harpoonButton.Anchor(Gum.Wireframe.Anchor.TopLeft);
        harpoonButton.X = 30.0f;
        harpoonButton.Y = 30.0f;
        harpoonButton.Click += ShowConfirmPanel;
        _panel.AddChild(harpoonButton);

    }

    public static void InitializePowerUpButtons()
    {
        harpoonButton.SetState(PlayerStatsManager.currentStatsLevels.harpoonLevel);
    }

    public override void Update()
    {
    }

    private void GoToStartGamePanel(object sender, EventArgs e)
    {
        TitlePanelManager.GoToStartGamePanel();
    }

    private void ShowConfirmPanel(object sender, EventArgs e)
    {
        lastButtonPressed = (PowerUpButton)sender;
        if(lastButtonPressed.GetState() < PowerUpButtonState.Level3)
        {
            int prize = lastButtonPressed.GetPowerUpPrize();
            if (prize <= PlayerStatsManager.currentStats.Money)
            {
                _confirmUpgradePanel.SetText(lastButtonPressed.GetItem(), prize);
                _confirmUpgradePanel.SetIsVisible(true);
            }
            else
            {
                //TODO panel you can't afford this
            }
        }
    }

    private void ConfirmBuy(object sender, EventArgs e)
    {
        int prize = lastButtonPressed.GetPowerUpPrize();
        PlayerStatsManager.currentStats.Money -= prize;
        lastButtonPressed.LevelUp();
        PowerUpButtonState state = lastButtonPressed.GetState();
        ShopItems itemType = lastButtonPressed.GetItem();
        PlayerStatsManager.SetPlayerStats(state, itemType);
    }
}