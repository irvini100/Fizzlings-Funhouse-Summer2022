using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceCards : MonoBehaviour
{
    // Get UI manager so we can call UI functions
    public GameObject UIManager;
    // Get specific UI objects that we don't want the user to be able to click behind
    public GameObject StartMenu;
    public GameObject WinMenu;
    public GameObject LevelSelector;
    public GameObject OpenLevelSelector;
    public GameObject BackButton;
    // Get Score text so we can increment it and display it on level 6
    public GameObject ScoreText;
    public int score = 0;

    // Card Prefab to spawn
    public GameObject defaultCard;

    // Sprites Cards can Have
    public Sprite cardBack;
    public List<Sprite> cardFaces;

    // Vars for checking if 2 cards a match
    private List<int> spawnOrder = new List<int>();
    private Card firstCard;
    private Card secondCard;

    // Animation Graphs for speed values
    public AnimationCurve flyInSpeedCurve;
    public AnimationCurve flyOutSpeedCurve;
    public AnimationCurve cardHoverSpeedCurve;
    public AnimationCurve cardLowerSpeedCurve;

    // Vars for checking if cards need to be spawned and spawning them
    private bool cardsSpawned = false;
    private int tmpLevel = -1;
    private List<GameObject> spawnedCards = new List<GameObject>();

    // vars for making cards fly
    private int reachedBoardPlaceCount = 0;
    private bool reachedBoardPlace = false;
    private int reachedFlyPlaceCount = 0;
    private bool reachedFlyPlace = false;

    // Vars for level win condition
    private int matchedCount;
    private bool allCardsMatched = false;

    // hover distance var for designer
    public float hoverDistance = 2f;

    // Wrong Card Delay Timer for designer
    public float wrongCardDelay = 2f;

    // vars for determining if card needs to be flipped forward or back
    private bool firstCardFlippedFront;
    private bool secondCardFlippedFront;
    private bool firstCardFlippedBack;
    private bool secondCardFlippedBack;

    // var for flipping cards faster or slower
    public float cardFlipSpeed = 1000;

    // Option for designer to cause cards to spin when they fly out
    public bool spinWhenFlyOut;
    public float flyOutSpinSpeed = 2.5f;

    // Options for designer to decide how camera moves between each level
    public bool cameraPanToNextPoint;
    public bool cameraPanBeforeCardsSpawn;

    // Points on board where cards will fly to
    public List<GameObject> level1BoardPoints = new List<GameObject>();
    public List<GameObject> level2BoardPoints = new List<GameObject>();
    public List<GameObject> level3BoardPoints = new List<GameObject>();
    public List<GameObject> level4BoardPoints = new List<GameObject>();
    public List<GameObject> level5BoardPoints = new List<GameObject>();
    public List<GameObject> level6BoardPoints = new List<GameObject>();

    // Points off board where cards will fly from
    public List<GameObject> level1FlyPoints = new List<GameObject>();
    public List<GameObject> level2FlyPoints = new List<GameObject>();
    public List<GameObject> level3FlyPoints = new List<GameObject>();
    public List<GameObject> level4FlyPoints = new List<GameObject>();
    public List<GameObject> level5FlyPoints = new List<GameObject>();
    public List<GameObject> level6FlyPoints = new List<GameObject>();

    // Store board points and fly points lists as separate lists
    private List<List<GameObject>> levelBoardPoints = new List<List<GameObject>>();
    private List<List<GameObject>> levelFlyPoints = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        // Add lists to lists
        levelBoardPoints.Add(level1BoardPoints);
        levelBoardPoints.Add(level2BoardPoints);
        levelBoardPoints.Add(level3BoardPoints);
        levelBoardPoints.Add(level4BoardPoints);
        levelBoardPoints.Add(level5BoardPoints);
        levelBoardPoints.Add(level6BoardPoints);

        levelFlyPoints.Add(level1FlyPoints);
        levelFlyPoints.Add(level2FlyPoints);
        levelFlyPoints.Add(level3FlyPoints);
        levelFlyPoints.Add(level4FlyPoints);
        levelFlyPoints.Add(level5FlyPoints);
        levelFlyPoints.Add(level6FlyPoints);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if start button clicked
        if (gameObject.GetComponent<CameraMovement>().startClicked)
        {
            // Check to see if need to halt for win screen
            if (gameObject.GetComponent<CameraMovement>().level != 6 || (gameObject.GetComponent<CameraMovement>().continueClicked || PlayerPrefs.GetInt("MatchingGameProgress") > 5))
            {
                // Handle how cards are spanwed/deleted/move at start and end
                handleLevel();
                // Ensure cards can't be hovered or flipped while flying in
               if(reachedBoardPlace)
               {
                    // Check if two cards are a match
                    StartCoroutine(checkTwoCards());
                    // Make cards hover if you place your mouse over them, this stops when all cards are matched
                    if (!allCardsMatched)
                    {
                        handleHoveringCards();
                    }
               }  
            }
            else
            {
                // open win menu if check fail
                UIManager.GetComponent<MatchingUIManager>().openWinMenu();
            }
        }
    }

    // Checks to see if I'm hitting specific UI objects so that I can avoid clicking on objects behind them
    private bool isPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name == BackButton.name || result.gameObject.name == OpenLevelSelector.name || result.gameObject.name == StartMenu.name || result.gameObject.name == WinMenu.name || result.gameObject.name == LevelSelector.name
                || result.gameObject.name == ScoreText.name)
            {
                return true;
            }
        }
        return false;
    }

    // Handle how cards are flipped and if they are a match
    IEnumerator checkTwoCards()
    {
        //if (reachedBoardPlace)
        //{
            // if first card exists, determine if need to flip front or back
            if (firstCard != null)
            {
                if (firstCard.cardFlipped)
                {
                    firstCard.transform.rotation = Quaternion.RotateTowards(firstCard.transform.rotation, Quaternion.Euler(0, -180, 0), Time.deltaTime * cardFlipSpeed);
                    // Change sprite when it looks thinnest to camera
                    if (firstCard.transform.eulerAngles.y >= 80f && firstCard.transform.eulerAngles.y <= 110f)
                    {
                        firstCard.changeCardSpriteToFace();
                    }
                    // set bool true if card is flipped to its face
                    if (firstCard.transform.eulerAngles.y == 180)
                    {
                        firstCardFlippedFront = true;
                    }
                }
                else
                {
                    // Flip card to back after fail to match
                    if (!firstCard.cardFlipped)
                    {
                        firstCard.transform.rotation = Quaternion.RotateTowards(firstCard.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * cardFlipSpeed);
                        if (firstCard.transform.eulerAngles.y >= 80f && firstCard.transform.eulerAngles.y <= 110f)
                        {
                            firstCard.changeCardSpriteToBack();
                        }
                        // set bool true if card is flipped to its back
                        if (firstCard.transform.eulerAngles.y == 0)
                        {
                            firstCardFlippedBack = true;
                        }
                    }
                }
            }
            // See first card above, this works the same way
            if (secondCard != null)
            {
                if (secondCard.cardFlipped)
                {
                    secondCard.transform.rotation = Quaternion.RotateTowards(secondCard.transform.rotation, Quaternion.Euler(0, -180, 0), Time.deltaTime * cardFlipSpeed);
                    if (secondCard.transform.eulerAngles.y >= 80f && secondCard.transform.eulerAngles.y <= 110f)
                    {
                        secondCard.changeCardSpriteToFace();
                    }
                    if (secondCard.transform.eulerAngles.y == 180)
                    {
                        secondCardFlippedFront = true;
                    }
                }
                else
                {
                    if (!secondCard.cardFlipped)
                    {
                        secondCard.transform.rotation = Quaternion.RotateTowards(secondCard.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * cardFlipSpeed);
                        if (secondCard.transform.eulerAngles.y >= 80f && secondCard.transform.eulerAngles.y <= 110f)
                        {
                            secondCard.changeCardSpriteToBack();
                        }
                        if (secondCard.transform.eulerAngles.y == 0)
                        {
                            secondCardFlippedBack = true;
                        }
                    }
                }
            }
            // Check for left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                // Cast ray to where user clicks (GetRayIntersection is the only function I could find that defaultly works with Perspective Camera)
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                // check if an object with a collider was found
                if (hit.collider != null && !isPointerOverUIObject())
                {
                    // check if object has a Card component 
                    if (hit.collider.gameObject.GetComponent<Card>() != null)
                    {
                        // check to make sure card isn't already matched
                        if (hit.collider.gameObject.GetComponent<Card>().matched == false)
                        {
                            // if first card null, make card clicked first card and change sprite to face
                            if (firstCard == null)
                            {
                                firstCard = hit.collider.gameObject.GetComponent<Card>();
                                firstCard.cardFlipped = true;
                                // Reset values to prepare for next card flip
                                firstCardFlippedFront = false;
                                secondCardFlippedFront = false;
                                firstCardFlippedBack = false;
                                secondCardFlippedBack = false;
                            }
                            // if second card null, make card clicked second card and change sprite to face
                            else if (secondCard == null)
                            {
                                secondCard = hit.collider.gameObject.GetComponent<Card>();
                                // if second card == first card set back to null
                                if (secondCard != firstCard)
                                {
                                    secondCard.cardFlipped = true;
                                    // if first and second card are a match, change their values to matched, else, wait, flip them back, and set both to null
                                    if (firstCard.matchIntPair == secondCard.matchIntPair)
                                    {
                                        // Return null until both cards are finished flipping
                                        while (!firstCardFlippedFront || !secondCardFlippedFront)
                                        {
                                            yield return null;
                                        }
                                        firstCard.matched = true;
                                        secondCard.matched = true;
                                        // increment score if level == 6
                                        if (gameObject.GetComponent<CameraMovement>().level == 6)
                                        {
                                            score++;
                                        }
                                        // change score text
                                        ScoreText.GetComponent<Text>().text = "Score: " + score;
                                        // Check if all cards on board are matched, if so increment level
                                        for (int i = 0; i < spawnedCards.Count; i++)
                                        {
                                            if (spawnedCards[i].GetComponent<Card>().matched)
                                            {
                                                matchedCount++;
                                                if (matchedCount == spawnedCards.Count)
                                                {
                                                    allCardsMatched = true;
                                                }
                                            }
                                        }
                                        // reset matchedCount for next tally
                                        matchedCount = 0;
                                    }
                                    else
                                    {
                                        // wait so user can see that cards are not the same
                                        yield return new WaitForSeconds(wrongCardDelay);
                                        // flip cards back over
                                        firstCard.cardFlipped = false;
                                        secondCard.cardFlipped = false;
                                        // wait for both cards to be flipped
                                        while (!firstCardFlippedBack || !secondCardFlippedBack)
                                        {
                                            yield return null;
                                        }
                                    }
                                    // set both cards to null so another pair can be chosen
                                    firstCard = null;
                                    secondCard = null;
                                }
                                secondCard = null;
                            }
                        }
                    }
                }
            }
        //}
    }

    // handle hovering cards when mouse is over them
    private void handleHoveringCards()
    {
        //if (reachedBoardPlace)
        //{
            // Cast Ray from mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            // Loop through all spawned cards
            for (int i = 0; i < spawnedCards.Count; i++)
            {
                // check hitting object
                if (hit.collider != null && !isPointerOverUIObject())
                {
                    // check hitting card
                    if (hit.collider.gameObject.GetComponent<Card>())
                    {
                        // Use unique identifier cardNum to lower all other cards that are not being hovered
                        GameObject card = hit.collider.gameObject;
                        if (card.GetComponent<Card>().cardNum == i && !card.GetComponent<Card>().matched)
                        {
                            // We're using perspective camera so we need the direction hover to point towards the camera
                            Vector3 start = levelBoardPoints[gameObject.GetComponent<CameraMovement>().level - 1][i].transform.position;
                            Vector3 end = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                            // Get direction between end and start and specify how many units forward we want to move
                            Vector3 direction = end - start;
                            Vector3 newEnd = start + direction.normalized * hoverDistance;
                            // Check to make sure distance to next waypoint is not approximately 0
                            if (Vector3.Distance(start, newEnd) > 0.001f)
                            {
                                // Get distances
                                float totalDistance = Vector3.Distance(start, newEnd);
                                float currentDistance = Vector3.Distance(spawnedCards[i].transform.position, newEnd);

                                // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                                float speedBasedOnPosition = currentDistance / totalDistance;

                                // Get Y value on animation graph for speed. 1f - cardHoverSpeedCurve ensures graph is read from left to right for designers
                                float currentSpeed = cardHoverSpeedCurve.Evaluate(1f - speedBasedOnPosition);

                                // MoveTowards next waypoint at currentSpeed
                                spawnedCards[i].transform.position = Vector3.MoveTowards(spawnedCards[i].transform.position, newEnd, Time.deltaTime * currentSpeed);
                            }
                        }
                        else
                        {
                            // Move from camera to board point
                            Vector3 start = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                            Vector3 end = levelBoardPoints[gameObject.GetComponent<CameraMovement>().level - 1][i].transform.position;
                            // Check to make sure distance to next waypoint is not approximately 0
                            if (Vector3.Distance(start, end) > 0.001f)
                            {
                                // Get distances
                                float totalDistance = Vector3.Distance(start, end);
                                float currentDistance = Vector3.Distance(spawnedCards[i].transform.position, end);

                                // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                                float speedBasedOnPosition = currentDistance / totalDistance;

                                // Get Y value on animation graph for speed. 1f - cardLowerSpeedCurve ensures graph is read from left to right for designers
                                float currentSpeed = cardLowerSpeedCurve.Evaluate(1f - speedBasedOnPosition);

                                // MoveTowards next waypoint at currentSpeed
                                spawnedCards[i].transform.position = Vector3.MoveTowards(spawnedCards[i].transform.position, end, Time.deltaTime * currentSpeed);
                            }
                        }
                    }
                    // Lower all cards if card component not found on object
                    else
                    {
                        // Move from camera to board point
                        Vector3 start = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                        Vector3 end = levelBoardPoints[gameObject.GetComponent<CameraMovement>().level - 1][i].transform.position;
                        // Check to make sure distance to next waypoint is not approximately 0
                        if (Vector3.Distance(start, end) > 0.001f)
                        {
                            // Get distances
                            float totalDistance = Vector3.Distance(start, end);
                            float currentDistance = Vector3.Distance(spawnedCards[i].transform.position, end);

                            // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                            float speedBasedOnPosition = currentDistance / totalDistance;

                            // Get Y value on animation graph for speed. 1f - cardLowerSpeedCurve ensures graph is read from left to right for designers
                            float currentSpeed = cardLowerSpeedCurve.Evaluate(1f - speedBasedOnPosition);

                            // MoveTowards next waypoint at currentSpeed
                            spawnedCards[i].transform.position = Vector3.MoveTowards(spawnedCards[i].transform.position, end, Time.deltaTime * currentSpeed);
                        }
                    }
                }
                // Lower all cards if hit.colider == null
                else
                {
                    Vector3 start = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                    Vector3 end = levelBoardPoints[gameObject.GetComponent<CameraMovement>().level - 1][i].transform.position;
                    // Check to make sure distance to next waypoint is not approximately 0
                    if (Vector3.Distance(start, end) > 0.001f)
                    {
                        // Get distances
                        float totalDistance = Vector3.Distance(start, end);
                        float currentDistance = Vector3.Distance(spawnedCards[i].transform.position, end);

                        // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                        float speedBasedOnPosition = currentDistance / totalDistance;

                        // Get Y value on animation graph for speed. 1f - cardLowerSpeedCurve ensures graph is read from left to right for designers
                        float currentSpeed = cardLowerSpeedCurve.Evaluate(1f - speedBasedOnPosition);

                        // MoveTowards next waypoint at currentSpeed
                        spawnedCards[i].transform.position = Vector3.MoveTowards(spawnedCards[i].transform.position, end, Time.deltaTime * currentSpeed);
                    }
                }
            }
        //}
    }
    
    private void handleLevel()
    {
        // check if need to be destroyed
        destroyCards();
        // spawn cards if bool true
        if (gameObject.GetComponent<CameraMovement>().cameraReachedPosition)
        {
            spawnCards(levelFlyPoints[gameObject.GetComponent<CameraMovement>().level - 1]);
        }
        // fly in cards from positions specified by level
        flyCardsIn(levelBoardPoints[gameObject.GetComponent<CameraMovement>().level - 1], levelFlyPoints[gameObject.GetComponent<CameraMovement>().level - 1]);
        // fly out cards when all cards have been matched
        if (allCardsMatched)
        {
            flyCardsOut(levelBoardPoints[gameObject.GetComponent<CameraMovement>().level - 1], levelFlyPoints[gameObject.GetComponent<CameraMovement>().level - 1]);
        }
        // check if all cards have reached their fly points
        if (reachedFlyPlace)
        {
            // increment level
            gameObject.GetComponent<CameraMovement>().level++;
            // pan camera before next set of cards spawn if designer chooses
            if (cameraPanBeforeCardsSpawn)
            {
                gameObject.GetComponent<CameraMovement>().cameraReachedPosition = false;
            }
        }
    }

    // Brings cards from points off screen to points on screen using flyInSpeedCurve animation graph to get smooth speed and movement
    private void flyCardsIn(List<GameObject> levelBoardPoints, List<GameObject> levelFlyPoints)
    {
        // Check if cards have not reached destinationss
        if (!reachedBoardPlace)
        {
            // Loop through currently spawned cards
            for (int i = 0; i < spawnedCards.Count; i++)
            {
                // Check to make sure distance to next waypoint is not approximately 0
                if (Vector3.Distance(spawnedCards[i].transform.position, levelBoardPoints[i].transform.position) > 0.001f)
                {
                    // Get distances
                    float totalDistance = Vector3.Distance(levelFlyPoints[i].transform.position, levelBoardPoints[i].transform.position);
                    float currentDistance = Vector3.Distance(spawnedCards[i].transform.position, levelBoardPoints[i].transform.position);
                    
                    // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                    float speedBasedOnPosition = currentDistance / totalDistance;
                    
                    // Get Y value on animation graph for speed. 1f - speedBasedOnPosition ensures graph is read from left to right for designers
                    float currentSpeed = flyInSpeedCurve.Evaluate(1f - speedBasedOnPosition);
                    
                    // MoveTowards next waypoint at currentSpeed
                    spawnedCards[i].transform.position = Vector3.MoveTowards(spawnedCards[i].transform.position, levelBoardPoints[i].transform.position, Time.deltaTime * currentSpeed);

                    // Check if distance to waypoint is approximately 0 (waypoint reached) and increment reachedBoardPlaceCount if true
                    if (Vector3.Distance(spawnedCards[i].transform.position, levelBoardPoints[i].transform.position) <= 0.001f)
                    {
                        reachedBoardPlaceCount++;
                        
                        // if reachedBoardPlaceCount is equal to amount of spawned cards then all cards have reached destination
                        if (reachedBoardPlaceCount == spawnedCards.Count)
                        {
                            reachedBoardPlace = true;
                        }
                    }
                }
            }
        }
    }

    // Brings cards from points on screen to points off screen using flyOutSpeedCurve animation graph to get smooth speed and movement
    private void flyCardsOut(List<GameObject> levelBoardPoints, List<GameObject> levelFlyPoints)
    {
        // Check if cards have not reached destinationss
        if (!reachedFlyPlace)
        {
            // Loop through currently spawned cards
            for (int i = 0; i < spawnedCards.Count; i++)
            {
                // Check to make sure distance to next waypoint is not approximately 0
                if (Vector3.Distance(levelFlyPoints[i].transform.position, spawnedCards[i].transform.position) > 0.001f)
                {
                    // Get distances
                    float totalDistance = Vector3.Distance(levelBoardPoints[i].transform.position, levelFlyPoints[i].transform.position);
                    float currentDistance = Vector3.Distance(levelBoardPoints[i].transform.position, spawnedCards[i].transform.position);

                    // Convert distance to 0.0 through 1.0 to compare to X value on animation graph 
                    float speedBasedOnPosition = currentDistance / totalDistance;

                    // Get Y value on animation graph for speed. 1f - speedBasedOnPosition ensures graph is read from left to right for designers
                    float currentSpeed = flyOutSpeedCurve.Evaluate(1f - speedBasedOnPosition);
                    
                    // Option to make cards spin when flying off screen
                    if (spinWhenFlyOut)
                    {
                        spawnedCards[i].transform.Rotate(0, 0, flyOutSpinSpeed);
                    }
                    // MoveTowards next waypoint at currentSpeed
                    spawnedCards[i].transform.position = Vector3.MoveTowards(spawnedCards[i].transform.position, levelFlyPoints[i].transform.position, Time.deltaTime * currentSpeed);

                    // Check if distance to waypoint is approximately 0 (waypoint reached) and increment reachedFlyPlaceCount if true
                    if (Vector3.Distance(spawnedCards[i].transform.position, levelFlyPoints[i].transform.position) <= 0.001f)
                    {
                        reachedFlyPlaceCount++;

                        // if reachedBoardPlaceCount is equal to amount of spawned cards then all cards have reached destination
                        if (reachedFlyPlaceCount == spawnedCards.Count)
                        {
                            reachedFlyPlace = true;
                        }
                    }
                }
            }
        }
    }

    // Spawn cards at placeholder points
    private void spawnCards(List<GameObject> levelCards)
    {
        // Check if cards have not been spawned
        if (!cardsSpawned)
        {
            // Loop through the amount of cards that the level is supposed to have
            for (int i = 0; i < levelCards.Count; i++)
            {
                // Spawn object at placeholder point and add to array to be looped through deleted later
                Transform spawnPosition = levelCards[i].transform;
                GameObject card = Instantiate(defaultCard, spawnPosition);
                card.GetComponent<Card>().matchIntPair = spawnOrder[i];
                card.GetComponent<Card>().cardNum = i;
                spawnedCards.Add(card);
            }
            cardsSpawned = true;
        }
    }

    // Destroy all existing cards
    private void destroyCards()
    {
        // Check if level has changed
        if (tmpLevel != gameObject.GetComponent<CameraMovement>().level)
        {
            // Destroy objects and clear array after all have been deleted
            foreach (GameObject card in spawnedCards)
            {
                Destroy(card);
            }
            spawnedCards.Clear();
            // Set level back to 6 if it passes 7
            if (gameObject.GetComponent<CameraMovement>().level == 7)
            {
                gameObject.GetComponent<CameraMovement>().level = 6;
            }
            // Set player progress based on if they completed a new level
            if (gameObject.GetComponent<CameraMovement>().level >= PlayerPrefs.GetInt("MatchingGameProgress"))
            {
                PlayerPrefs.SetInt("MatchingGameProgress", gameObject.GetComponent<CameraMovement>().level);
            }
            // display score text if on level 6, else set inactive
            if (gameObject.GetComponent<CameraMovement>().level == 6)
            {
                ScoreText.SetActive(true);
            }
            else
            {
                ScoreText.SetActive(false);
            }
            // set tmpLevel to current level and reset values
            tmpLevel = gameObject.GetComponent<CameraMovement>().level;
            cardsSpawned = false;
            reachedBoardPlace = false;
            reachedBoardPlaceCount = 0;
            reachedFlyPlace = false;
            reachedFlyPlaceCount = 0;
            allCardsMatched = false;
            matchedCount = 0;
            // Shuffle spawnOrder to make random card pairs
            shuffleCards();
        }
    }

    // Shuffle how cards spawn in so that levels are different each time
    private void shuffleCards()
    {
        // clear array on new level
        spawnOrder.Clear();
        // add 2 of the same value into the array to ensure each value has a pair
        for (int i = 0; i <= gameObject.GetComponent<CameraMovement>().level; i++)
        {
            spawnOrder.Add(i);
            spawnOrder.Add(i);
        }
        // Randomize array order
        for (int i = spawnOrder.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = spawnOrder[i];
            spawnOrder[i] = spawnOrder[j];
            spawnOrder[j] = tmp;
        }
    }
}
