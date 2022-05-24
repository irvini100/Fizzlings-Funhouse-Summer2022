using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StartMenuFuctions : MonoBehaviour
{
    //Button to load the Main Scene from the Main Menu
    public void Play_Button()
    {
        SceneManager.LoadScene("Hub");
    }

    //Button to load the Options Menu Scene
    public void Options_Button()
    {
        
    }
    //Button to quit the game from the Main Menu
    public void Quit_Button()
    {
        Application.Quit();
    }

    public AudioMixer mixer; //gets the audio mixer from unity


    public void SetMaster(float Master) //master volume mixer slider
    {

        mixer.SetFloat("Master Volume", Master);

    }

    public void SetMusic(float Music) //music volume mixer slider
    {

        mixer.SetFloat("Music Volume", Music);

    }

    public void SetSFX(float SFX) //SFX volume mixer slider
    {

        mixer.SetFloat("SFX Volume", SFX);

    }


}
