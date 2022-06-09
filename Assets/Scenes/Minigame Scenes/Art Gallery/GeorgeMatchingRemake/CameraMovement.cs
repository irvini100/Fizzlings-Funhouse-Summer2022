using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    // Animation curve for camera speed when changing positions
    public AnimationCurve cameraFlySpeed;
    // Checks to make sure the player doesn't go through menus more times than necessary
    public bool startClicked;
    public bool continueClicked;
    // Current level
    public int level = 1;
    // initialize var for distance calculating on camera points
    public Vector3 start;
    // Use this to make camera pan before cards spawn
    public bool cameraReachedPosition;
    // list of camera points for each level
    public List<GameObject> cameraTransforms;

    // Start is called before the first frame update
    void Start()
    {
        // Camera reached point will defaultly be true on start
        cameraReachedPosition = true;
        // Check that playerpref has been made
        if (!PlayerPrefs.HasKey("MatchingGameProgress"))
        {
            // set progess to level 1
            PlayerPrefs.SetInt("MatchingGameProgress", 1);
            // set camera at level 1 point
            Camera.main.transform.position = Camera.main.GetComponent<CameraMovement>().cameraTransforms[PlayerPrefs.GetInt("MatchingGameProgress") - 1].transform.position;
        }
        else
        {
            // Set camera to correct level point based on player pref on start
            level = PlayerPrefs.GetInt("MatchingGameProgress");
            Camera.main.transform.position = Camera.main.GetComponent<CameraMovement>().cameraTransforms[PlayerPrefs.GetInt("MatchingGameProgress") - 1].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Halt camera movement when win screen is displayed
        if (level != 6 || (gameObject.GetComponent<CameraMovement>().continueClicked || PlayerPrefs.GetInt("MatchingGameProgress") > 5))
        {
            // Use smooth movement system is cameraPanToNextPoint checked by designers
            if (gameObject.GetComponent<PlaceCards>().cameraPanToNextPoint)
            {
                // Set start point depending on which level you're on to avoid out of bounds issues
                if (level > 1)
                {
                    start = cameraTransforms[level - 2].transform.position;
                }
                else if (Vector3.Distance(gameObject.transform.position, cameraTransforms[0].transform.position) > 0.001f)
                {
                    start = cameraTransforms[1].transform.position;
                }
                else
                {
                    start = cameraTransforms[0].transform.position;
                }
                // Only pan if level !> 6 so that Place Cards script has a chance to change level back to 6 after infinite level loop complete
                if (level <= 6)
                {
                    // Check distance between camera and level point
                    if (Vector3.Distance(gameObject.transform.position, cameraTransforms[level - 1].transform.position) > 0.001f)
                    {
                        // Disable level selector during panning
                        gameObject.GetComponent<PlaceCards>().OpenLevelSelector.GetComponent<Button>().enabled = false;
                        // Get distances
                        float totalDistance = Vector3.Distance(start, cameraTransforms[level - 1].transform.position);
                        float currentDistance = Vector3.Distance(gameObject.transform.position, cameraTransforms[level - 1].transform.position);

                        // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                        float speedBasedOnPosition = currentDistance / totalDistance;

                        // Get Y value on animation graph for speed. 1f - cameraFlySpeed ensures graph is read from left to right for designers
                        float currentSpeed = cameraFlySpeed.Evaluate(1f - speedBasedOnPosition);

                        // MoveTowards next waypoint at currentSpeed
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, cameraTransforms[level - 1].transform.position, Time.deltaTime * currentSpeed);
                        // Set bool to true and reenable level selector when destination reached
                        if (Vector3.Distance(gameObject.transform.position, cameraTransforms[level - 1].transform.position) <= 0.001f)
                        {
                            cameraReachedPosition = true;
                            gameObject.GetComponent<PlaceCards>().OpenLevelSelector.GetComponent<Button>().enabled = true;
                        }
                    }
                }
                else
                {
                    // else set true and enabled if level passed 6 and was caught
                    cameraReachedPosition = true;
                    gameObject.GetComponent<PlaceCards>().OpenLevelSelector.GetComponent<Button>().enabled = true;
                }
            }
            else
            {
                // Use snapping movement system if designers specify
                gameObject.transform.position = cameraTransforms[level - 1].transform.position;
                cameraReachedPosition = true;
            }
            
            
            // George's Cheat codes
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                level++;
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                level--;
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (PlayerPrefs.HasKey("MatchingGameProgress"))
                {
                    PlayerPrefs.DeleteKey("MatchingGameProgress");
                }
            }
        }
    }
}
