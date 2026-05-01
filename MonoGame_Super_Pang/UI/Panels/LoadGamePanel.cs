using Microsoft.Xna.Framework.Input;
using MonoGame_Super_Pang.Config;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Gum.Forms.Controls;
using MonoGameGum;
using System;
using System.IO;

namespace MonoGame_Super_Pang.UI;

public class LoadGamePanel : PangPanel
{
    private LoadButton _loadButton1;
    private LoadButton _loadButton2;
    private LoadButton _loadButton3;

    private LoadButton _newGameButton;

    private AnimatedButton _loadBackButton;

    private bool _isLastFocusedLoadBackButton = false;

    private string _saveToDelete;

    public LoadGamePanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        TextureAtlas book2Atlas = TextureAtlas.FromFile(Core.Content, "images/UI/Book2_atlas.xml");
        TextureRegion loadGamePaperRegion = book2Atlas.GetRegion("paper-tile-9");

        float screenHeight = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;
        float screenWidth = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;

        _loadButton1 = new(_GUIatlas);
        _loadButton2 = new(_GUIatlas);
        _loadButton3 = new(_GUIatlas);

        _loadButton1.Click += HandleLoadButton;
        _loadButton1.Anchor(Gum.Wireframe.Anchor.TopLeft);
        _loadButton1.X = 2;
        _loadButton1.Y = 10;
        _loadButton1.Width = 240;
        _loadButton1.Height = 50;

        if(PlayerStatsManager.pStats1 != null)
        {
            LoadButton(PlayerStatsManager.pStats1, _loadButton1, Gum.Wireframe.Anchor.TopRight, -65, 30);
        }

        _loadButton2.Anchor(Gum.Wireframe.Anchor.TopLeft);
        _loadButton2.Click += HandleLoadButton;
        _loadButton2.X = 2;
        _loadButton2.Y = 65;
        _loadButton2.Width = 240;
        _loadButton2.Height = 50;

        if(PlayerStatsManager.pStats2 != null)
        {
            LoadButton(PlayerStatsManager.pStats2, _loadButton2, Gum.Wireframe.Anchor.Right, -65, 0);
        }

        _loadButton3.Anchor(Gum.Wireframe.Anchor.TopLeft);
        _loadButton3.Click += HandleLoadButton;
        _loadButton3.X = 2;
        _loadButton3.Y = 120;
        _loadButton3.Width = 240;
        _loadButton3.Height = 50;
        _loadButton3.KeyDown += updateFlagLoadButton;

        if(PlayerStatsManager.pStats3 != null)
        {
            LoadButton(PlayerStatsManager.pStats3, _loadButton3, Gum.Wireframe.Anchor.BottomRight, -65, -30);
        }

        _loadBackButton = new AnimatedButton(_GUIatlas);
        _loadBackButton.Text = "BACK";
        _loadBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _loadBackButton.X = -15f;
        _loadBackButton.Y = -5f;
        _loadBackButton.Click += TitlePanelManager.HandleOptionsButtonBack;
        _loadBackButton.KeyDown += updateFlagLoadButton;

