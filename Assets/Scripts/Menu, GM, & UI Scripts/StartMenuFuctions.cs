using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartMenuFuctions : MonoBehaviour
{
    public AudioMixer mixer; //gets the audio mixer from unity
    //public Dropdown resolutionDropdown;
    //Resolution[] resolutions;
    public AudioSource buttonClick;

    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Toggle fullscreenToggle;

    // tmp vars for playerprefs
    private int playerMasterVolume;
    private int playerMusicVolume;
    private int playerSFXVolume;
    private int playerFullscreen;

    void Start()
    {
        //resolutions = Screen.resolutions;
        //resolutionDropdown.ClearOptions(); //clears preset resolution options
        //List<string> options = new List<string>(); //creates a list of strings for our options
        
        //loops through each of the options in our resolutions array
        //for (int i = 0; i < resolutions.Length; i++) 
        //{
        //    string option = resolutions[i].width + " x " + resolutions[i].height; //displays them as a string
        //    options.Add(option); //adds those to the option list
        //}
        //resolutionDropdown.AddOptions(options); //adds option list to the resolution dropdown
        
        // use PlayerPrefs to set game settings
        if (PlayerPrefs.HasKey("Settings Changed"))
        {
            SetMaster(PlayerPrefs.GetInt("Master Volume"));
            SetMusic(PlayerPrefs.GetInt("Music Volume"));
            SetSFX(PlayerPrefs.GetInt("SFX Volume"));
            if (PlayerPrefs.GetInt("Fullscreen") == 1)
            {
                SetFullScreen(true);
            }
            else
            {
                SetFullScreen(false);
            }

            // Set values in game to show player prefs
            masterVolumeSlider.value = PlayerPrefs.GetInt("Master Volume");
            sfxVolumeSlider.value = PlayerPrefs.GetInt("SFX Volume");
            musicVolumeSlider.value = PlayerPrefs.GetInt("Music Volume");
            if (PlayerPrefs.GetInt("Fullscreen") == 1)
            {
                fullscreenToggle.isOn = true;
            }
            else
            {
                fullscreenToggle.isOn = false;
            }
        }
    }

    //Button to load the Main Scene from the Main Menu
    public void Play_Button()
    {
        SceneManager.LoadScene("Hub");
    }

    //Button to quit the game from the Main Menu
    public void Quit_Button()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    //master volume mixer slider
    public void SetMaster(float Master)
    {
        // set tmp masterVolume
        playerMasterVolume = (int) Master;
        // set exposed var to new volume
        mixer.SetFloat("Master Volume", Master);
    }

    //music volume mixer slider
    public void SetMusic(float Music)
    {
        // set tmp masterVolume
        playerMusicVolume = (int) Music;
        // set exposed var to new volume
        mixer.SetFloat("Music Volume", Music);
    }

    //SFX volume mixer slider
    public void SetSFX(float SFX)
    {
        // set tmp sfxVolume
        playerSFXVolume = (int) SFX;
        // set exposed var to new volume
        mixer.SetFloat("SFX Volume", SFX);
    }

    ////quality settings dropdown
    //public void SetQuality (int qualityIndex)
    //{
    //    QualitySettings.SetQualityLevel(qualityIndex);
    //}
    
    //Fullscreen toggle
    public void SetFullScreen (bool isFullscreen)
    {
        // set tmp fullscreen
        if (isFullscreen)
        {
            playerFullscreen = 1;
        }
        else
        {
            playerFullscreen = 0;
        }
        // set game to fullscreen or windowed
        Screen.fullScreen = isFullscreen;
    }

    public void PlayClick()
    {
        buttonClick.Play();
    }

    public void saveSettings()
    {
        PlayerPrefs.SetInt("Settings Changed", 1);
        PlayerPrefs.SetInt("Master Volume", playerMasterVolume);
        PlayerPrefs.SetInt("Music Volume", playerMusicVolume);
        PlayerPrefs.SetInt("SFX Volume", playerSFXVolume);
        PlayerPrefs.SetInt("Fullscreen", playerFullscreen);
    }
}
