using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int DoorPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Camera reached point will defaultly be true on start
        cameraReachedPosition = true;
        Camera.main.transform.position = Camera.main.GetComponent<NewHubCamera>().cameraTransforms[0].transform.position;
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

    void Update()
    {
        if(isClickedRight == true)
        {
            if(DoorPoint <= 10)
            {
                if (Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint + 1].transform.position) > 0.001f)
                {
                    //disalbes buttons during panning
                    //gameObject.GetTag<CameraButton>().enabled = false;

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
                    //Set bool to true and reenable buttons when destination reached
                    if (Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint + 1].transform.position) <= 0.001f)
                    {
                        cameraReachedPosition = true;
                        if(cameraReachedPosition == true)
                        {
                        DoorPoint ++;
                        isClickedRight = false;
                        //gameObject.GetTag<CameraButton>().enabled = true;
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
                    //disalbes buttons during panning
                    //gameObject.GetTag<CameraButton>().enabled = false;

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
                    //Set bool to true and reenable buttons when destination reached
                    if (Vector3.Distance(gameObject.transform.position, cameraTransforms[DoorPoint - 1].transform.position) <= 0.001f)
                    {
                        cameraReachedPosition = true;
                        if(cameraReachedPosition == true)
                        {
                        DoorPoint --;
                        isClickedLeft = false;
                        //gameObject.GetTag<CameraButton>().enabled = true;
                        }
                    }    
                }  
            }
        }
    }
    
}
