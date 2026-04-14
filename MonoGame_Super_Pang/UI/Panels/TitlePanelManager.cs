using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using System;

namespace MonoGame_Super_Pang.UI;

public class TitlePanelManager
{
    private static TitleScreenButtonsPanel _titleScreenButtonsPanel;
    private static LoadGamePanel _loadGamePanel;
    private static NewGamePanel _newGamePanel;
    private static OptionsPanel _optionsPanel;
    private static DeleteGamePanel _deleteGamePanel;

    public static SoundEffect _uiSoundEffect;

    public TitlePanelManager()
    {
        _titleScreenButtonsPanel = new TitleScreenButtonsPanel();
        _loadGamePanel = new LoadGamePanel();
    }

    private static void LoadContent()
    {
        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/Confirm 1");
    }

    public static void HandleStartClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.SetIsVisible(false);

        _loadGamePanel.SetIsVisible(true);

        _newGamePanel.SetIsVisible(false);

        // Set the options panel to be visible.
        _optionsPanel.SetIsVisible(false);

        _deleteGamePanel.SetIsVisible(false);

        _newGameNametextBox.Text = "";
    }

    public static void HandleOptionsClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Set the title panel to be invisible.
        _titleScreenButtonsPanel.SetIsVisible(false);

        _loadGamePanel.SetIsVisible(false);

        _newGamePanel.SetIsVisible(false);

        _deleteGamePanel.SetIsVisible(false);

        // Set the options panel to be visible.
        _optionsPanel.SetIsVisible(true);

        // Give the back button on the options panel focus.
        _optionsBackButton.IsFocused = true;
    }

    public static bool IsTitlePanelVisible()
    {
        return _titleScreenButtonsPanel.IsVisible();
    }

}