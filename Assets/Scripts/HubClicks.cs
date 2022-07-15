using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HubClicks : MonoBehaviour
{
    // Get UI manager so we can call UI functions
    public GameObject UIManager;
    public GameObject UpButton;
    public GameObject DownButton;
    public GameObject RightButton;
    public GameObject LeftButton;

    private bool isPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name == UpButton.name || result.gameObject.name == DownButton.name || result.gameObject.name == RightButton.name || result.gameObject.name == LeftButton.name)
            {
                return true;
            }
        }
        return false;
    }

    private void Doors()
    {
        // Check for left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                // Cast ray to where user clicks (GetRayIntersection is the only function I could find that defaultly works with Perspective Camera)
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
                // check if an object with a collider was found
                if (hit.collider != null && !isPointerOverUIObject())
                {     
                    Debug.Log("Hit " + hit.collider.tag);
                    if(hit.collider.tag == "ArtDoor")
                    {
                        SceneManager.LoadScene("Art Room");
                    }                           
                    else if(hit.collider.tag == "FarmDoor")
                    {
                        SceneManager.LoadScene("NewZoo");
                    }
                    else if(hit.collider.tag == "HangarDoor")
                    {
                        SceneManager.LoadScene("Hangar");
                    }
                    else if(hit.collider.tag == "MusicDoor")
                    {
                        SceneManager.LoadScene("Music Room");
                    }
                    else if(hit.collider.tag == "LibraryDoor")
                    {
                        SceneManager.LoadScene("Library");
                    }
                    else if(hit.collider.tag == "OceanDoor")
                    {
                        SceneManager.LoadScene("Ocean Room");
                    }
                    else if(hit.collider.tag == "TrophyDoor")
                    {  
                        SceneManager.LoadScene("Trophy Room");
                    }
                    else if(hit.collider.tag == "GarageDoor")
                    {
                        SceneManager.LoadScene("Garage");
                    }
                    else if(hit.collider.tag == "HotAirBalloon")
                    {
                        Debug.Log("You hit the hot air balloon. Nothing else because theres no minigame attached yet");
                    }
                }
            }

    }

    void Update()
    {
        Doors();
    }
 
}
