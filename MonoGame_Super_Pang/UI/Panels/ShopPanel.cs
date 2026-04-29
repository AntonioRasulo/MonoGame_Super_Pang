using Gum.Forms.Controls;
using MonoGame_Super_Pang.Config;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System;

namespace MonoGame_Super_Pang.UI;

class ShopPanel : PangPanel
{
    private static AnimatedButton _backButton;
    private TextureAtlas _itemsAtlas;

    private static PowerUpButton harpoonButton;
    private static PowerUpButton speedButton;
    private static PowerUpButton livesButton;

    private PowerUpButton lastButtonPressed;

    private ConfirmUpgradeItemPanel _confirmUpgradePanel;
    private ShopFailedBuyPanel _shopFailedPanel;

    private const float BUTTONDISTANCEX = 30.0f;

    public ShopPanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        _confirmUpgradePanel = new ConfirmUpgradeItemPanel();
        _confirmUpgradePanel.confirmButton.Click += ConfirmBuy;
        _panel.AddChild(_confirmUpgradePanel.Visual());

        _shopFailedPanel = new ShopFailedBuyPanel();
        _panel.AddChild(_shopFailedPanel.Visual());

        _backButton = new AnimatedButton(_GUIatlas);
        _backButton.Text = "BACK";
        _backButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _backButton.X = -28f;
        _backButton.Y = -10f;
        _backButton.Click += GoToStartGamePanel;
        _panel.AddChild(_backButton);

        _itemsAtlas = TextureAtlas.FromFile(Core.Content, "images/Items/items-atlas.xml");

        TextureRegion harpoonText = _itemsAtlas.GetRegion("harpoonTexture");
        TextureRegion livesText = _itemsAtlas.GetRegion("livesSprite");

        harpoonButton = new PowerUpButton(harpoonText.Texture, harpoonText.SourceRectangle, _GUIatlas, ShopItems.HARPOON);
        harpoonButton.Anchor(Gum.Wireframe.Anchor.TopLeft);
        harpoonButton.X = BUTTONDISTANCEX;
        harpoonButton.Y = 30.0f;
        harpoonButton.Click += ShowConfirmPanel;
        _panel.AddChild(harpoonButton);

        Texture2D speed2DTexture = Core.Content.Load<Texture2D>("images/UI/white_wings");
        TextureRegion speedRegion = new TextureRegion(speed2DTexture, 0, 0, speed2DTexture.Width, speed2DTexture.Height);

        speedButton = new PowerUpButton(speedRegion.Texture, speedRegion.SourceRectangle, _GUIatlas, ShopItems.SPEED);
        speedButton.Anchor(Gum.Wireframe.Anchor.TopLeft);
        speedButton.X = 2*BUTTONDISTANCEX;
        speedButton.Y = 30.0f;
        speedButton.Click += ShowConfirmPanel;
        speedButton.SetScale(0.02f);
        _panel.AddChild(speedButton);

        livesButton = new PowerUpButton(livesText.Texture, livesText.SourceRectangle, _GUIatlas, ShopItems.LIVES);
        livesButton.Anchor(Gum.Wireframe.Anchor.TopLeft);
        livesButton.X = 3*BUTTONDISTANCEX;
        livesButton.Y = 30.0f;
        livesButton.Click += ShowConfirmPanel;
        _panel.AddChild(livesButton);

    }

    public static void InitializePowerUpButtons()
    {
        harpoonButton.SetState(PlayerStatsManager.currentStatsLevels.harpoonLevel);
        speedButton.SetState(PlayerStatsManager.currentStatsLevels.speedLevel);
        livesButton.SetState(PlayerStatsManager.currentStatsLevels.livesLevel);
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
                _shopFailedPanel.SetText(lastButtonPressed.GetItem(), prize);
                _shopFailedPanel.SetIsVisible(true);
            }
            setButtonsIsEnabled(false);
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

    public static void setButtonsIsEnabled(bool isEnabled)
    {
        speedButton.IsEnabled = isEnabled;
        harpoonButton.IsEnabled = isEnabled;
        livesButton.IsEnabled = isEnabled;
        _backButton.IsEnabled = isEnabled;
    }
}