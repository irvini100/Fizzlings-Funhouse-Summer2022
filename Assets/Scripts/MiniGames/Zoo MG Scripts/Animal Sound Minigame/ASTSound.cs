﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASTSound : MonoBehaviour
{
    //Reference to the "game" Game Object
    public GameObject game;

    //Check if the player is clicking on the different sound choices
    //Play the assigned sound when clicked
    public void Interaction()
    {
        game.GetComponent<ASTGame>().selected = this.GetComponent<AudioSource>().clip.name;
        this.GetComponent<AudioSource>().Play();
    }
}
