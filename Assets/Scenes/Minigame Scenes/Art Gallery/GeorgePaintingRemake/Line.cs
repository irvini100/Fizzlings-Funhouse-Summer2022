using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    // Adapted from https://www.youtube.com/watch?v=D0ENg1dQN64
    // get lineRenderer component
    public LineRenderer lineRenderer;
    // Create a list of Vector2s
    private List<Vector2> points;

    // Determine when and how to add new points
    public void updateLine(Vector2 position)
    {
        // if no points exist, set the passed position as the first point
        if (points == null)
        {
            points = new List<Vector2>();
            setPoint(position);
        }
        // else if distance between last point and the passed position is greater than desired distance, set passed position as new point
        else if (Vector2.Distance(points[points.Count - 1], position) > .1f)
        {
            setPoint(position);
        }
    }

    // adds a point to the list of points, then sets the line renderer position count equal to points count and sets the position to the newest point
    void setPoint(Vector2 point)
    {
        points.Add(point);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }
}
