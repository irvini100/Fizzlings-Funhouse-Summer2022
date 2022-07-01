using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightOnHover : MonoBehaviour
{
    GameObject activeInteractable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Cast Ray from mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        // If collider hit
        if (hit.collider != null)
        {
            // if hit light switch
            if (hit.collider.gameObject.GetComponent<Interactable>() != null)
            {
                // set activeInteractable to object
                if (activeInteractable == null)
                {
                    activeInteractable = hit.collider.gameObject;
                }
                activeInteractable.GetComponent<Interactable>().setIsHovered(true);
            }
        }
        // If collider not hit
        else
        {
            // If activeInteractable exists
            if (activeInteractable != null)
            {
                // Set hovering to false and set back to null
                activeInteractable.GetComponent<Interactable>().setIsHovered(false);
                activeInteractable = null;
            }
        }
    }
}
