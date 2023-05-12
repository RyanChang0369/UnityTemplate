using System.Collections;
using UnityEngine;

public static class DebugExt
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

    #region Cross Square
    /// <summary>
    /// Draws a crossed square.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="rotation">Rotation of the drawn item.</param>
    /// <param name="crossColor">Color of cross.</param>
    /// <param name="boxColor">Color of outer box.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossSquare(Vector3 position, Quaternion rotation,
        Color crossColor, Color boxColor, float duration, float size)
    {
        float half = size / 2;

        // Define corners
        Vector3[] corners = {
            // First face
            new Vector3(half, half, 0),
            new Vector3(-half, half, 0),
            new Vector3(-half, -half, 0),
            new Vector3(half, -half, 0),
            new Vector3(half, half, 0),
        };

        // Apply rotation, then transformation.
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = rotation * corners[i] + position;
        }

        // Draw outer box
        for (int i = 1; i < corners.Length; i++)
        {
            Debug.DrawLine(corners[i - 1], corners[i], boxColor, duration);
        }
        Debug.DrawLine(corners[0], corners[corners.Length - 1], boxColor,
            duration);

        // Draw cross
        Debug.DrawLine(corners[0], corners[2], crossColor, duration);
        Debug.DrawLine(corners[1], corners[3], crossColor, duration);
    }

    /// <summary>
    /// Draws a crossed square.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="crossColor">Color of cross.</param>
    /// <param name="boxColor">Color of outer box.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossSquare(Vector3 position, Color crossColor,
        Color boxColor, float duration, float size)
    {
        DrawCrossSquare(position, Quaternion.identity, crossColor, boxColor,
            duration, size);
    }

    /// <summary>
    /// Draws a crossed square.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="rotation">Rotation of the drawn item.</param>
    /// <param name="color">Color of shape.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossSquare(Vector3 position, Quaternion rotation,
        Color color, float duration, float size)
    {
        DrawCrossSquare(position, rotation, color, color, duration, size);
    }

    /// <summary>
    /// Draws a crossed square.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="color">Color of shape.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossSquare(Vector3 position, Color color,
        float duration, float size)
    {
        DrawCrossSquare(position, color, color, duration, size);
    }
    #endregion

    #region Cross Cube
    /// <summary>
    /// Draws a crossed cube.
    /// </summary>
    /// <param name="position">Center of the cross, at world position.</param>
    /// <param name="rotation">Rotation of the drawn item.</param>
    /// <param name="crossColor">Color of the inner x.</param>
    /// <param name="boxColor">Color of the outer box.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Length of the cube.</param>
    public static void DrawCrossCube(Vector3 position, Quaternion rotation,
        Color crossColor, Color boxColor, float duration, float size)
    {
        float half = size / 2;

        // Define corners
        Vector3[] corners = {
            // First face
            new Vector3(half, half, half),
            new Vector3(-half, half, half),
            new Vector3(-half, -half, half),
            new Vector3(half, -half, half),
            new Vector3(half, half, half),
            //Cross over to other face
            new Vector3(half, half, -half),
            new Vector3(-half, half, -half),
            new Vector3(-half, -half, -half),
            new Vector3(half, -half, -half),
            new Vector3(half, half, -half),
        };

        // Apply rotation, then transformation.
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = rotation * corners[i] + position;
        }

        // Draw outer box
        for (int i = 1; i < corners.Length; i++)
        {
            Debug.DrawLine(corners[i - 1], corners[i], boxColor, duration);
        }
        Debug.DrawLine(corners[0], corners[corners.Length - 1], boxColor,
            duration);

        // Other corners
        Debug.DrawLine(corners[1], corners[6], boxColor, duration);
        Debug.DrawLine(corners[2], corners[7], boxColor, duration);
        Debug.DrawLine(corners[3], corners[8], boxColor, duration);

        // Draw cross
        Debug.DrawLine(corners[0], corners[7], crossColor, duration);
        Debug.DrawLine(corners[1], corners[8], crossColor, duration);
        Debug.DrawLine(corners[2], corners[9], crossColor, duration);
        Debug.DrawLine(corners[3], corners[6], crossColor, duration);
    }

    /// <summary>
    /// Draws a crossed cube.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="crossColor">Color of cross.</param>
    /// <param name="boxColor">Color of outer box.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossCube(Vector3 position, Color crossColor,
        Color boxColor, float duration, float size)
    {
        DrawCrossCube(position, Quaternion.identity, crossColor, boxColor,
            duration, size);
    }

    /// <summary>
    /// Draws a crossed cube.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="rotation">Rotation of the drawn item.</param>
    /// <param name="color">Color of shape.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossCube(Vector3 position, Quaternion rotation,
        Color color, float duration, float size)
    {
        DrawCrossCube(position, rotation, color, color, duration, size);
    }

    /// <summary>
    /// Draws a crossed cube.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="color">Color of shape.</param>
    /// <param name="duration">How long to show the drawn item.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossCube(Vector3 position, Color color,
        float duration, float size)
    {
        DrawCrossCube(position, color, color, duration, size);
    }
    #endregion
}