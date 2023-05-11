using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEditor;

public static class ExtensionMethods
{
    private const string PROJECT_NAME = "Sunder";

    /// <summary>
    /// Checks if an object either
    /// - is null
    /// - is a UnityEngine.Object that is == null, meaning that's invalid - ie.
	/// Destroyed, not assigned, or created with new
    ///
    /// Unity overloads the == operator for UnityEngine.Object, and returns true
	/// for a == null both if a is null, or if
    /// it doesn't exist in the c++ engine. This method is for checking for
	/// either of those being the case
    /// for objects that are not necessarily UnityEngine.Objects.
	/// This is useful when you're using interfaces, since ==
    /// is a static method, so if you check if a member of an interface == null,
	/// it will hit the default C# == check instead
    /// of the overridden Unity check.
    /// 
    /// Source:
	/// https://forum.unity.com/threads/when-a-rigid-body-is-not-attached-component-getcomponent-rigidbody-returns-null-as-a-string.521633/
    /// </summary>
    /// <param name="obj">Object to check</param>
    /// <returns>True if the object is null, or if it's a UnityEngine.Object that has been destroyed</returns>
    public static bool IsNullOrUnityNull(this object obj)
    {
        if (obj == null)
        {
            return true;
        }

        if (obj is UnityEngine.Object @object)
        {
            if (@object == null)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }

    public static void Swap<T>(ref T a, ref T b)
    {
        T t = a;
        a = b;
        b = t;
    }

    /// <summary>
    /// Converts from polar coordinates
    /// </summary>
    /// <param name="r">Radius</param>
    /// <param name="theta">Angle, in radians</param>
    /// <returns></returns>
    public static Vector2 ConvertFromPolarCoords(float r, float theta)
    {
        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);

        return new Vector2(x, y);
    }

    /// <summary>
    /// See documentation for
    /// https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
    /// </summary>
    /// <returns></returns>
    public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime,
        float maxSpeed, float deltaTime)
    {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
          0,
          0,
          Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime)
        );
    }

    public static void WriteSaveFile(string output, string fileName)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PROJECT_NAME, fileName);

        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PROJECT_NAME));
        }

        using (StreamWriter sw = new(filePath))
        {
            sw.Write(output);
        }
    }

    public static string ReadSaveFile(string fileName)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PROJECT_NAME, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Cannot find " + fileName);
        }

        using (StreamReader sr = new(filePath))
        {
            return sr.ReadToEnd();
        }
    }

    public static bool Contains(this string string1, string string2, StringComparison stringComparison)
    {
        return string1.IndexOf(string2, stringComparison) >= 0;
    }

    public static Color GetColorFromHEX(this string hexCode)
    {
        if (!hexCode.Contains("#"))
        {
            hexCode = "#" + hexCode;
        }

        Color color;

        ColorUtility.TryParseHtmlString(hexCode, out color);

        return color;
    }

    public static Texture2D GetTexture(this Sprite sprite)
    {
        Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

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
    /// Adds addition to a dictionary with a list as its value.
    /// <typeparam name="TKey">The key.</typeparam>
    /// <typeparam name="TMem">The member of the list within the dictionary.</typeparam>
    /// <param name="dict">The dictionary to add to.</param>
    /// <param name="key">The key that holds the list to add addition to.</param>
    /// <param name="addition">What to add to the internal list.</param>
    /// </summary>
    public static void AddToDictList<TKey, TMem>(this Dictionary<TKey, List<TMem>> dict, TKey key, TMem addition)
    {
        if (!dict.ContainsKey(key))
            dict[key] = new List<TMem>();

        dict[key].Add(addition);
    }

    /// <summary>
    /// Shuffles the list in place. Adapted from https://stackoverflow.com/a/1262619.
    /// </summary>
    /// <typeparam name="T">Any type.</typeparam>
    /// <param name="list">List to shuffle.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RNG.GetRandomInteger(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
