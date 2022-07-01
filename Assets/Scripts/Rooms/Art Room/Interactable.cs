using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public List<Sprite> sprites;
    bool isHovered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getIsHovered()
    {
        return isHovered;
    }

    public void setIsHovered(bool isHovered)
    {
        this.isHovered = isHovered;
    }
}
