using UnityEngine;

/// <summary>
/// Contains methods pertaining to Unity rectangles.
/// </summary>
public static class RectExt
{
    #region Modifications
    /// <summary>
    /// Translates the rectangle by amount. Modifies the rectangle in place.
    /// </summary>
    /// <param name="rect">Rectangle to translate.</param>
    /// <param name="amount">Amount to translate by.</param>
    /// <returns></returns>
    public static void Translate(this ref Rect rect, Vector2 amount)
    {
        Vector2 center = rect.center + amount;
        rect.center = center;
    }
    #endregion
}