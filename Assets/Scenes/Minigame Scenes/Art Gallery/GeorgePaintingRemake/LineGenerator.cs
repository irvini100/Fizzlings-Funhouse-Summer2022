using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    // Adapted from https://www.youtube.com/watch?v=D0ENg1dQN64
    // Get reference to line prefab
    public GameObject linePrefab;
    // var for getting the current line
    private Line activeLine;
    // List of lines created so that they can be cleared later
    private List<GameObject> lines = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        // on mouse click down instantiate new line and add it to lines list, set as active line
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLine = Instantiate(linePrefab);
            lines.Add(newLine);
            activeLine = newLine.GetComponent<Line>();
        }

        // on mouse up set active line to null
        if (Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }

        // if active line not null (mouse is down) draw in direction of mouse
        if (activeLine != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.updateLine(mousePosition);
        }

        // George's cheatcode for adjusting width, setting color, and clearing lines
        if (Input.GetKeyDown(KeyCode.K))
        {
            linePrefab.GetComponent<LineRenderer>().startWidth = 0.2f;
            linePrefab.GetComponent<LineRenderer>().endWidth = 0.2f;
            linePrefab.GetComponent<LineRenderer>().sharedMaterial.SetColor("_Color", new Color(0, 1, 0, 1));
            foreach (GameObject line in lines)
            {
                Destroy(line);
            }
            lines.Clear();
        }
    }
}
