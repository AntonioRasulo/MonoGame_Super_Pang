using Microsoft.Xna.Framework.Input;
using MonoGame_Super_Pang.Config;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGame_Super_Pang.Scenes;
using Gum.Forms.Controls;
using MonoGameGum;
using System;
using System.IO;

namespace MonoGame_Super_Pang.UI;

public class LoadGamePanel : PangPanel
{
    private TextureButton _loadButton1;
    private TextureButton _loadButton2;
    private TextureButton _loadButton3;

    private TextureButton _newGameButton;

    private AnimatedButton _loadBackButton;

    private bool _isLastFocusedLoadBackButton = false;

    private string _saveToDelete;

    TextureButton _gameToDelete;


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

        _loadButton1 = new(loadGamePaperRegion.Texture, loadGamePaperRegion.SourceRectangle);
        _loadButton2 = new(loadGamePaperRegion.Texture, loadGamePaperRegion.SourceRectangle);
        _loadButton3 = new(loadGamePaperRegion.Texture, loadGamePaperRegion.SourceRectangle);

        _loadButton1.Click += HandleLoadButton;
        _loadButton1.Anchor(Gum.Wireframe.Anchor.Left);
        _loadButton1.X = 25;
        _loadButton1.Y = 0;
        _loadButton1.SetScale(1.8f);

        if(PlayerStatsManager.pStats1 != null)
        {
            LoadButton(PlayerStatsManager.pStats1, _loadButton1, Gum.Wireframe.Anchor.Left, 90, -58);
        }

        _loadButton2.Anchor(Gum.Wireframe.Anchor.Center);
        _loadButton2.Click += HandleLoadButton;
        _loadButton2.X = 0;
        _loadButton2.Y = 0;
        _loadButton2.SetScale(1.8f);

        if(PlayerStatsManager.pStats2 != null)
        {
            LoadButton(PlayerStatsManager.pStats2, _loadButton2, Gum.Wireframe.Anchor.Center, 33, -58);
        }

        _loadButton3.Anchor(Gum.Wireframe.Anchor.Right);
        _loadButton3.Click += HandleLoadButton;
        _loadButton3.X = -25;
        _loadButton3.Y = 0;
        _loadButton3.SetScale(1.8f);
        _loadButton3.KeyDown += updateFlagLoadButton;

        if(PlayerStatsManager.pStats3 != null)
        {
            LoadButton(PlayerStatsManager.pStats3, _loadButton3, Gum.Wireframe.Anchor.Right, -24, -58);
        }

        _loadBackButton = new AnimatedButton(_GUIatlas);
        _loadBackButton.Text = "BACK";
        _loadBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _loadBackButton.X = -28f;
        _loadBackButton.Y = -10f;
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
            PlayerStatsManager.pStats1 = new PlayerStats();
            PlayerStatsManager.pStats1.Name = newGameText;
            PlayerStatsManager.pStats1.Money = 0;
            PlayerStatsManager.pStats1.Path = PlayerStatsManager.PATH1;

            PlayerStats.SaveGame(PlayerStatsManager.pStats1);
            LoadButton(PlayerStatsManager.pStats1, _loadButton1, Gum.Wireframe.Anchor.Left, 90, -58);

            TitlePanelManager.GoToStartGamePanel(PlayerStatsManager.pStats1);
        }
        if (_newGameButton == _loadButton2)
        {
            PlayerStatsManager.pStats2 = new PlayerStats();
            PlayerStatsManager.pStats2.Name = newGameText;
            PlayerStatsManager.pStats2.Money = 0;
            PlayerStatsManager.pStats2.Path = PlayerStatsManager.PATH2;

            PlayerStats.SaveGame(PlayerStatsManager.pStats2);
            LoadButton(PlayerStatsManager.pStats2, _loadButton2, Gum.Wireframe.Anchor.Center, 33, -58);

            TitlePanelManager.GoToStartGamePanel(PlayerStatsManager.pStats2);
        }
        if (_newGameButton == _loadButton3)
        {
            PlayerStatsManager.pStats3 = new PlayerStats();
            PlayerStatsManager.pStats3.Name = newGameText;
            PlayerStatsManager.pStats3.Money = 0;
            PlayerStatsManager.pStats3.Path = PlayerStatsManager.PATH3;

            PlayerStats.SaveGame(PlayerStatsManager.pStats3);
            LoadButton(PlayerStatsManager.pStats3, _loadButton3, Gum.Wireframe.Anchor.Right, -24, -58);

            TitlePanelManager.GoToStartGamePanel(PlayerStatsManager.pStats3);
        }
    }

    private void LoadButton(PlayerStats pStats, TextureButton loadButton, Gum.Wireframe.Anchor anchor, float deleteX, float deleteY)
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
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(TitlePanelManager.uiSoundEffect);

        PlayerStats pStats = null;

        //TODO load game implementation
        if(sender == _loadButton1)
        {
            if(_loadButton1.isNewGame)
            {
                HandleNewGameClicked(_loadButton1);
                return;
            }
            
            pStats = PlayerStatsManager.pStats1;
        }

        if(sender == _loadButton2)
        {
            if(_loadButton2.isNewGame)
            {
                HandleNewGameClicked(_loadButton2);
                return;
            }
            pStats = PlayerStatsManager.pStats2;
        }

        if(sender == _loadButton3)
        {
            if (_loadButton3.isNewGame)
            {
                HandleNewGameClicked(_loadButton3);
                return;
            }
            pStats = PlayerStatsManager.pStats3;
        }

        TitlePanelManager.GoToStartGamePanel(pStats);
    }

    private void HandleDeleteGameClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(TitlePanelManager.uiSoundEffect);

        _gameToDelete = null;
        _saveToDelete = null;

        if(sender == _loadButton1._deleteButton)
        {
            _gameToDelete = new TextureButton(_loadButton1);
            _saveToDelete = PlayerStatsManager.pStats1.Path;
        }
        if(sender == _loadButton2._deleteButton)
        {
            _gameToDelete = new TextureButton(_loadButton2);
            _saveToDelete = PlayerStatsManager.pStats2.Path;
        }
        if(sender == _loadButton3._deleteButton)
        {
            _gameToDelete = new TextureButton(_loadButton3);
            _saveToDelete = PlayerStatsManager.pStats3.Path;
        }

        _gameToDelete.Click-=HandleLoadButton;
        _gameToDelete.Anchor(Gum.Wireframe.Anchor.Center);

        TitlePanelManager.HandleDeleteGameClicked(_gameToDelete);
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
        _newGameButton = (TextureButton)sender;

        TitlePanelManager.HandleNewGameClicked();

    }

    public void handleConfirmDeleteGameClicked(object sender)
    {
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

        _gameToDelete = null;

        var jsonFile = _saveToDelete + ".json";
        var bakFile = _saveToDelete + ".bak";
        File.Delete(jsonFile);
        File.Delete(bakFile);
        _saveToDelete = null;
    }

    private void CleanButton(TextureButton button)
    {
        button.Text = "NewGame";
        button.setTextMoney("");
        button.isNewGame = true;
        button._deleteButton.Visual.Parent=null;
    }

}