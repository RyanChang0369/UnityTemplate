using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Defines a range of values to be used as a bounds for random number
/// generation. This class breaks all sorts of OOP practices, in order for it to
/// be compatible with the unity editor.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
[JsonConverter(typeof(RangeConverter))]
public class Range
{
    /// <summary>
    /// Determines the mode the RNG uses.
    /// </summary>
    public enum RangePattern
    {
        Single,
        Linear,
        Curves,
        Perlin
    }

    [Tooltip("The range pattern to select:\n" +
        "Option 1: Single. A fixed single value is selected.\n" +
        "Option 2: Linear. Randomly select between 2\n\tvalues.\n" +
        "Option 3: Curves. Uses animation curve to select\n\tvalues.\n" +
        "Option 4: Perlin. Uses Perlin noise to select values.\n" +
        "\tPerlin noise is special in that it generates a\n" +
        "\tsmooth randomization, useful for things like\n" +
        "\tBrownian movement.")]
    [FormerlySerializedAs("rangePattern")]
    public RangePattern mode;

    // [Header("Option 1: Single value as float.")]
    public float singleValue = 0;

    // [Header("Option 2: Specify values as float.")]
    public float scalarMin = -1;
    public float scalarMax = 1;

    // [Header("Option 3: Specify values as curves.")]
    public AnimationCurve curve;

    // [Header("Option 4: Specify forces with Perlin Noise")]
    public float perlinCrawlSpeed = 0.1f;
    private Vector2 crawlPos = RNGExt.RandomVector2(1000);
    private Vector2 crawlDir = RNGExt.OnCircle();

    // [Header("Common to Options 2 and 3")]
    [Tooltip("Modifies the base range to include a multiplication followed by an addition.")]
    [FormerlySerializedAs("modifer")]
    public Modifier modifier = new(0, 1);

    /// <summary>
    /// If true, then the range is unfolded in the inspector.
    /// </summary>
    public bool unfoldedInInspector;

    /// <summary>
    /// If true, then the Select method has been called and Range will return
    /// selectedVal when Select is called.
    /// </summary>
    private bool selected = false;

    /// <summary>
    /// The selected value.
    /// </summary>
    private float selectedVal = 0;

    /// <summary>
    /// Constructs a single range.
    /// </summary>
    /// <param name="value">The value to set to.</param>
    public Range(float value) : this(RangePattern.Single)
    {
        singleValue = value;
        scalarMax = value;
        scalarMin = value;
    }

    /// <summary>
    /// Creates a new linear range with max and min.
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximal value</param>
    public Range(float min, float max) : this(RangePattern.Linear)
    {
        scalarMax = max;
        scalarMin = min;
    }

    /// <summary>
    /// Creates a new curve range with the specified <paramref name="curve"/>.
    /// </summary>
    /// <param name="curve">The animation curve used to generate the random
    /// values.</param>
    public Range(AnimationCurve curve) : this(RangePattern.Curves)
    {
        this.curve = curve;
    }

    /// <summary>
    /// Creates new range with desired pattern.
    /// </summary>
    /// <param name="pattern">Pattern to use.</param>
    public Range(RangePattern pattern)
    {
        mode = pattern;
        crawlPos = RNGExt.RandomVector2(1000);
        crawlDir = RNGExt.OnCircle();
    }

    /// <summary>
    /// Evaluates the range and returns the value generated.
    /// </summary>
    /// <returns>The value generated.</returns>
    public float Evaluate() => mode switch
    {
        RangePattern.Single => singleValue,
        RangePattern.Linear => RNGExt.RandomFloat(scalarMin, scalarMax),
        RangePattern.Curves => modifier.Modify(curve.Evaluate(RNGExt.RandomFloat())),
        RangePattern.Perlin => EvalForPerlin(),
        _ => throw new NotImplementedException(),
    };
    
    private float EvalForPerlin()
    {
        crawlPos += crawlDir * perlinCrawlSpeed;
        return modifier.Modify(Mathf.PerlinNoise(crawlPos.x, crawlPos.y));
    }

    /// <summary>
    /// Evaluates the range once. When this function is called again, return the return value
    /// of this function the first time it was called.
    /// </summary>
    /// <returns>The value generated.</returns>
    public float Select()
    {
        if (!selected)
        {
            selected = true;
            selectedVal = Evaluate();
        }

        return selectedVal;
    }

    /// <summary>
    /// Resets the selection
    /// </summary>
    public void Reset()
    {
        selected = false;
    }

    public override string ToString()
    {
        return mode switch
        {
            RangePattern.Single => $"{base.ToString()}, Single Value, {singleValue}",
            RangePattern.Linear => $"{base.ToString()}, Linear, [{scalarMin}, {scalarMax}]",
            RangePattern.Curves => $"{base.ToString()}, Curve, {curve}",
            RangePattern.Perlin => $"{base.ToString()}, Perlin, Crawl Speed = {perlinCrawlSpeed}",
            _ => base.ToString(),
        };
    }
}
