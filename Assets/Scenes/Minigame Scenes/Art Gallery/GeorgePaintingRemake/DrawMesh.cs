using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMesh : MonoBehaviour
{
    private Mesh mesh;

    private Vector3 mousePosition;
    private Vector3 lastMousePosition;

    private void Update()
    {
        // Get mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Set z index to zero, this basically converts to Vector2 but allows us to use Vector3 math
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

        // On first frame of mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Move gameObject to mousePosition
            transform.position = mousePosition;

            // Create new quad
            mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            // Create vertices
            //vertices[0] = new Vector3(-1, +1);
            //vertices[1] = new Vector3(-1, -1);
            //vertices[2] = new Vector3(+1, -1);
            //vertices[3] = new Vector3(+1, +1);

            // This makes the line start thinner
            vertices[0] = mousePosition;
            vertices[1] = mousePosition;
            vertices[2] = mousePosition;
            vertices[3] = mousePosition;

            // Create UVs
            uv[0] = Vector2.zero;
            uv[1] = Vector2.zero;
            uv[2] = Vector2.zero;
            uv[3] = Vector2.zero;

            // Create 1st triangle
            triangles[0] = 0;
            triangles[1] = 3;
            triangles[2] = 1;

            // Create 2nd triangle
            triangles[3] = 1;
            triangles[4] = 3;
            triangles[5] = 2;

            // apply arrays to create mesh
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            // This mesh will change so we add markDynamic to help with performance
            mesh.MarkDynamic();

            // Assign new mesh to the mesh filter to display on screen
            GetComponent<MeshFilter>().mesh = mesh;

            // Store mouse position so that we can get the beginning of the new line
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Set z index to zero, this basically converts to Vector2 but allows us to use Vector3 math
            lastMousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        }

        // On all frames mouse is held down
        if (Input.GetMouseButton(0))
        {
            // Add necessary vertices UVs and triangles to make another polygon
            Vector3[] vertices = new Vector3[mesh.vertices.Length + 2];
            Vector2[] uv = new Vector2[mesh.uv.Length + 2];
            int[] triangles = new int[mesh.triangles.Length + 6];

            // copy existing values into new ones, from here we only need to define the new polygon
            mesh.vertices.CopyTo(vertices, 0);
            mesh.uv.CopyTo(uv, 0);
            mesh.triangles.CopyTo(triangles, 0);

            // This puts us at the beginning vertex of the new polygon
            int vIndex = vertices.Length - 4;
            // Create indexes for each new vertex, the first two are shard by the previous polygon
            int vIndex0 = vIndex + 0;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            // Get the direction that we're moving the mouse in (current mouse position - start mouse position)
            Vector3 mouseForwardVector = (mousePosition - lastMousePosition).normalized;
            Vector3 normal2D = new Vector3(0, 0, -1f);
            float lineThickness = 1f;
            Vector3 newVertexUp = mousePosition + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
            Vector3 newVertexDown = mousePosition + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;

            vertices[vIndex2] = newVertexUp;
            vertices[vIndex3] = newVertexDown;

            uv[vIndex2] = Vector2.zero;
            uv[vIndex3] = Vector2.zero;

            int tIndex = triangles.Length - 6;

            triangles[tIndex + 0] = vIndex0;
            triangles[tIndex + 1] = vIndex2;
            triangles[tIndex + 2] = vIndex1;

            triangles[tIndex + 3] = vIndex1;
            triangles[tIndex + 4] = vIndex2;
            triangles[tIndex + 5] = vIndex3;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            // Store mouse position so that we can get the beginning of the new line
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Set z index to zero, this basically converts to Vector2 but allows us to use Vector3 math
            lastMousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }
    //[SerializeField] private Transform debugVisual1;
    //[SerializeField] private Transform debugVisual2;

    //private Mesh mesh;
    //private Vector3 lastMousePosition;

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        mesh = new Mesh();
    //        Vector3[] vertices = new Vector3[4];
    //        Vector2[] uv = new Vector2[4];
    //        int[] triangles = new int[6];

    //        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //        vertices[0] = mousePosition;
    //        vertices[1] = mousePosition;
    //        vertices[2] = mousePosition;
    //        vertices[3] = mousePosition;

    //        uv[0] = Vector2.zero;
    //        uv[1] = Vector2.zero;
    //        uv[2] = Vector2.zero;
    //        uv[3] = Vector2.zero;

    //        triangles[0] = 0;
    //        triangles[1] = 3;
    //        triangles[2] = 1;

    //        triangles[3] = 1;
    //        triangles[4] = 3;
    //        triangles[5] = 2;

    //        mesh.vertices = vertices;
    //        mesh.uv = uv;
    //        mesh.triangles = triangles;
    //        mesh.MarkDynamic();

    //        GetComponent<MeshFilter>().mesh = mesh;

    //        lastMousePosition = mousePosition;
    //    }

    //    if (Input.GetMouseButton(0))
    //    {
    //        Vector3[] vertices = new Vector3[mesh.vertices.Length + 2];
    //        Vector2[] uv = new Vector2[mesh.uv.Length + 2];
    //        int[] triangles = new int[mesh.triangles.Length + 6];

    //        mesh.vertices.CopyTo(vertices, 0);
    //        mesh.uv.CopyTo(triangles, 0);
    //        mesh.triangles.CopyTo(triangles, 0);

    //        int vIndex = vertices.Length - 4;
    //        int vIndex0 = vIndex + 0;
    //        int vIndex1 = vIndex + 1;
    //        int vIndex2 = vIndex + 2;
    //        int vIndex3 = vIndex + 3;

    //        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector3 mouseForwardVector = (mousePosition - lastMousePosition).normalized;
    //        Vector3 normal2D = new Vector3(0, 0, -1f);
    //        float lineThickness = 1f;
    //        Vector2 newVertexUp = mousePosition + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
    //        Vector2 newVertexDown = mousePosition + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;

    //        vertices[vIndex2] = newVertexUp;
    //        vertices[vIndex3] = newVertexDown;

    //        uv[vIndex2] = Vector2.zero;
    //        uv[vIndex3] = Vector2.zero;

    //        int tIndex = triangles.Length - 6;

    //        triangles[tIndex + 0] = vIndex0;
    //        triangles[tIndex + 1] = vIndex2;
    //        triangles[tIndex + 2] = vIndex1;

    //        triangles[tIndex + 3] = vIndex1;
    //        triangles[tIndex + 4] = vIndex2;
    //        triangles[tIndex + 5] = vIndex3;

    //        mesh.vertices = vertices;
    //        mesh.uv = uv;
    //        mesh.triangles = triangles;

    //        lastMousePosition = mousePosition;
    //    }
    //}
}
