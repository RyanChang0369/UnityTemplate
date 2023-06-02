using UnityEngine;

/// <summary>
/// Contains methods pertaining to transforms.
/// </summary>
public static class TransformExt
{
    /// <summary>
    /// Returns true if a and b have the same root
    /// </summary>
    /// <param name="a">A</param>
    /// <param name="b">B</param>
    /// <returns>True if a and b have the same root, false otherwise.</returns>
    public static bool HasSameRoot(this Transform a, Transform b)
    {
        return a.root == b.root;
    }

    /// <summary>
    /// Returns true if a and b have different roots
    /// </summary>
    /// <param name="a">A</param>
    /// <param name="b">B</param>
    /// <returns>False if a and b have the same root, true otherwise.</returns>
    public static bool HasDifferentRoot(this Transform a, Transform b)
    {
        return !HasSameRoot(a, b);
    }

    /// <summary>
    /// Orphans this transform, setting the parent to null while preserving
    /// world position, rotation, and scale.
    /// </summary>
    /// <param name="transform">The transform to orphan.</param>
    public static void Orphan(this Transform transform)
    {
        transform.SetParent(null, true);
    }
}