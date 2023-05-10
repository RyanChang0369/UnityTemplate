using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RNG
{
    private static System.Random RNGNum = new System.Random();

    /// <summary>
    /// Returns true based on percent chance given.
    /// </summary>
    /// <param name="percentChance">A float [0 - 1]</param>
    /// <returns>True based on percent chance given.</returns>
    public static bool PercentChance(float percentChance)
    {
        return GetRandomFloat() < percentChance;
    }

    /// <summary>
    /// Returns an integer ranging from 0 - 99.
    /// </summary>
    /// <returns>An integer ranging from 0 - 99.</returns>
    public static float GetRandomPercent()
    {
        return GetRandomInteger(100);
    }

    /// <summary>
    /// Returns an integer ranging from minValue, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>An integer ranging from minValue to maxValue - 1.</returns>
    public static int GetRandomInteger(int minVal, int maxVal)
    {
        return RNGNum.Next(maxVal - minVal) + minVal;
    }

    /// <summary>
    /// Returns an integer ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>An integer ranging from 0 to maxValue - 1.</returns>
    public static int GetRandomInteger(int maxVal)
    {
        return GetRandomInteger(0, maxVal);
    }

    /// <summary>
    /// Returns a double ranging from minValue, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>An integer ranging from minValue to maxValue - 1.</returns>
    public static double GetRandomDouble(double minVal, double maxVal)
    {
        return RNGNum.NextDouble() * (maxVal - minVal) + minVal;
    }

    /// <summary>
    /// Returns a double ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>A double ranging from 0, inclusive, to maxValue, exclusive.</returns>
    public static double GetRandomDouble(double maxVal)
    {
        return GetRandomDouble(0, maxVal);
    }

    /// <summary>
    /// Returns a double ranging from 0, inclusive, to 1, exclusive.
    /// </summary>
    /// <returns>A double ranging from 0 to 1.</returns>
    public static double GetRandomDouble()
    {
        return RNGNum.NextDouble();
    }

    /// <summary>
    /// Returns a float ranging from minValue, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>A float ranging from minValue, inclusive, to maxValue, exclusive.</returns>
    public static float GetRandomFloat(float minVal, float maxVal)
    {
        return (float)(RNGNum.NextDouble() * (maxVal - minVal) + minVal);
    }

    /// <summary>
    /// Returns a float ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>A float ranging from minValue, inclusive, to maxValue, exclusive.</returns>
    public static float GetRandomFloat(float maxVal)
    {
        return GetRandomFloat(0, maxVal);
    }

    /// <summary>
    /// Returns a float ranging from the smallest value of the Vector2 (inclusive)
    /// to the largest value (exclusive) 
    /// </summary>
    public static float GetRandomFloat(Vector2 val)
    {
        return GetRandomFloat(Mathf.Min(val.x, val.y), Mathf.Max(val.x, val.y));
    }

    /// <summary>
    /// Returns a Vector2 with all components ranging from
    /// -val to val
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static Vector2 GetRandomVector3(float val)
    {
        return GetRandomVector3(-val, val);
    }

    /// <summary>
    /// Gets a point on the unit circle. Works everywhere.
    /// </summary>
    /// <returns>S point on the unit circle.</returns>
    public static Vector2 OnUnitCircle()
    {
        return Quaternion.Euler(GetRandomFloat(0, 360), 0, 0) * Vector2.up;
    }

    /// <summary>
    /// Returns a Vector2 with all components ranging from
    /// minVal to maxVal
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static Vector3 GetRandomVector3(float minVal, float maxVal)
    {
        return GetRandomVector3(minVal, minVal, minVal, maxVal, maxVal, maxVal);
    }

    /// <summary>
    /// Returns a random Vector2 ranging from min to max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Vector3 GetRandomVector3(Vector3 min, Vector3 max)
    {
        return GetRandomVector3(min.x, min.y, min.z, max.x, max.y, max.z);
    }

    public static Vector3 GetRandomVector3(float minValX, float minValY, float minValZ, float maxValX, float maxValY, float maxValZ)
    {
        return new Vector3(
            GetRandomFloat(minValX, maxValX),
            GetRandomFloat(minValY, maxValY),
            GetRandomFloat(minValZ, maxValZ)
            );
    }

    /// <summary>
    /// Returns a Vector2 with both components ranging from
    /// -val to val
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static Vector2 GetRandomVector2(float val)
    {
        return GetRandomVector2(-val, val);
    }

    /// <summary>
    /// Returns a Vector2 with both components ranging from
    /// minVal to maxVal
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static Vector2 GetRandomVector2(float minVal, float maxVal)
    {
        return GetRandomVector2(minVal, minVal, maxVal, maxVal);
    }

    ///// <summary>
    ///// Returns a random Vector2 ranging from +bound to -bound
    ///// </summary>
    ///// <param name="bound"></param>
    ///// <returns></returns>
    //public static Vector2 GetRandomVector2(Vector2 bound)
    //{
    //    return GetRandomVector2(bound.x, bound.y);
    //}

    /// <summary>
    /// Returns a random Vector2 ranging from min to max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Vector2 GetRandomVector2(Vector2 min, Vector2 max)
    {
        return GetRandomVector2(min.x, min.y, max.x, max.y);
    }

    public static Vector2 GetRandomVector2(float minValX, float minValY, float maxValX, float maxValY)
    {
        return new Vector2(GetRandomFloat(minValX, maxValX), GetRandomFloat(minValY, maxValY));
    }

    /// <summary>
    /// Returns a float ranging from 0, inclusive, to 1, exclusive.
    /// </summary>
    /// <returns>A float ranging from 0 to 1.</returns>
    public static float GetRandomFloat()
    {
        return (float)RNGNum.NextDouble();
    }
    
    /// <summary>
    /// Returns a random value from the provided enumeration of values.
    /// </summary>
    /// <param name="defaultOK">If true, then return default value if values length is 0. Else throw an error.</param>
    public static T GetRandomValue<T>(this IEnumerable<T> values, bool defaultOK = true)
    {
        int length = values.Count();

        if (length < 1 && defaultOK)
        {
            return default;
        }

        return values.ElementAt(GetRandomInteger(length));
    }

    ///// <summary>
    ///// Returns a random value along with its index in the enum.
    ///// </summary>
    ///// <param name="defaultOK">If true, then return default value if values length is 0. Else throw an error.</param>
    ///// <returns>(index, value)</returns>
    //public static Tuple<int, T> GetRandomValuePair<T>(this IEnumerable<T> values, bool defaultOK = true)
    //{
    //    int length = values.Count();

    //    if (length < 1 && defaultOK)
    //    {
    //        return default;
    //    }

    //    int randI = GetRandomInteger(length);

    //    return new(randI, values.ElementAt(randI));
    //}

    /// <summary>
    /// Selects multiple items from an IEnumerable. Can also select none.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key">How to get to the percentage [0-1] from any obj.</param>
    /// <returns></returns>
    public static IEnumerable<T> RandomSelectMany<T>(this IEnumerable<T> objs, Func<T, float> key)
    {
        List<T> selected = new List<T>();

        foreach (T obj in objs)
        {
            if (PercentChance(key(obj)))
            {
                selected.Add(obj);
            }
        }

        return selected;
    }

    ///// <summary>
    ///// Selects multiple items from an IEnumerable. Can also select none.
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="objs"></param>
    ///// <param name="key">How to get to the percentage [0-1] from any obj.</param>
    ///// <returns>(index, value)</returns>
    //public static IEnumerable<Tuple<int, T>> RandomSelectManyPairs<T>(this IEnumerable<T> objs, Func<T, float> key)
    //{
    //    List<Tuple<int, T>> selected = new List<Tuple<int, T>>();

    //    int i = 0;

    //    foreach (T obj in objs)
    //    {
    //        if (PercentChance(key(obj)))
    //        {
    //            selected.Add(new(i, obj));
    //        }

    //        i++;
    //    }

    //    return selected;
    //}

    /// <summary>
    /// Randomly selects multiple items from an IEnumerable. Guarenteed to select at least one
    /// item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IEnumerable<T> RandomSelectAtLeastOne<T>(this IEnumerable<T> objs, Func<T, float> key)
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
    /// <param name="key">How to get to the percentage [0-1] from any obj.</param>
    /// <returns></returns>
    public static T RandomSelectOneOrNone<T>(this IEnumerable<T> objs, Func<T, float> key)
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
    /// <param name="key">How to get to the percentage [0-1] from any obj.</param>
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

    ///// <summary>
    ///// Selects one item pair from an IEnumerable
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="objs"></param>
    ///// <param name="key">How to get to the percentage [0-1] from any obj.</param>
    ///// <returns>(index, value)</returns>
    //public static Tuple<int, T> RandomSelectOnePair<T>(this IEnumerable<T> objs, Func<T, float> key)
    //{
    //    IEnumerable<T> selected = objs.RandomSelectMany(key);

    //    if (selected.Count() < 1)
    //    {
    //        return objs.GetRandomValuePair();
    //    }
    //    else
    //    {
    //        return selected.GetRandomValuePair();
    //    }
    //}

    
}

