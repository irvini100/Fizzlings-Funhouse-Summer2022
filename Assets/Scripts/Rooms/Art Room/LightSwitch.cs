using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightSwitch : Interactable
{
    bool lightOn = false;
    public GameObject background;
    public GameObject drawings;
    public GameObject lights;
    public List<Sprite> backgroundSprites;

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
            // Turn light on or off
            if (lightOn)
            {
                lightOn = false;
            }
            else
            {
                lightOn = true;
            }
        }
        determineSprite();
    }

    // determine which sprite to use
    void determineSprite()
    {
        if (lightOn)
        {
            if (getIsHovered())
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
            background.GetComponent<Image>().sprite = backgroundSprites[1];
            drawings.GetComponent<Image>().sprite = backgroundSprites[3];
            lights.GetComponent<Image>().sprite = backgroundSprites[5];
        }
        else
        {
            if (getIsHovered())
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
            }
            background.GetComponent<Image>().sprite = backgroundSprites[0];
            drawings.GetComponent<Image>().sprite = backgroundSprites[2];
            lights.GetComponent<Image>().sprite = backgroundSprites[4];
        }
    }
}
