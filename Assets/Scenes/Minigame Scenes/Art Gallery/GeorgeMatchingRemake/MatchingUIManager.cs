using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchingUIManager : MonoBehaviour
{
    // GameObjects to set active and inactive depending on what menu user is in
    public GameObject StartMenu;
    public GameObject OnScreenUI;
    public GameObject LevelSelector;
    public GameObject WinMenu;
    // Score text for when player is on final level
    public GameObject ScoreText;
    // List of buttons for selecting level
    public List<GameObject> levelSelectButtons;
    // Start is called before the first frame update
    void Start()
    {
        // Set only start menu active on start
        StartMenu.SetActive(true);
        OnScreenUI.SetActive(false);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
        ScoreText.SetActive(false);

        // Disable levelSelectButtons based on how much progress the user hasn't made
        if (PlayerPrefs.HasKey("MatchingGameProgress"))
        {
            for (int i = 0; i < levelSelectButtons.Count; i++)
            {
                if (PlayerPrefs.GetInt("MatchingGameProgress") <= i)
                {
                    levelSelectButtons[i].GetComponent<Button>().enabled = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Enable levelSelectButtons based on how much progress the user has made
        if (PlayerPrefs.HasKey("MatchingGameProgress"))
        {
            for (int i = 0; i < levelSelectButtons.Count; i++)
            {
                if (PlayerPrefs.GetInt("MatchingGameProgress") > i)
                {
                    levelSelectButtons[i].GetComponent<Button>().enabled = true;
                }
            }
        }
    }

    // Turns off start menu and turns on OnScreenUI
    public void startGame()
    {
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(true);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
        ScoreText.SetActive(false);
        Camera.main.GetComponent<CameraMovement>().startClicked = true;
    }

    // Load Art Gallery Scene
    public void openArtGallery()
    {
        SceneManager.LoadScene("Art Gallery");
    }

    // Closes all menus except for level selector
    public void openLevelSelector()
    {
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(false);
        LevelSelector.SetActive(true);
        WinMenu.SetActive(false);
    }

    // Close all menus except for win menu
    public void openWinMenu()
    {
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(false);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(true);
    }

    // Called after continue button on win screen is clicked, will never be shown to user again, closes all menus and opens OnScreenUI
    public void continueAfterWin()
    {
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(true);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
        Camera.main.GetComponent<CameraMovement>().continueClicked = true;
    }

    // All functions below are hooked up to 1 button each, corresponding level is loaded and opens OnScreenUI
    public void openLevel1()
    {
        Camera.main.GetComponent<CameraMovement>().level = 1;
        Camera.main.transform.position = Camera.main.GetComponent<CameraMovement>().cameraTransforms[0].transform.position;
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(true);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
    }

    public void openLevel2()
    {
        Camera.main.GetComponent<CameraMovement>().level = 2;
        Camera.main.transform.position = Camera.main.GetComponent<CameraMovement>().cameraTransforms[1].transform.position;
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(true);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
    }

    public void openLevel3()
    {
        Camera.main.GetComponent<CameraMovement>().level = 3;
        Camera.main.transform.position = Camera.main.GetComponent<CameraMovement>().cameraTransforms[2].transform.position;
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(true);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
    }

    public void openLevel4()
    {
        Camera.main.GetComponent<CameraMovement>().level = 4;
        Camera.main.transform.position = Camera.main.GetComponent<CameraMovement>().cameraTransforms[3].transform.position;
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(true);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
    }

    public void openLevel5()
    {
        Camera.main.GetComponent<CameraMovement>().level = 5;
        Camera.main.transform.position = Camera.main.GetComponent<CameraMovement>().cameraTransforms[4].transform.position;
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(true);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
    }

    public void openLevel6()
    {
        Camera.main.GetComponent<CameraMovement>().level = 6;
        Camera.main.transform.position = Camera.main.GetComponent<CameraMovement>().cameraTransforms[5].transform.position;
        StartMenu.SetActive(false);
        OnScreenUI.SetActive(true);
        LevelSelector.SetActive(false);
        WinMenu.SetActive(false);
    }
}
