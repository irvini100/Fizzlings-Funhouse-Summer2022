using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHubCamera : MonoBehaviour
{
     // Animation curve for camera speed when changing positions
    public AnimationCurve cameraFlySpeed;
    // initialize var for distance calculating on camera points
    public Vector3 start;
    // Use this to make camera pan before cards spawn
    public bool cameraReachedPosition;
    // list of camera points for each level
    public List<GameObject> cameraTransforms;
    //bool for button clicks
    private bool isClickedRight = false;
    private bool isClickedLeft = false;
    private bool isClickedUp = false;
    private bool isClickedDown = false;
    //int for doorpoints/math
    private int DoorPoint = 0;
    //serialized fields to instantiate the buttons so that I can make them inactive
    [SerializeField] private Button ButtonLeft = null;
    [SerializeField] private Button ButtonRight = null;
    [SerializeField] private Button ButtonUp = null;
    [SerializeField] private Button ButtonDown = null;
    //bools for reaching the max left or right/stairs/roof 
    private bool cameraReachedMax = false;
    private bool cameraReachedStairs = false;
    private bool cameraReachedRoof = false;

    // Start is called before the first frame update
    void Start()
    {
        // Camera reached point will defaultly be true on start
        cameraReachedPosition = true;
        Camera.main.transform.position = Camera.main.GetComponent<NewHubCamera>().cameraTransforms[0].transform.position;
        ButtonLeft.gameObject.SetActive(false);
        ButtonUp.gameObject.SetActive(false);
        ButtonDown.gameObject.SetActive(false);
    }
    //on click event for the UI button
    public void ButtonClickRight()
    {
        isClickedRight = true;
    }
    public void ButtonClickLeft()
    {
        isClickedLeft = true;
    }
    public void ButtonClickUp()
    {
        isClickedUp = true;
    }
    public void ButtonClickDown()
    {
        isClickedDown = true;
    }

    void Update()
    {
        if(isClickedRight == true)
        {
            if(DoorPoint <= 10)
            {
                if (Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint + 1].transform.position) > 0.001f)
                {
                    start = cameraTransforms[DoorPoint].transform.position;
                    
                    //gets distances
                    float totalDistance = Vector3.Distance(start, cameraTransforms[DoorPoint + 1].transform.position);
                    float currentDistance = Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint + 1].transform.position);
                    
                    // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                    float speedBasedOnPosition = currentDistance / totalDistance;

                    // Get Y value on animation graph for speed. 1f - cameraFlySpeed ensures graph is read from left to right for designers
                    float currentSpeed = cameraFlySpeed.Evaluate(1f - speedBasedOnPosition);
                    
                    // MoveTowards next waypoint at currentSpeed
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, cameraTransforms[DoorPoint + 1].transform.position, Time.deltaTime * currentSpeed);
                    
                    //sets bool to false and disables the buttons while moving
                    if(Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint + 1].transform.position) >= 0.001f)
                    {
                        cameraReachedPosition = false;
                        if(cameraReachedPosition == false)
                        {
                            ButtonLeft.gameObject.SetActive(false);
                            ButtonRight.gameObject.SetActive(false);
                            ButtonUp.gameObject.SetActive(false); 
                        
                        }
                    }                    
                    
                    //Set bool to true and reenable buttons when destination reached
                    else
                    {
                        cameraReachedPosition = true;
                        if(cameraReachedPosition == true)
                        {
                            DoorPoint ++;
                            isClickedRight = false;
                            ButtonLeft.gameObject.SetActive(true);
                            ButtonRight.gameObject.SetActive(true);                     
                        }
                    }

                //sets the right button to inactive if the player is unable to go right anymore
                    if(DoorPoint >= 8)
                    {
                        cameraReachedMax = true;
                        if(cameraReachedMax == true && cameraReachedPosition == true)
                        {
                            ButtonRight.gameObject.SetActive(false);
                        } 
                    }    
                }  
            }
        }
    
     if(isClickedLeft == true)
        {
            if(DoorPoint >= 0)
            {
                if (Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint -1].transform.position) > 0.001f)
                {
                    start = cameraTransforms[DoorPoint].transform.position;
                    
                    //gets distances
                    float totalDistance = Vector3.Distance(start, cameraTransforms[DoorPoint - 1].transform.position);
                    float currentDistance = Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint - 1].transform.position);
                    
                    // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                    float speedBasedOnPosition = currentDistance / totalDistance;

                    // Get Y value on animation graph for speed. 1f - cameraFlySpeed ensures graph is read from left to right for designers
                    float currentSpeed = cameraFlySpeed.Evaluate(1f - speedBasedOnPosition);
                    
                    // MoveTowards next waypoint at currentSpeed
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, cameraTransforms[DoorPoint - 1].transform.position, Time.deltaTime * currentSpeed);
                    
                    //sets bool to false and disables the buttons while moving
                    if(Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint - 1].transform.position) >= 0.001f)
                    {
                        cameraReachedPosition = false;
                        if(cameraReachedPosition == false)
                        {
                            ButtonLeft.gameObject.SetActive(false);
                            ButtonRight.gameObject.SetActive(false);
                            ButtonUp.gameObject.SetActive(false);
                            ButtonDown.gameObject.SetActive(false);
                        }
                    }
                    
                    //Set bool to true and reenable buttons when destination reached
                    else
                    {
                        cameraReachedPosition = true;
                        if(cameraReachedPosition == true)
                        {
                            DoorPoint--;
                            isClickedLeft = false;
                            ButtonLeft.gameObject.SetActive(true);
                            ButtonRight.gameObject.SetActive(true);
                            ButtonUp.gameObject.SetActive(true);
                        }
                    } 
                    //sests the left button to inactive if the player is unable to go left anymore.
                    if(DoorPoint > 0)
                    {
                        cameraReachedMax = true;
                        if(cameraReachedMax == true && cameraReachedPosition == true)
                        {
                            ButtonLeft.gameObject.SetActive(true);
                        } 
                    }
                    else
                    {
                        ButtonLeft.gameObject.SetActive(false);
                    }
                }  
            }
        }

        if(DoorPoint == 7)
        {
            cameraReachedStairs = true;
            if(cameraReachedStairs == true)
            {
                ButtonUp.gameObject.SetActive(true);
                if(isClickedUp == true)
                {
                    start = cameraTransforms[DoorPoint].transform.position;
                    //gets distances
                    float totalDistance = Vector3.Distance(start, cameraTransforms[DoorPoint + 2].transform.position);
                    float currentDistance = Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint + 2].transform.position);
                    
                    // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                    float speedBasedOnPosition = currentDistance / totalDistance;

                    // Get Y value on animation graph for speed. 1f - cameraFlySpeed ensures graph is read from left to right for designers
                    float currentSpeed = cameraFlySpeed.Evaluate(1f - speedBasedOnPosition);
                    
                    // MoveTowards next waypoint at currentSpeed
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, cameraTransforms[DoorPoint + 2].transform.position, Time.deltaTime * currentSpeed);
                    
                    //sets bool to false and disables the buttons while moving
                    if(Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint + 2].transform.position) >= 0.001f)
                    {
                        cameraReachedPosition = false;
                        if(cameraReachedPosition == false)
                        {
                            ButtonLeft.gameObject.SetActive(false);
                            ButtonRight.gameObject.SetActive(false);
                            ButtonUp.gameObject.SetActive(false);
                            ButtonDown.gameObject.SetActive(false);
                        }
                    }                    
                    
                    //Set bool to true and reenable buttons when destination reached
                    else
                    {
                        cameraReachedPosition = true;
                        if(cameraReachedPosition == true)
                        {
                            DoorPoint += 2;
                            isClickedUp = false;
                            ButtonLeft.gameObject.SetActive(true);
                            ButtonRight.gameObject.SetActive(true);
                            ButtonUp.gameObject.SetActive(true);                                           
                        }
                    }
                }
            }
            if(cameraReachedPosition == false)
            {
                ButtonUp.gameObject.SetActive(false);
            }
            
        }
        else
        {
            cameraReachedStairs = false;
            ButtonUp.gameObject.SetActive(false);
        }
        //if the camera is at the roof sets camerareachedroof to true and enables the down button
       if(DoorPoint == 9)
       {
            cameraReachedRoof = true;
            if(cameraReachedRoof == true)
            {
                ButtonDown.gameObject.SetActive(true);
                ButtonLeft.gameObject.SetActive(false);
                ButtonRight.gameObject.SetActive(false);
                //if the down button is clicked move towards the stairs.
                if(isClickedDown == true)
                {
                    start = cameraTransforms[DoorPoint].transform.position;
                    //gets distances
                    float totalDistance = Vector3.Distance(start, cameraTransforms[DoorPoint - 2].transform.position);
                    float currentDistance = Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint - 2].transform.position);
                        
                    // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                    float speedBasedOnPosition = currentDistance / totalDistance;

                    // Get Y value on animation graph for speed. 1f - cameraFlySpeed ensures graph is read from left to right for designers
                    float currentSpeed = cameraFlySpeed.Evaluate(1f - speedBasedOnPosition);
                        
                    // MoveTowards next waypoint at currentSpeed
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, cameraTransforms[DoorPoint - 2].transform.position, Time.deltaTime * currentSpeed);
                        
                    //sets bool to false and disables the buttons while moving
                    if(Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint - 2].transform.position) >= 0.001f)
                    {
                        cameraReachedPosition = false;
                        if(cameraReachedPosition == false)
                        {
                            ButtonLeft.gameObject.SetActive(false);
                            ButtonRight.gameObject.SetActive(false);
                            ButtonUp.gameObject.SetActive(false);
                            ButtonDown.gameObject.SetActive(false);  
                        }
                    }                    
                        
                        //Set bool to true and reenable buttons when destination reached
                    else
                    {
                        cameraReachedPosition = true;
                        if(cameraReachedPosition == true)
                        {
                            DoorPoint -= 2;
                            isClickedDown = false;
                            ButtonLeft.gameObject.SetActive(true);
                            ButtonRight.gameObject.SetActive(true);
                            ButtonUp.gameObject.SetActive(true);                        
                        }
                    }
                }
            }
        }
        else
        {
            cameraReachedRoof = false;
            ButtonDown.gameObject.SetActive(false);
        }
    
    }
    
    
}
