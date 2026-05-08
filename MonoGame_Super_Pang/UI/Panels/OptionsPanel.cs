using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using System;

namespace MonoGame_Super_Pang.UI;

public class OptionsPanel: PangPanel
{
    private OptionsSlider sfxSlider;
    private OptionsSlider musicSlider;
    private AnimatedButton _optionsBackButton;

    bool _isLastFocusedBackButton = false;

    public OptionsPanel()
    {
        _panel = new Panel();
        _panel.Dock(Gum.Wireframe.Dock.Fill);
        _panel.IsVisible = false;
        _panel.AddToRoot();

        TextRuntime optionsText = new TextRuntime();
        optionsText.X = 10;
        optionsText.Y = 10;
        optionsText.Text = "OPTIONS";
        optionsText.UseCustomFont = true;
        optionsText.FontScale = 0.5f;
        optionsText.CustomFontFile = @"fonts/04b_30.fnt";
        optionsText.IsEnabled = false;
        _panel.AddChild(optionsText);

        musicSlider = new OptionsSlider(_GUIatlas);
        musicSlider.Name = "MusicSlider";
        musicSlider.Text = "MUSIC";
        musicSlider.Anchor(Gum.Wireframe.Anchor.Top);
        musicSlider.Y = 30f;
        musicSlider.Minimum = 0;
        musicSlider.Maximum = 1;
        musicSlider.Value = Core.Audio.SongVolume;
        musicSlider.SmallChange = .1;
        musicSlider.LargeChange = .2;
        musicSlider.ValueChanged += HandleMusicSliderValueChanged;
        musicSlider.ValueChangeCompleted += HandleMusicSliderValueChangeCompleted;
        musicSlider.KeyDown += updateFlagButton;
        _panel.AddChild(musicSlider);

        sfxSlider = new OptionsSlider(_GUIatlas);
        sfxSlider.Name = "SfxSlider";
        sfxSlider.Text = "SFX";
        sfxSlider.Anchor(Gum.Wireframe.Anchor.Top);
        sfxSlider.Y = 93;
        sfxSlider.Minimum = 0;
        sfxSlider.Maximum = 1;
        sfxSlider.Value = Core.Audio.SoundEffectVolume;
        sfxSlider.SmallChange = .1;
        sfxSlider.LargeChange = .2;
        sfxSlider.ValueChanged += HandleSfxSliderChanged;
        sfxSlider.ValueChangeCompleted += HandleSfxSliderChangeCompleted;
        _panel.AddChild(sfxSlider);

        _optionsBackButton = new AnimatedButton(_GUIatlas);
        _optionsBackButton.Text = "BACK";
        _optionsBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        _optionsBackButton.X = -28f;
        _optionsBackButton.Y = -10f;
        _optionsBackButton.Click += TitlePanelManager.HandleOptionsButtonBack;
        _optionsBackButton.KeyDown += updateFlagButton;
        _panel.AddChild(_optionsBackButton);
    }

    public void OptionsBackButtonSetFocus(bool IsFocused)
    {
        _optionsBackButton.IsFocused = IsFocused;
    }

    public override void Update()
    {
        if (_optionsBackButton.IsFocused == false &&
            sfxSlider.IsFocused == false &&
            musicSlider.IsFocused == false &&
            _panel.IsVisible == true)
        {
            if (_isLastFocusedBackButton)
            {
                musicSlider.IsFocused = true;
            }
            else
            {
                _optionsBackButton.IsFocused = true;
            }
        }
    }

    private void HandleMusicSliderValueChanged(object sender, EventArgs args)
    {
        // Intentionally not playing the UI sound effect here so that it is not
        // constantly triggered as the user adjusts the slider's thumb on the
        // track.

        // Get a reference to the sender as a Slider.
        var slider = (Slider)sender;

        // Set the global song volume to the value of the slider.
        Core.Audio.SongVolume = (float)slider.Value;
    }

    private void HandleMusicSliderValueChangeCompleted(object sender, EventArgs args)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(TitlePanelManager.uiSoundEffect);
    }

    private void HandleSfxSliderChanged(object sender, EventArgs args)
    {
        // Intentionally not playing the UI sound effect here so that it is not
        // constantly triggered as the user adjusts the slider's thumb on the
        // track.

        // Get a reference to the sender as a Slider.
        var slider = (Slider)sender;

        // Set the global sound effect volume to the value of the slider.;
        Core.Audio.SoundEffectVolume = (float)slider.Value;
    }

    private void HandleSfxSliderChangeCompleted(object sender, EventArgs e)
    {
        // Play the UI Sound effect so the player can hear the difference in audio.
        Core.Audio.PlaySoundEffect(TitlePanelManager.uiSoundEffect);
    }

    private void updateFlagButton(Object sender, KeyEventArgs e)
    {
        if(sender == musicSlider)
        {
            if (e.Key == Keys.Up)
            {
                _isLastFocusedBackButton = false;
            }
        }
        else if(sender == _optionsBackButton)
        {
            if (e.Key == Keys.Down || e.Key == Keys.Right)
            {
                _isLastFocusedBackButton = true;
            }
        }
    }

}