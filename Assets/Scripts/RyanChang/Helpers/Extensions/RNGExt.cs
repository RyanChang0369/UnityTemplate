using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RNGExt
{
    #region Variables
    private static readonly System.Random RNGNum = new();
    #endregion

    #region Integer
    /// <summary>
    /// Returns an integer ranging from minValue, inclusive, to maxValue,
    /// exclusive.
    /// </summary>
    /// <returns>An integer ranging from minValue to maxValue - 1.</returns>
    public static int RandomInt(int minVal, int maxVal)
    {
        return RNGNum.Next(minVal, maxVal);
    }

    /// <summary>
    /// Returns an integer ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>An integer ranging from 0 to maxValue - 1.</returns>
    public static int RandomInt(int maxVal)
    {
        return RandomInt(0, maxVal);
    }

    /// <summary>
    /// Alias for <see cref="System.Random.Next"/>.
    /// </summary>
    /// <returns>An integer ranging from 0 to <see
    /// cref="Int32.MaxValue"/>.</returns>
    public static int RandomInt()
    {
        return RNGNum.Next();
    }
    #endregion

    #region Boolean
    /// <summary>
    /// Returns a random boolean value.
    /// </summary>
    /// <returns>The random boolean value, either true (1) or false
    /// (0).</returns>
    public static bool RandomBool()
    {
        return RandomInt() % 2 == 0;
    }
    #endregion

    #region Double
    /// <summary>
    /// Returns a double ranging from minValue, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>An integer ranging from minValue to maxValue - 1.</returns>
    public static double RandomDouble(double minVal, double maxVal)
    {
        return RNGNum.NextDouble() * (maxVal - minVal) + minVal;
    }

    /// <summary>
    /// Returns a double ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>A double ranging from 0, inclusive, to maxValue,
    /// exclusive.</returns>
    public static double RandomDouble(double maxVal)
    {
        return RandomDouble(0, maxVal);
    }

    /// <summary>
    /// Returns a double ranging from 0, inclusive, to 1, exclusive.
    /// </summary>
    /// <returns>A double ranging from 0 to 1.</returns>
    public static double RandomDouble()
    {
        return RNGNum.NextDouble();
    }
    #endregion

    #region Float
    /// <summary>
    /// Returns a float ranging from minValue, inclusive, to maxValue,
    /// exclusive.
    /// </summary>
    /// <returns>A float ranging from minValue, inclusive, to maxValue,
    /// exclusive.</returns>
    public static float RandomFloat(float minVal, float maxVal)
    {
        return (float)(RNGNum.NextDouble() * (maxVal - minVal) + minVal);
    }

    /// <summary>
    /// Returns a float ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>A float ranging from minValue, inclusive, to maxValue,
    /// exclusive.</returns>
    public static float RandomFloat(float maxVal)
    {
        return RandomFloat(0, maxVal);
    }

    /// <summary>
    /// Returns a float ranging from 0, inclusive, to 1, exclusive.
    /// </summary>
    /// <returns>A float ranging from 0 to 1.</returns>
    public static float RandomFloat()
    {
        return (float)RNGNum.NextDouble();
    }

    /// <summary>
    /// Returns true based on percent chance given.
    /// </summary>
    /// <param name="percentChance">A float [0 - 1]</param>
    /// <returns>True based on percent chance given.</returns>
    public static bool PercentChance(float percentChance)
    {
        return RandomFloat() < percentChance;
    }
    #endregion

    #region Byte
    /// <summary>
    /// Gets a random byte string.
    /// </summary>
    /// <param name="bytes">How many bytes of RNG to generate?</param>
    /// <returns></returns>
    public static byte[] RandomHash(int bytes = 16)
    {
        byte[] arr = new byte[bytes];
        RNGNum.NextBytes(arr);
        return arr;
    }

    /// <inheritdoc cref="RandomHash(int)"/>
    /// <summary>
    /// Gets a random byte string as a hexadecimal hash.
    /// </summary>
    public static string RandomHashString(int bytes = 16) =>
        BitConverter.ToString(RandomHash(bytes)).Replace("-", "");
    #endregion

    #region Vector2
    /// <summary>
    /// Returns a Vector2 with all components as random values (determined by
    /// <see cref="RandomFloat()"/>).
    /// </summary>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2()
    {
        return new(RandomFloat(), RandomFloat());
    }

    /// <summary>
    /// Returns a Vector2 with all components ranging from -val (inclusive) to
    /// val (exclusive).
    /// </summary>
    /// <param name="val">The bounds of the Vector2.</param>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2(float val)
    {
        return RandomVector2(-val, val);
    }

    /// <summary>
    /// Returns a Vector2 with both components ranging from min to max.
    /// </summary>
    /// <param name="min">Minimum value of the x and y components,
    /// inclusive.</param>
    /// <param name="max">Maximum value of the x and y components,
    /// exclusive.</param>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2(float min, float max)
    {
        return RandomVector2(min, min, max, max);
    }

    /// <summary>
    /// Returns a random Vector2 with components ranging between the respective
    /// components of min and max.
    /// </summary>
    /// <param name="min">The lower bound of the random Vector2,
    /// inclusive.</param>
    /// <param name="max">The upper bound of the random Vector2,
    /// exclusive.</param>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2(Vector2 min, Vector2 max)
    {
        return RandomVector2(min.x, min.y, max.x, max.y);
    }

    /// <summary>
    /// Returns a random Vector2 with
    /// the x component ranging from minX to maxX and
    /// the y component ranging from minY to maxY.
    /// </summary>
    /// <param name="minX">Minimum value of the x component, inclusive.</param>
    /// <param name="minY">Minimum value of the y component, inclusive.</param>
    /// <param name="maxX">Maximum value of the x component, exclusive.</param>
    /// <param name="maxY">Maximum value of the y component, exclusive.</param>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2(float minX, float minY, float maxX,
        float maxY)
    {
        return new Vector2(RandomFloat(minX, maxX), RandomFloat(minY, maxY));
    }

    /// <summary>
    /// Returns a random Vector2 with its respective components ranging from
    /// rect.min (inclusive) to rect.max (exclusive).
    /// </summary>
    /// <param name="rect">The bounds.</param>
    /// <returns></returns>
    public static Vector2 WithinRect(this Rect rect)
    {
        return RandomVector2(rect.min.x, rect.min.y, rect.max.x,
            rect.max.y);
    }

    #region Circle
    /// <summary>
    /// Gets a point on the perimeter of a circle.
    /// </summary>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A point on the circle.</returns>
    public static Vector2 OnCircle(float radius = 1)
    {
        return RandomVector2().normalized * radius;
    }

    /// <summary>
    /// Gets a point within a circle.
    /// </summary>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A point within the circle.</returns>
    public static Vector2 WithinCircle(float radius = 1)
    {
        return RandomVector2().normalized * RandomFloat(radius);
    }
    #endregion

    #region Square
    /// <summary>
    /// Gets a point on the perimeter of a square.
    /// </summary>
    /// <param name="length">The length of one of the square's sides.</param>
    /// <returns>A point on the square.</returns>
    public static Vector2 OnSquare(float length = 1)
    {
        return RandomVector2().normalized * length;
    }

    /// <summary>
    /// Gets a point within a square.
    /// </summary>
    /// <param name="length">The length of one of the square's sides.</param>
    /// <returns>A point within the square.</returns>
    public static Vector2 WithinSquare(float length = 1)
    {
        var vector = RandomVector2();
        var mag = vector.sqrMagnitude;

        if (mag.Approx(0))
            return Vector2.zero;

        return vector / mag * length;
    }
    #endregion
    #endregion

    #region Vector3
    #region Vector
    /// <summary>
    /// Returns a Vector3 with all components as random values (determined by
    /// <see cref="RandomFloat()"/>).
    /// </summary>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3()
    {
        return new(RandomFloat(), RandomFloat(), RandomFloat());
    }

    /// <summary>
    /// Returns a Vector3 with all components ranging from -val (inclusive) to
    /// val (exclusive).
    /// </summary>
    /// <param name="val">The bounds of the Vector3.</param>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3(float val)
    {
        return RandomVector3(-val, val);
    }

    /// <summary>
    /// Returns a Vector3 with all components ranging between min and max.
    /// </summary>
    /// <param name="min">
    /// Minimum value of the x, y, and z components, inclusive.
    /// </param>
    /// <param name="max">
    /// Maximum value of the x, y, and z components, exclusive.
    /// </param>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3(float min, float max)
    {
        return RandomVector3(min, min, min, max, max, max);
    }

    /// <summary>
    /// Returns a random Vector3 with components ranging between the respective
    /// components of min and max.
    /// </summary>
    /// <param name="min">
    /// The lower bound of the random Vector3, inclusive.
    /// </param>
    /// <param name="max">
    /// The upper bound of the random Vector3, exclusive.
    /// </param>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        return RandomVector3(min.x, min.y, min.z, max.x, max.y, max.z);
    }

    /// <summary>
    /// Returns a random Vector3 with the x component ranging from minX to maxX,
    /// the y component ranging from minY to maxY, and the z component ranging
    /// from minZ to maxZ.
    /// </summary>
    /// <param name="minX">Minimum value of the x component, inclusive.</param>
    /// <param name="minY">Minimum value of the y component, inclusive.</param>
    /// <param name="minZ">Minimum value of the z component, inclusive.</param>
    /// <param name="maxX">Maximum value of the x component, exclusive.</param>
    /// <param name="maxY">Maximum value of the y component, exclusive.</param>
    /// <param name="maxZ">Maximum value of the z component, exclusive.</param>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3(float minX, float minY,
        float minZ, float maxX, float maxY, float maxZ)
    {
        return new Vector3(
            RandomFloat(minX, maxX),
            RandomFloat(minY, maxY),
            RandomFloat(minZ, maxZ)
            );
    }
    #endregion

    #region Bounds
    /// <summary>
    /// Returns a random Vector3 with its respective components ranging from
    /// bounds.min (inclusive) to bounds.max (exclusive).
    /// </summary>
    /// <param name="bounds">The bounds.</param>
    /// <returns></returns>
    public static Vector3 WithinBounds(Bounds bounds)
    {
        return RandomVector3(bounds.min.x, bounds.min.y, bounds.max.z,
            bounds.max.x, bounds.max.y, bounds.max.z);
    }
    #endregion

    #region Sphere
    /// <summary>
    /// Returns a random Vector3 that lies on the surface of the sphere,
    /// centered at (0,0,0), with the specified <paramref name="radius"/>.
    /// </summary>
    /// <param name="radius">Radius of the sphere.</param>
    public static Vector3 OnSphere(float radius = 1)
    {
        return RandomVector3().normalized * radius;
    }

    /// <summary>
    /// Returns a random Vector3 that lies on the surface of the sphere,
    /// centered at (0,0,0), with the specified <paramref name="radius"/>.
    /// </summary>
    /// <param name="radius">Radius of the sphere.</param>
    public static Vector3 WithinSphere(float radius = 1) =>
        OnSphere(RandomFloat(radius));
    #endregion

    #region Cube
    /// <summary>
    /// Returns a random Vector3 that lies on the surface of the cube,
    /// centered at (0,0,0), with the specified <paramref name="length"/>.
    /// </summary>
    /// <param name="length">Length of one of the cube's sides.</param>
    public static Vector3 OnCube(float length = 1)
    {
        var vector = RandomVector3();
        var mag = vector.sqrMagnitude;

        if (mag.Approx(0))
            return Vector3.zero;

        return vector / mag * length;
    }

    /// <summary>
    /// Returns a random Vector3 that lies on the surface of the cube,
    /// centered at (0,0,0), with the specified <paramref name="length"/>.
    /// </summary>
    /// <param name="length">Length of one of the cube's sides.</param>
    public static Vector3 WithinCube(float length = 1) =>
        OnCube(RandomFloat(length));
    #endregion
    #endregion

    #region Vector4
    #region Vector
    /// <summary>
    /// Returns a Vector4 with all components as random values (determined by
    /// <see cref="RandomFloat()"/>).
    /// </summary>
    /// <returns>A random Vector4.</returns>
    public static Vector4 RandomVector4()
    {
        return new(RandomFloat(), RandomFloat(), RandomFloat(), RandomFloat());
    }

    /// <summary>
    /// Returns a Vector4 with all components ranging from -val (inclusive) to
    /// val (exclusive).
    /// </summary>
    /// <param name="val">The bounds of the Vector4.</param>
    /// <returns>A random Vector4.</returns>
    public static Vector4 RandomVector4(float val)
    {
        return RandomVector4(-val, val);
    }

    /// <summary>
    /// Returns a Vector4 with all components ranging between min and max.
    /// </summary>
    /// <param name="min">
    /// Minimum value of the x, y, z, and w components, inclusive.
    /// </param>
    /// <param name="max">
    /// Maximum value of the x, y, z, and w components, exclusive.
    /// </param>
    /// <returns>A random Vector4.</returns>
    public static Vector4 RandomVector4(float min, float max)
    {
        return RandomVector4(min, min, min, min, max, max, max, max);
    }

    /// <summary>
    /// Returns a random Vector4 with components ranging between the respective
    /// components of min and max.
    /// </summary>
    /// <param name="min">The lower bound of the random Vector4,
    /// inclusive.</param>
    /// <param name="max">The upper bound of the random Vector4,
    /// exclusive.</param>
    /// <returns>A random Vector4.</returns>
    public static Vector4 RandomVector4(Vector4 min, Vector4 max)
    {
        return RandomVector4(min.x, min.y, min.z, min.w,
            max.x, max.y, max.z, max.w);
    }

    /// <summary>
    /// Returns a random Vector4 with the x component ranging from <paramref
    /// name="minX"/> to <paramref name="maxX"/>, the y component ranging from
    /// <paramref name="minY"/> to <paramref name="maxY"/>, the z component
    /// ranging from <paramref name="minZ"/> to <paramref name="maxZ"/>, and the
    /// w component ranging from <paramref name="minW"/> to <paramref
    /// name="maxW"/>.
    /// </summary>
    /// <param name="minX">Minimum value of the x component, inclusive.</param>
    /// <param name="minY">Minimum value of the y component, inclusive.</param>
    /// <param name="minZ">Minimum value of the z component, inclusive.</param>
    /// <param name="minW">Minimum value of the w component, inclusive.</param>
    /// <param name="maxX">Maximum value of the x component, exclusive.</param>
    /// <param name="maxY">Maximum value of the y component, exclusive.</param>
    /// <param name="maxZ">Maximum value of the z component, exclusive.</param>
    /// <param name="maxW">Maximum value of the w component, exclusive.</param>
    /// <returns>A random Vector4.</returns>
    public static Vector4 RandomVector4(float minX, float minY,
        float minZ, float minW, float maxX, float maxY, float maxZ, float maxW)
    {
        return new Vector4(
            RandomFloat(minX, maxX),
            RandomFloat(minY, maxY),
            RandomFloat(minZ, maxZ),
            RandomFloat(minW, maxW)
            );
    }
    #endregion

    #region Hyper Sphere
    /// <summary>
    /// Returns a random Vector4 that lies on the surface of the 4 dimensional
    /// hyper-sphere, centered at (0,0,0,0), with the specified <paramref
    /// name="radius"/>.
    /// </summary>
    /// <param name="radius">Radius of the hyper-sphere.</param>
    public static Vector4 OnHyperSphere(float radius = 1)
    {
        return RandomVector4().normalized * radius;
    }

    /// <summary>
    /// Returns a random Vector4 that lies on the surface of the 4 dimensional
    /// hyper-sphere, centered at (0,0,0,0), with the specified <paramref
    /// name="radius"/>.
    /// </summary>
    /// <param name="radius">Radius of the hyper-sphere.</param>
    public static Vector4 WithinHyperSphere(float radius = 1) =>
        OnHyperSphere(RandomFloat(radius));
    #endregion

    #region Hyper Cube
    /// <summary>
    /// Returns a random Vector4 that lies on the surface of the 4 dimensional
    /// hyper-cube, centered at (0,0,0,0), with the specified <paramref
    /// name="length"/>.
    /// </summary>
    /// <param name="length">Length of one of the hyper-cube's sides.</param>
    public static Vector4 OnHyperCube(float length = 1)
    {
        var vector = RandomVector4();
        var mag = vector.sqrMagnitude;

        if (mag.Approx(0))
            return Vector4.zero;

        return vector / mag * length;
    }

    /// <summary>
    /// Returns a random Vector4 that lies on the surface of the 4 dimensional
    /// hyper-cube, centered at (0,0,0,0), with the specified <paramref
    /// name="length"/>.
    /// </summary>
    /// <param name="length">Length of one of the hyper-cube's sides.</param>
    public static Vector4 WithinHyperCube(float length = 1) =>
        OnHyperCube(RandomFloat(length));
    #endregion
    #endregion

    #region Rect
    /// <summary>
    /// Generates a rectangle with random components.
    /// </summary>
    /// <param name="minX">Minimum value of the x component, inclusive.</param>
    /// <param name="minY">Minimum value of the y component, inclusive.</param>
    /// <param name="minWidth">Minimum value of the width component,
    /// inclusive.</param>
    /// <param name="minHeight">Minimum value of the height component,
    /// inclusive.</param>
    /// <param name="maxX">Maximum value of the x component, exclusive.</param>
    /// <param name="maxY">Maximum value of the y component, exclusive.</param>
    /// <param name="maxWidth">Maximum value of the width component,
    /// exclusive.</param>
    /// <param name="maxHeight">Maximum value of the height component,
    /// exclusive.</param>
    /// <returns>The random rectangle.</returns>
    public static Rect RandomRect(
        float minX, float minY, float minWidth, float minHeight,
        float maxX, float maxY, float maxWidth, float maxHeight) =>
        new(
            RandomFloat(minX, maxX),
            RandomFloat(minY, maxY),
            RandomFloat(minWidth, maxWidth),
            RandomFloat(minHeight, maxHeight)
        );

    /// <inheritdoc cref="RandomRect(float, float, float, float, float, float,
    /// float, float)"/>
    /// <summary>
    /// Generates a random rectangle that lies between the rectangle bounded by
    /// <paramref name="minimum"/> and <paramref name="maximum"/>.
    /// </summary>
    /// <param name="minimum">The lower bound.</param>
    /// <param name="maximum">The higher bound.</param>
    public static Rect RandomRect(Vector2 minimum, Vector2 maximum)
    {
        var lowerX = Mathf.Min(minimum.x, maximum.x);
        var lowerY = Mathf.Min(minimum.y, maximum.y);
        var higherX = Mathf.Max(minimum.x, maximum.x);
        var higherY = Mathf.Max(minimum.y, maximum.y);

        return RandomRect(
            lowerX, lowerY, 0, 0,
            higherX, higherY, higherX - lowerX, higherY - lowerY
        );
    }

    /// <inheritdoc cref="RandomRect(float, float, float, float, float, float,
    /// float, float)"/>
    /// <summary>
    /// Generates a random rectangle that lies within <paramref name="bound"/>.
    /// </summary>
    /// <param name="bound">The bounds of the random rectangle.</param>
    public static Rect RandomRect(Rect bound) =>
        RandomRect(bound.min, bound.max);
    #endregion

    #region Curve
    /// <summary>
    /// Retrieves a random value on the animation curve, allowing for the biased
    /// selection of random values.
    /// </summary>
    /// <param name="curve">The animation curve.</param>
    /// <param name="floor">The absolute minimal value this method can
    /// return. This is disabled if the value given is not finite.</param>
    /// <param name="ceiling">The absolute maximal value this method can
    /// return. This is disabled if the value given is not finite.</param>
    public static float RandomValueOnCurve(this AnimationCurve curve,
        float floor, float ceiling)
    {
        if (curve == null)
            throw new ArgumentNullException(
                "Value of curve not set."
            );
        else if (curve.keys.IsEmpty())
            throw new ArgumentOutOfRangeException(
                "Curve does not have any keys."
            );

        float minT = curve.keys.First().time;
        float maxT = curve.keys.Last().time;

        if (minT > maxT)
            throw new ArgumentException(
                "Curve is malformed (first key occurs before the last key)"
            );

        float val = curve.Evaluate(RandomFloat(minT, maxT));

        if (float.IsFinite(floor))
            val = Mathf.Max(val, floor);
        if (float.IsFinite(ceiling))
            val = Mathf.Min(val, ceiling);

        return val;
    }

    /// <inheritdoc cref="RandomValueOnCurve(AnimationCurve, float, float)"/>
    public static float RandomValueOnCurve(this AnimationCurve curve) =>
        RandomValueOnCurve(curve, float.NaN, float.NaN);
    #endregion

    #region IEnumerable & Related
    /// <summary>
    /// Returns a random value from the provided enumeration of values.
    /// </summary>
    /// <param name="defaultOK">If true, then return default value if values
    /// length is 0. Else throw an index error.</param>
    public static T GetRandomValue<T>(this IEnumerable<T> values,
        bool defaultOK = true)
    {
        int length = values.Count();

        if (length < 1 && defaultOK)
        {
            return default;
        }

        return values.ElementAt(RandomInt(length));
    }

    /// <inheritdoc cref="GetRandomValue{T}(IEnumerable{T}, bool)"/>
    public static T GetRandomValue<T>(params T[] values) =>
        values.GetRandomValue();

    /// <inheritdoc cref="GetRandomValue{T}(IEnumerable{T}, bool)"/>
    /// <summary>
    /// Returns a random value from the provided enumeration of values. Alias
    /// for <see cref="GetRandomValue{T}(IEnumerable{T}, bool)"/>.
    /// </summary>
    public static T RandomSelectOne<T>(this IEnumerable<T> values,
        bool defaultOK = true)
    {
        return values.GetRandomValue(defaultOK);
    }

    /// <summary>
    /// Selects multiple items from an IEnumerable. Can also select none.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key">How to get to the percentage [0-1] from any
    /// obj.</param>
    /// <returns></returns>
    public static IEnumerable<T> RandomSelectMany<T>(this IEnumerable<T> objs,
        Func<T, float> key)
    {
        return objs
            .Where(obj => PercentChance(key(obj)));
    }

    /// <summary>
    /// Randomly selects multiple items from an IEnumerable. Guaranteed to
    /// select at least one item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IEnumerable<T> RandomSelectAtLeastOne<T>(
        this IEnumerable<T> objs, Func<T, float> key)
    {
        var selected = objs.RandomSelectMany(key);

        if (selected.Count() < 1)
        {
            T[] arr = { objs.GetRandomValue() };
            selected = arr.AsEnumerable();
        }

        return selected;
    }

    /// <summary>
    /// Selects one or no items from an IEnumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key">
    /// How to get to the percentage [0-1] from any obj.
    /// </param>
    /// <returns></returns>
    public static T RandomSelectOneOrNone<T>(this IEnumerable<T> objs,
        Func<T, float> key)
    {
        IEnumerable<T> selected = objs.RandomSelectMany(key);

        if (selected.Count() < 1)
        {
            return objs.GetRandomValue();
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Selects one item from an IEnumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key">How to get to the percentage [0-1] from any
    /// obj.</param>
    /// <returns></returns>
    public static T RandomSelectOne<T>(this IEnumerable<T> objs, Func<T, float> key)
    {
        IEnumerable<T> selected = objs.RandomSelectMany(key);

        if (selected.Count() < 1)
        {
            return objs.GetRandomValue();
        }
        else
        {
            return selected.GetRandomValue();
        }
    }


    /// <summary>
    /// Shuffles the list in place. This is an O(n) operation. Adapted from
    /// https://stackoverflow.com/a/1262619.
    /// </summary>
    /// <typeparam name="T">Any type.</typeparam>
    /// <param name="list">List to shuffle.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RandomInt(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    /// <summary>
    /// Selects one item from a param list of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">List to select from.</param>
    /// <returns>A random item from items.</returns>
    public static T SelectOneFrom<T>(params T[] items)
    {
        return items.GetRandomValue<T>();
    }
    #endregion
}
