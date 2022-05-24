using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartMenuFuctions : MonoBehaviour
{
    public AudioMixer mixer; //gets the audio mixer from unity
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public AudioSource buttonClick;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions(); //clears preset resolution options
        List<string> options = new List<string>(); //creates a list of strings for our options
        
        //loops through each of the options in our resolutions array
        for (int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //displays them as a string
            options.Add(option); //adds those to the option list
        }
        resolutionDropdown.AddOptions(options); //adds option list to the resolution dropdown

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
    }

    //master volume mixer slider
    public void SetMaster(float Master)
    {
        mixer.SetFloat("Master Volume", Master);
    }

    //music volume mixer slider
    public void SetMusic(float Music)
    {
        mixer.SetFloat("Music Volume", Music);
    }

    //SFX volume mixer slider
    public void SetSFX(float SFX)
    {
        mixer.SetFloat("SFX Volume", SFX);
    }

    //quality settings dropdown
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    //Fullscreen toggle
    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void PlayClick()
    {
        buttonClick.Play();
    }
}
