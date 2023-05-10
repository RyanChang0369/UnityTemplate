using System.Collections;
using UnityEngine;

public static class DebugExtensions
{
    /// <summary>
    /// Draws a crosshair at position.
    /// </summary>
    /// <param name="position">Where to draw the crosshair in world position.</param>
    /// <param name="color">Color of the crosshair.</param>
    /// <param name="size">Diameter of the crosshair.</param>
    public static void DrawCrosshair(Vector3 position, Color color, float size = 4)
    {
        float half = size / 2;
        Debug.DrawLine(position + Vector3.down * half, position + Vector3.up * half, color);
        Debug.DrawLine(position + Vector3.left * half, position + Vector3.right * half, color);
    }

    /// <summary>
    /// Draws an X within a box at position.
    /// </summary>
    /// <param name="position">Center of the cross, at world position.</param>
    /// <param name="crossColor">Color of the inner x.</param>
    /// <param name="boxColor">Color of the outer box.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Diameter of cross.</param>
    public static void DrawCrossCube(Vector3 position, Color crossColor, Color boxColor, float duration, float size = 4)
    {
        float half = size / 2;

        Vector3[] corners = {
            // First face
            position + new Vector3(half, half, half),
            position + new Vector3(-half, half, half),
            position + new Vector3(-half, -half, half),
            position + new Vector3(half, -half, half),
            position + new Vector3(half, half, half),
            //Cross over to other face
            position + new Vector3(half, half, -half),
            position + new Vector3(-half, half, -half),
            position + new Vector3(-half, -half, -half),
            position + new Vector3(half, -half, -half),
            position + new Vector3(half, half, -half),
        };
        
        // Draw outer box
        for (int i = 1; i < corners.Length; i++)
        {
            Debug.DrawLine(corners[i - 1], corners[i], boxColor, duration);
        }
        Debug.DrawLine(corners[0], corners[corners.Length - 1], boxColor, duration);

        // Other corners
        Debug.DrawLine(corners[1], corners[6], crossColor, duration);
        Debug.DrawLine(corners[2], corners[7], crossColor, duration);
        Debug.DrawLine(corners[3], corners[8], crossColor, duration);

        // Draw cross
        Debug.DrawLine(corners[0], corners[7], crossColor, duration);
        Debug.DrawLine(corners[1], corners[8], crossColor, duration);
        Debug.DrawLine(corners[2], corners[9], crossColor, duration);
        Debug.DrawLine(corners[3], corners[6], crossColor, duration);
    }

    /// <summary>
    /// Draws an X within a box at position.
    /// </summary>
    /// <param name="position">Center of the cross, at world position.</param>
    /// <param name="crossColor">Color of the inner x.</param>
    /// <param name="boxColor">Color of the outer box.</param>
    /// <param name="size">Diameter of cross.</param>
    public static void DrawCrossCube(Vector3 position, Color crossColor, Color boxColor, float size = 4)
    {
        DrawCrossCube(position, crossColor, boxColor, Time.deltaTime, size);
    }

    /// <summary>
    /// Draws an X within a box at position.
    /// </summary>
    /// <param name="position">Center of the cross, at world position.</param>
    /// <param name="color">Color of the cross and box.</param>
    /// <param name="size">Diameter of cross.</param>
    public static void DrawCrossCube(Vector3 position, Color color, float size = 4)
    {
        DrawCrossCube(position, color, color, size);
    }
}