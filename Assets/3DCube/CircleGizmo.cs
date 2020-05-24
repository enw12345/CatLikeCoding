using UnityEngine;
using System.Collections;
/// <summary>
/// A visualizer for the mapping from square to circle
/// </summary>
public class CircleGizmo : MonoBehaviour
{
    public int resolution = 50;

    private void OnDrawGizmos()
    {
        float step = 2f / resolution;
        //Show the vertices of the top and bottom edges
        for(int i = 0; i <= resolution; i++)
        {
            ShowPoint(i * step - 1f, -1f);
            ShowPoint(i * step - 1f, 1f);
        }

        //Show the vertices of the left and right edges
        for (int i = 1; i < resolution; i++)
        {
            ShowPoint(-1f, i * step - 1f);
            ShowPoint(1f, i * step - 1f);
        }
    }

    private void ShowPoint(float x, float y)
    {
        Vector2 square = new Vector2(x, y);

        Vector2 circle;
        circle.x = square.x * Mathf.Sqrt(1f - square.y * square.y * 0.5f);
        circle.y = square.y * Mathf.Sqrt(1f - square.x * square.x * 0.5f);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(square, 0.025f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(square, circle);

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(circle, Vector2.zero);
    }
}
