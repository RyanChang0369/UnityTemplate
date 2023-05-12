using UnityEngine;

/// <summary>
/// Contains methods pertaining to Vector3 and Vector2.
/// </summary>
public static class VectorExt
{
    #region Conversion
    /// <summary>
    /// Converts from a Vector2, v2, to a Vector3, v3, such that
    /// v3.x == v2.x and v3.z == v2.y and v3.y == 0
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 ToVector3(this Vector2 v)
    {
        return new(v.x, 0, v.y);
    }

    /// <summary>
    /// Converts from a Vector3, v3, to a Vector2, v2, such that
    /// v2.x == v3.x and v2.y == v3.z
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2 ToVector2(this Vector3 v)
    {
        return new(v.x, v.z);
    }
    #endregion

    #region Comparison
    /// <summary>
    /// Returns true if a differs from b by at most margin. Otherwise, returns
    /// false.
    /// </summary>
    /// <param name="a">First vector.</param>
    /// <param name="b">Second vector.</param>
    /// <param name="margin">How much can a differ from b (component wise).</param>
    /// <returns></returns>
    public static bool Approx(this Vector3 a, Vector3 b, float margin)
    {
        return a.x.Approx(b.x, margin) && a.y.Approx(b.y, margin)
            && a.z.Approx(b.z, margin);
    }

    /// <summary>
    /// Returns true if a is approximately b. Otherwise, returns false.
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>True if a is approximately b, false otherwise</returns>
    public static bool Approx(this Vector3 a, Vector3 b)
    {
        return a.x.Approx(b.x) && a.y.Approx(b.y) && a.z.Approx(b.z);
    }

    /// <summary>
    /// Returns true if a is approximately zero. Otherwise, returns false.
    /// </summary>
    /// <param name="a">Vector</param>
    /// <returns>True if a is approximately zero, false otherwise.</returns>
    public static bool ApproxZero(this Vector3 a)
    {
        return a.Approx(Vector3.zero);
    }

    /// <summary>
    /// Returns true if a differs from b by at most margin. Otherwise, returns
    /// false.
    /// </summary>
    /// <param name="a">First vector.</param>
    /// <param name="b">Second vector.</param>
    /// <param name="margin">How much can a differ from b (component wise).</param>
    /// <returns></returns>
    public static bool Approx(this Vector2 a, Vector2 b, float margin)
    {
        return a.x.Approx(b.x, margin) && a.y.Approx(b.y, margin);
    }

    /// <summary>
    /// Returns true if a is approximately b. Otherwise, returns false.
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>True if a is approximately b, false otherwise</returns>
    public static bool Approx(this Vector2 a, Vector2 b)
    {
        return a.x.Approx(b.x) && a.y.Approx(b.y);
    }

    /// <summary>
    /// Returns true if a is approximately zero. Otherwise, returns false.
    /// </summary>
    /// <param name="a">Vector</param>
    /// <returns>True if a is approximately zero, false otherwise.</returns>
    public static bool ApproxZero(this Vector2 a)
    {
        return a.Approx(Vector2.zero);
    }
    #endregion

    #region Other Operations
    /// <summary>
    /// Rotates a vector by theta degrees
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="theta">In degrees</param>
    /// <returns></returns>
    public static Vector2 RotateVector2(this Vector2 vec, float theta)
    {
        float sin = Mathf.Sin(theta * Mathf.Deg2Rad);
        float cos = Mathf.Cos(theta * Mathf.Deg2Rad);

        float oldX = vec.x, oldY = vec.y;

        return new Vector2(cos * oldX - sin * oldY, sin * oldX + cos * oldY);
    }
    #endregion
}