        _panel.AddChild(_loadBackButton);
        _panel.AddChild(_loadButton1);
        _panel.AddChild(_loadButton2);
        _panel.AddChild(_loadButton3);
    }

    public void handleConfirmNameClicked(string newGameText)
    {
        if (_newGameButton == _loadButton1)
        {
            PlayerStatsManager.pStats1 = new PlayerStats(newGameText, PlayerStatsManager.PATH1);

            PlayerStats.SaveGame(PlayerStatsManager.pStats1);
            LoadButton(PlayerStatsManager.pStats1, _loadButton1, Gum.Wireframe.Anchor.TopRight, -65, 30);

            PlayerStatsManager.SelectPlayerStats(1);
            TitlePanelManager.GoToStartGamePanel();
        }
        if (_newGameButton == _loadButton2)
        {
            PlayerStatsManager.pStats2 = new PlayerStats(newGameText, PlayerStatsManager.PATH2);

            PlayerStats.SaveGame(PlayerStatsManager.pStats2);
            LoadButton(PlayerStatsManager.pStats2, _loadButton2, Gum.Wireframe.Anchor.Right, -65, 0);

            PlayerStatsManager.SelectPlayerStats(2);
            TitlePanelManager.GoToStartGamePanel();
        }
        if (_newGameButton == _loadButton3)
        {
            PlayerStatsManager.pStats3 = new PlayerStats(newGameText, PlayerStatsManager.PATH3);

            PlayerStats.SaveGame(PlayerStatsManager.pStats3);
            LoadButton(PlayerStatsManager.pStats3, _loadButton3, Gum.Wireframe.Anchor.BottomRight, -65, -30);

            PlayerStatsManager.SelectPlayerStats(3);
            TitlePanelManager.GoToStartGamePanel();
        }
    }

    private void LoadButton(PlayerStats pStats, LoadButton loadButton, Gum.Wireframe.Anchor anchor, float deleteX, float deleteY)
    {
        loadButton.Text = pStats.Name;
        loadButton.setTextMoney(pStats.Money.ToString());
        loadButton.isNewGame = false;
        loadButton._deleteButton.Click += HandleDeleteGameClicked;
        loadButton._deleteButton.Anchor(anchor);
        loadButton._deleteButton.Text = "";
        loadButton._deleteButton.Y = deleteY;
        loadButton._deleteButton.X = deleteX;
        _panel.AddChild(loadButton._deleteButton);
    }

    private void HandleLoadButton(object sender, EventArgs e)
    {
        //TODO load game implementation
        if(sender == _loadButton1)
        {
            if(_loadButton1.isNewGame)
            {
                HandleNewGameClicked(_loadButton1);
                return;
            }
            
            PlayerStatsManager.SelectPlayerStats(1);
        }

        if(sender == _loadButton2)
        {
            if(_loadButton2.isNewGame)
            {
                HandleNewGameClicked(_loadButton2);
                return;
            }
            PlayerStatsManager.SelectPlayerStats(2);
        }

        if(sender == _loadButton3)
        {
            if (_loadButton3.isNewGame)
            {
                HandleNewGameClicked(_loadButton3);
                return;
            }
            PlayerStatsManager.SelectPlayerStats(3);
        }

        TitlePanelManager.GoToStartGamePanel();
    }

    private void HandleDeleteGameClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(TitlePanelManager.uiSoundEffect);

        _saveToDelete = null;

        if(sender == _loadButton1._deleteButton)
        {
            _saveToDelete = PlayerStatsManager.pStats1.Path;
        }
        if(sender == _loadButton2._deleteButton)
        {
            _saveToDelete = PlayerStatsManager.pStats2.Path;
        }
        if(sender == _loadButton3._deleteButton)
        {
            _saveToDelete = PlayerStatsManager.pStats3.Path;
        }

        setButtonsIsEnabled(false);
        TitlePanelManager.HandleDeleteGameClicked();
    }

    public override void Update()
    {
        if(_loadBackButton.IsFocused == false &&
        _loadButton1.IsFocused == false &&
        _loadButton2.IsFocused == false &&
        _loadButton3.IsFocused == false &&
        _panel.IsVisible == true)
        {
            if (_isLastFocusedLoadBackButton)
            {
                _loadButton3.IsFocused = true;
            }
            else
            {
                _loadBackButton.IsFocused = true;
            }
        }
    }

    private void updateFlagLoadButton(Object sender, KeyEventArgs e)
    {
        if(sender == _loadButton3)
        {
            if (e.Key == Keys.Down || e.Key == Keys.Right)
            {
                _isLastFocusedLoadBackButton = false;
            }
        }
        else if(sender == _loadBackButton)
        {
            if (e.Key == Keys.Up || e.Key == Keys.Left)
            {
                _isLastFocusedLoadBackButton = true;
            }
        }
    }

    private void HandleNewGameClicked(object sender)
    {
        _newGameButton = (LoadButton)sender;

        TitlePanelManager.HandleNewGameClicked();

    }

    public void handleConfirmDeleteGameClicked(object sender)
    {
        setButtonsIsEnabled(true);

        if(PlayerStatsManager.pStats1 != null && _saveToDelete == PlayerStatsManager.pStats1.Path)
        {
            CleanButton(_loadButton1);
        }

        if(PlayerStatsManager.pStats2 != null &&_saveToDelete == PlayerStatsManager.pStats2.Path)
        {
            CleanButton(_loadButton2);
        }

        if(PlayerStatsManager.pStats3 != null && _saveToDelete == PlayerStatsManager.pStats3.Path)
        {
            CleanButton(_loadButton3);
        }

        var jsonFile = _saveToDelete + ".json";
        var bakFile = _saveToDelete + ".bak";
        File.Delete(jsonFile);
        File.Delete(bakFile);
        _saveToDelete = null;
    }

    private void CleanButton(LoadButton button)
    {
        button.Text = "NewGame";
        button.setTextMoney("");
        button.isNewGame = true;
        button._deleteButton.Visual.Parent=null;
    }

    public void UpdateLoadButton()
    {
        if(PlayerStatsManager.currentStats == PlayerStatsManager.pStats1)
        {
            _loadButton1.setTextMoney(PlayerStatsManager.pStats1.Money.ToString());
        }

        if(PlayerStatsManager.currentStats == PlayerStatsManager.pStats2)
        {
            _loadButton2.setTextMoney(PlayerStatsManager.pStats2.Money.ToString());
        }

        if(PlayerStatsManager.currentStats == PlayerStatsManager.pStats3)
        {
            _loadButton3.setTextMoney(PlayerStatsManager.pStats3.Money.ToString());
        }
    }

    public void setButtonsIsEnabled(bool isEnabled)
    {
        _loadButton1.IsEnabled = isEnabled;
        if(_loadButton1._deleteButton != null)
        {
            _loadButton1._deleteButton.IsEnabled = isEnabled;
        }
        _loadButton2.IsEnabled = isEnabled;
        if(_loadButton2._deleteButton != null)
        {
            _loadButton2._deleteButton.IsEnabled = isEnabled;
        }
        _loadButton3.IsEnabled = isEnabled;
        if(_loadButton3._deleteButton != null)
        {
            _loadButton3._deleteButton.IsEnabled = isEnabled;
        }
        _loadBackButton.IsEnabled = isEnabled;
    }


}