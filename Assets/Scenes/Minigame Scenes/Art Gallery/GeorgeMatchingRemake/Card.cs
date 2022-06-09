using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Set default back of card sprite
    public Sprite cardBack;
    // Use to determine if card matched with another by checking if both cards have same matchIntPair
    public int matchIntPair;
    // Set this card to matched so it passes check when all cards are checked for match to progress level
    public bool matched;
    private SpriteRenderer spriteRenderer;
    // Unique identifier for card
    public int cardNum;
    // determine whether to flip card based on click and if it didn't find a match
    public bool cardFlipped;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Change sprite to face using matchIntPair to find correct sprite
    public void changeCardSpriteToFace()
    {
        spriteRenderer.sprite = Camera.main.gameObject.GetComponent<PlaceCards>().cardFaces[matchIntPair];
    }

    // change card sprite back to default sprite
    public void changeCardSpriteToBack()
    {
        spriteRenderer.sprite = Camera.main.gameObject.GetComponent<PlaceCards>().cardBack;
    }
}
