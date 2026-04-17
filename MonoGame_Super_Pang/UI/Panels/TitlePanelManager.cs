using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using MonoGame_Super_Pang.Config;
using System;

namespace MonoGame_Super_Pang.UI;

public class TitlePanelManager
{
    private static TitleScreenButtonsPanel _titleScreenButtonsPanel;
    private static LoadGamePanel _loadGamePanel;
    private static NewGamePanel _newGamePanel;
    private static OptionsPanel _optionsPanel;
    private static DeleteGamePanel _deleteGamePanel;
    private static StartGamePanel _startGamePanel;

    public static SoundEffect uiSoundEffect;

    public static void LoadContent()
    {
        PangPanel.LoadContent();
        _titleScreenButtonsPanel = new TitleScreenButtonsPanel();
        _loadGamePanel = new LoadGamePanel();
        _newGamePanel = new NewGamePanel();
        _optionsPanel = new OptionsPanel();
        _deleteGamePanel = new DeleteGamePanel();
        _startGamePanel = new StartGamePanel();
        uiSoundEffect = Core.Content.Load<SoundEffect>("audio/Confirm 1");
    }

    public static void Update()
    {
        _titleScreenButtonsPanel.Update();
        _optionsPanel.Update();
        _loadGamePanel.Update();
        _deleteGamePanel.Update();
        _newGamePanel.Update();
        _startGamePanel.Update();
    }

    public static void HandleStartClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.SetIsVisible(false);

        _loadGamePanel.SetIsVisible(true);

        _newGamePanel.SetIsVisible(false);

        // Set the options panel to be visible.
        _optionsPanel.SetIsVisible(false);

        _deleteGamePanel.SetIsVisible(false);

        _newGamePanel.ClearNewGameTextBox();
    }

    public static void HandleOptionsClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.SetIsVisible(false);

        _loadGamePanel.SetIsVisible(false);

        _newGamePanel.SetIsVisible(false);

        _deleteGamePanel.SetIsVisible(false);

        // Set the options panel to be visible.
        _optionsPanel.SetIsVisible(true);

        // Give the back button on the options panel focus.
        _optionsPanel.OptionsBackButtonSetFocus(true);
    }

    public static void HandleOptionsButtonBack(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Set the title panel to be visible.
        _titleScreenButtonsPanel.SetIsVisible(true);

        // Set the options panel to be invisible.
        _optionsPanel.SetIsVisible(false);

        _loadGamePanel.SetIsVisible(false);

        _newGamePanel.SetIsVisible(false);

        _deleteGamePanel.SetIsVisible(false);

        // Give the options button on the title panel focus since we are coming
        // back from the options screen.
        _titleScreenButtonsPanel.SetOptionButtonFocus(true);
    }

    public static void handleConfirmNameClicked(object sender, EventArgs e)
    {
        _loadGamePanel.handleConfirmNameClicked(_newGamePanel.GetNewGameTextBoxText());
        _newGamePanel.ClearNewGameTextBox();
    }

    public static bool IsTitlePanelVisible()
    {
        return _titleScreenButtonsPanel.IsVisible();
    }

    public static void HandleDeleteGameClicked(TextureButton gameToDelete)
    {
        _deleteGamePanel.AddChild(gameToDelete);

        // Set the title panel to be visible.
        _titleScreenButtonsPanel.SetIsVisible(false);

        // Set the options panel to be invisible.
        _optionsPanel.SetIsVisible(false);

        _loadGamePanel.SetIsVisible(false);

        _newGamePanel.SetIsVisible(false);

        _deleteGamePanel.SetIsVisible(true);
    }

    public static void HandleNewGameClicked()
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.SetIsVisible(false);

        _loadGamePanel.SetIsVisible(false);

        _newGamePanel.SetIsVisible(true);

        // Set the options panel to be visible.
        _optionsPanel.SetIsVisible(false);

        _deleteGamePanel.SetIsVisible(false);
    }

    public static void handleConfirmDeleteGameClicked(object sender)
    {
        _loadGamePanel.handleConfirmDeleteGameClicked(sender);

        _loadGamePanel.SetIsVisible(true);

        _deleteGamePanel.SetIsVisible(false);

    }

    public static void HandleBackStartGameClicked(object sender, EventArgs e)
    {
        _startGamePanel.SetIsVisible(false);
        _loadGamePanel.SetIsVisible(true);
    }

    public static void GoToStartGamePanel(PlayerStats pStats)
    {
        _titleScreenButtonsPanel.SetIsVisible(false);
        _loadGamePanel.SetIsVisible(false);
        _newGamePanel.SetIsVisible(false);
        _optionsPanel.SetIsVisible(false);
        _deleteGamePanel.SetIsVisible(false);
        _startGamePanel.SetIsVisible(true);
        _startGamePanel.pStats = pStats;
    }

}