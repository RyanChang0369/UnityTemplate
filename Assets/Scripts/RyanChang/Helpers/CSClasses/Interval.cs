using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Interval
{
    #region Variables
    /// <summary>
    /// The RNG pattern used.
    /// </summary>
    [Tooltip("The RNG pattern used.")]
    [FormerlySerializedAs("range")]
    public RNGPattern pattern;

    /// <summary>
    /// The timer used for the interval.
    /// </summary>
    [Tooltip("The timer used for the interval.")]
    [ReadOnly]
    public float timer;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs an interval with a single range.
    /// </summary>
    /// <param name="value">The value to set to.</param>
    public Interval(float value)
    {
        pattern = new(value);
    }

    /// <summary>
    /// Creates a new interval with a linear range with max and min.
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximal value</param>
    public Interval(float min, float max)
    {
        pattern = new(min, max);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Updates the interval.
    /// </summary>
    /// <param name="deltaTime">Amount of time that has passed since last
    /// update.</param>
    /// <returns>True if the interval has reached its internal timer. Most
    /// likely Time.deltaTime.</returns>
    public bool UpdateInterval(float deltaTime)
    {
        if (timer >= pattern.Select())
        {
            pattern.Reset();
            timer = 0;
            return true;
        }
        else
        {
            timer += deltaTime;
            return false;
        }
    }

    /// <summary>
    /// Ensure that the next call to UpdateInterval returns true
    /// by setting the timer to its maximum selected value.
    /// </summary>
    public void WindToEnd()
    {
        timer = pattern.Select() + 1;
    }

    /// <summary>
    /// Reset timer to zero without resetting the timer's
    /// time selection.
    /// </summary>
    public void WindToStart()
    {
        timer = 0;
    }

    /// <summary>
    /// Does WindToStart (resets timer), then resets the range
    /// (selects a new time for the timer).
    /// </summary>
    public void Restart()
    {
        WindToStart();
        pattern.Reset();
    }
    #endregion
}
