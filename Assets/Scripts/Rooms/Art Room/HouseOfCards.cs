using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseOfCards : Interactable
{
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
            SceneManager.LoadScene("MatchingGame");
        }
        determineSprite();
    }

    // determine which sprite to use
    void determineSprite()
    {
        if (getIsHovered())
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
    }
}
