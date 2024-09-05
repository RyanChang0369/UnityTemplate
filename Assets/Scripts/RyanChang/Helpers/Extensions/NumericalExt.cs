using System;
using UnityEngine;

/// <summary>
/// Contains methods pertaining to C# floats, doubles, and ints.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public static class NumericalExt
{
    #region Comparison
    /// <summary>
    /// True if a differs from b by no more than <paramref name="margin"/>.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <param name="margin">The margin.</param>
    public static bool Approx(this float a, float b, float margin)
    {
        return Mathf.Abs(a - b) <= margin;
    }

    /// <inheritdoc cref="Approx(float, float, float)"/>
    /// <summary>
    /// Alias to <see cref="Mathf.Approximately(float, float)"/>
    /// </summary>
    public static bool Approx(this float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    /// <summary>
    /// Returns true if number is in between bounds A and B, inclusive
    /// </summary>
    /// <param name="number">The number to evaluate</param>
    /// <param name="boundsA">The lower bound</param>
    /// <param name="boundsB">The upper bound</param>
    /// <param name="fixRange">If true, swap bounds A and B if B < A.</param>
    public static bool IsBetween(this float number, float boundsA,
        float boundsB, bool fixRange = true)
    {
        if (fixRange)
        {
            float temp = boundsA;

            boundsA = Mathf.Min(boundsA, boundsB);
            boundsB = Mathf.Max(boundsB, temp);
        }

        return boundsA <= number && number <= boundsB;
    }
    #endregion

    #region Sign
    /// <summary>
    /// How zero is handled by sign-determining functions (ie <see
    /// cref="IsPositive"/>, <see cref="IsNegative"/>, <see cref="Sign"/>, etc).
    /// </summary>
    public enum ZeroSignBehavior
    {
        /// <summary>
        /// If number is zero, a boolean function will return false, and an
        /// integer function will return zero.
        /// </summary>
        ZeroIsFalse,

        /// <summary>
        /// If number is zero, a boolean function will return true, and an
        /// integer function will return zero.
        /// </summary>
        ZeroIsTrue,

        /// <summary>
        /// If number is zero, this function will treat the number as positive.
        /// </summary>
        ZeroIsPositive,

        /// <summary>
        /// If number is zero, this function will treat the number as negative.
        /// </summary>
        ZeroIsNegative
    }

    /// <summary>
    /// Returns true if <paramref name="number"/> is less than 0.
    /// </summary>
    /// <param name="number">The number to compare with.</param>
    /// <returns></returns>
    public static bool IsPositive<N>(this N number,
        ZeroSignBehavior behavior = ZeroSignBehavior.ZeroIsPositive)
        where N : IComparable => behavior switch
        {
            ZeroSignBehavior.ZeroIsFalse => number.CompareTo(0.0) > 0,
            ZeroSignBehavior.ZeroIsNegative => number.CompareTo(0.0) > 0,
            ZeroSignBehavior.ZeroIsTrue => number.CompareTo(0.0) >= 0,
            ZeroSignBehavior.ZeroIsPositive => number.CompareTo(0.0) >= 0,
            _ => throw new NotImplementedException(),
        };

    /// <summary>
    /// Returns true if <paramref name="number"/> is less than 0.
    /// </summary>
    /// <param name="number">The number to compare with.</param>
    /// <returns></returns>
    public static bool IsNegative<N>(this N number,
        ZeroSignBehavior behavior = ZeroSignBehavior.ZeroIsPositive)
        where N : IComparable => behavior switch
        {
            ZeroSignBehavior.ZeroIsFalse => number.CompareTo(0.0) < 0,
            ZeroSignBehavior.ZeroIsPositive => number.CompareTo(0.0) < 0,
            ZeroSignBehavior.ZeroIsTrue => number.CompareTo(0.0) <= 0,
            ZeroSignBehavior.ZeroIsNegative => number.CompareTo(0.0) <= 0,
            _ => throw new NotImplementedException(),
        };

    /// <summary>
    /// Returns the sign of number.
    /// </summary>
    /// <param name="number">The sign of the number.</param>
    /// <param name="behavior">The behavior of the method.</param>
    /// <returns>-1, 0, or 1, depending on the value of <paramref
    /// name="behavior"/>.</returns>
    public static int Sign<N>(this N number,
        ZeroSignBehavior behavior = ZeroSignBehavior.ZeroIsPositive)
        where N : IComparable
    {
        int cmp = number.CompareTo(0.0);
        cmp = (cmp == 0) ? 0 : (cmp / Math.Abs(cmp));

        return behavior switch
        {
            ZeroSignBehavior.ZeroIsFalse or
                ZeroSignBehavior.ZeroIsTrue => cmp,
            ZeroSignBehavior.ZeroIsNegative => cmp == 0 ? -1 : cmp,
            ZeroSignBehavior.ZeroIsPositive => cmp == 0 ? 1 : cmp,
            _ => throw new NotImplementedException(),
        };
    }
    #endregion

    #region Deltas
    /// <summary>
    /// Returns value such that the change of value is towards target and is no
    /// greater than margin;
    /// </summary>
    /// <param name="value">The value to change.</param>
    /// <param name="target">The number to change towards.</param>
    /// <param name="margin">The maximal change.</param>
    /// <returns></returns>
    public static float GetMinimumDelta(this float value, float target,
        float margin)
    {
        return value + Mathf.Sign(target)
            * Mathf.Min(Mathf.Abs(target), Mathf.Abs(margin));
    }
    #endregion

    #region Rounding
    #region Enum
    public enum RoundMode
    {
        /// <summary>
        /// Rounds to the nearest int.
        /// </summary>
        NearestInt,
        /// <summary>
        /// Rounds to the nearest int greater than the value.
        /// </summary>
        Ceiling,
        /// <summary>
        /// Rounds to the nearest int lesser than the value.
        /// </summary>
        Floor,
        /// <summary>
        /// If value is positive, round to the nearest int greater than value.
        /// Else, round to the nearest int lesser than value.
        /// </summary>
        IncreaseAbs,
        /// <summary>
        /// If value is positive, round to the nearest int lesser than value.
        /// Else, round to the nearest int greater than value.
        /// </summary>
        DecreaseAbs
    }
    #endregion

    /// <summary>
    /// Rounds the float using the provided rounding method.
    /// </summary>
    /// <param name="number">The float to round.</param>
    /// <param name="mode">How to round <paramref name="number"/>.</param>
    /// <returns></returns>
    public static int RoundToInt(this float number,
        RoundMode mode = RoundMode.NearestInt)
    {
        switch (mode)
        {
            case RoundMode.NearestInt:
                return Mathf.RoundToInt(number);
            case RoundMode.Ceiling:
                return Mathf.CeilToInt(number);
            case RoundMode.Floor:
                return Mathf.FloorToInt(number);
            case RoundMode.IncreaseAbs:
                if (number < 0)
                    return Mathf.FloorToInt(number);
                else if (number > 0)
                    return Mathf.CeilToInt(number);
                else
                    return 0;
            case RoundMode.DecreaseAbs:
            default:
                if (number > 0)
                    return Mathf.FloorToInt(number);
                else if (number < 0)
                    return Mathf.CeilToInt(number);
                else
                    return 0;
        }
    }

    /// <summary>
    /// Alias for <see cref="Mathf.Round(float)"/>.
    /// </summary>
    /// <param name="number">Number to round.</param>
    /// <param name="digits">Places after zero to round to.</param>
    /// <returns>The rounded value.</returns>
    public static float Round(this float number, int digits = 0) =>
        (float)Math.Round(number, digits);
    #endregion

    #region Misc Operations
    /// <summary>
    /// Returns the square of value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float Squared(this float value)
    {
        return value * value;
    }

    /// <summary>
    /// Returns the square of value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Squared(this int value)
    {
        return value * value;
    }
    #endregion
}
