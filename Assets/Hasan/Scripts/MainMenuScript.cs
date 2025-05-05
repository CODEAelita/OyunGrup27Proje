using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource buttonClickSound;

    public Image musicButtonImage;   
    public Image soundEffectButtonImage; 

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite soundEffectOnSprite;
    public Sprite soundEffectOffSprite;

    private bool isMusicOn = true;
    private bool isSoundEffectsOn = true;

    public Button musicToggleButton;
    public Button soundEffectsToggleButton;

    void Start()
    {
        UpdateMusicState();
        UpdateSoundEffectState();

        musicToggleButton.onClick.AddListener(ToggleMusic);
        soundEffectsToggleButton.onClick.AddListener(ToggleSoundEffects);
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        UpdateMusicState();
    }

    private void UpdateMusicState()
    {
        if (isMusicOn)
        {
            backgroundMusic.Play(); 
            musicButtonImage.sprite = musicOnSprite;
        }
        else
        {
            backgroundMusic.Pause(); 
            musicButtonImage.sprite = musicOffSprite; 
        }
    }

    public void ToggleSoundEffects()
    {
        isSoundEffectsOn = !isSoundEffectsOn;
        UpdateSoundEffectState();
    }

    private void UpdateSoundEffectState()
    {
        if (isSoundEffectsOn)
        {
            buttonClickSound.enabled = true; 
            soundEffectButtonImage.sprite = soundEffectOnSprite; 
        }
        else
        {
            buttonClickSound.enabled = false; 
            soundEffectButtonImage.sprite = soundEffectOffSprite; 
        }
    }

    public void PlayButtonClickSound()
    {
        if (isSoundEffectsOn)
        {
            buttonClickSound.Play(); 
        }
    }
}
