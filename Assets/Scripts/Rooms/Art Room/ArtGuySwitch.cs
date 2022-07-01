using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtGuySwitch : Interactable
{
    // Keep track of state
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check for left mouse click and check if being hovered
        if (Input.GetMouseButtonDown(0) && getIsHovered())
        {
            // increment count, set back to 0 if 3
            count++;
            if (count == 3)
            {
                count = 0;
            }
        }
        determineSprite();
    }

    // determine which sprite to use
    void determineSprite()
    {
        // Switch based on count, and choose sprite based on count and isHovered
        switch (count)
        {
            case 0:
                if (getIsHovered())
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
                }
                break;
            case 1:
                if (getIsHovered())
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
                }
                break;
            case 2:
                if (getIsHovered())
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[5];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[4];
                }
                break;
            default:
                break;
        }
    }
}
