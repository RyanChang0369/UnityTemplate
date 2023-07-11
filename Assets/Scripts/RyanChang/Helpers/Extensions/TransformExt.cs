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

    /// <summary>
    /// Returns the distance between the two transforms.
    /// </summary>
    /// <param name="a">The first transform.</param>
    /// <param name="b">The second transform.</param>
    /// <returns>The distance between the two transforms.</returns>
    public static float Distance(this Transform a, Transform b)
    {
        return Vector3.Distance(a.position, b.position);
    }

    /// <summary>
    /// Returns the distance between this transform and a point.
    /// </summary>
    /// <param name="transform">The transform.</param>
    /// <param name="point">The point.</param>
    /// <returns>The distance between transform and point.</returns>
    public static float Distance(this Transform transform, Vector3 point)
    {
        return Vector3.Distance(transform.position, point);
    }

    /// <summary>
    /// Returns the center of the all the transforms.
    /// </summary>
    /// <param name="transforms">List of transforms.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">If passed empty list of
    /// params.</exception>
    public static Vector3 Center(params Transform[] transforms)
    {
        if (transforms.Length > 0)
        {
            Vector3 sum = new();

            foreach (var t in transforms) sum += t.position;

            return sum / transforms.Length;
        }
        else
        {
            throw new System.ArgumentException("Cannot provide empty params");
        }
    }

    /// <summary>
    /// Destroys all children in <paramref name="transform"/>.
    /// </summary>
    /// <param name="transform">The transform whose children we destroy.</param>
    public static void DestroyAllChildren(this Transform transform)
    {
        while (transform.childCount > 0)
        {
            Object.Destroy(transform.GetChild(0));
        }
    }
}