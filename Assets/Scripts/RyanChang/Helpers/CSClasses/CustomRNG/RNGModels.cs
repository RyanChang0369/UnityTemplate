using System;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

#region Interface
/// <summary>
/// Can be attached to a <see cref="RNGPattern"/> object to customize the
/// behavior of the pattern.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public interface IRNGModel
{
    /// <summary>
    /// The generated random number.
    /// </summary>
    public float RandomValue { get; }
}
#endregion

#region Single
/// <summary>
/// A <see cref="IRNGModel"/> that always returns a fixed value.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public struct SingleRNGModel : IRNGModel
{
    #region Variables/Properties
    /// <summary>
    /// The singlet value.
    /// </summary>
    [Tooltip("The singlet value.")]
    [SerializeField, JsonProperty]
    private float value;

    public readonly float RandomValue => value;
    #endregion

    #region Constructors
    [JsonConstructor]
    public SingleRNGModel(float value)
    {
        this.value = value;
    }
    #endregion
}
#endregion

#region Linear
/// <summary>
/// A <see cref="IRNGModel"/> that generates a random value that lies between
/// <see cref="min"/> and <see cref="max"/>.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public struct LinearRNGModel : IRNGModel
{
    #region Variables/Properties
    /// <summary>
    /// The minimal bounds of the random generation.
    /// </summary>
    [Tooltip("The minimal bounds of the random generation.")]
    [SerializeField, JsonProperty]
    private float min;

    /// <summary>
    /// The maximal bounds of the random generation.
    /// </summary>
    [Tooltip("The maximal bounds of the random generation.")]
    [SerializeField, JsonProperty]
    private float max;

    public readonly float RandomValue => RNGExt.RandomFloat(min, max);
    #endregion

    #region Constructors
    [JsonConstructor]
    public LinearRNGModel(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
    #endregion
}
#endregion

#region Curve
/// <summary>
/// A <see cref="IRNGModel"/> that uses an animation curve to generate random
/// values. This allows for the bias of arbitrary random distribution.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public struct CurveRNGModel : IRNGModel
{
    #region Variables/Properties
    /// <summary>
    /// The animation curve used to generate the random value.
    /// </summary>
    [Tooltip("The animation curve used to generate the random value.")]
    [SerializeField, JsonProperty]
    private AnimationCurve curve;

    /// <summary>
    /// Whether or not to use <see cref="floor"/>.
    /// </summary>
    [Tooltip("Whether or not to use floor.")]
    [SerializeField, JsonProperty]
    private bool enableFloor;

    /// <summary>
    /// The absolute minimal value that can be generated by this pattern.
    /// </summary>
    [Tooltip("The absolute minimal value that can be generated by this pattern.")]
    [SerializeField]
    [HideIf(nameof(enableFloor))]
    [AllowNesting]
    private float floor;

    /// <summary>
    /// Whether or not to use <see cref="ceiling"/>.
    /// </summary>
    [Tooltip("Whether or not to use ceiling.")]
    [SerializeField, JsonProperty]
    private bool enableCeiling;

    /// <summary>
    /// The absolute maximal value that can be generated by this pattern.
    /// </summary>
    [Tooltip("The absolute maximal value that can be generated by this pattern.")]
    [SerializeField]
    [HideIf(nameof(enableCeiling))]
    [AllowNesting]
    private float ceiling;

    public readonly float RandomValue => curve.RandomValueOnCurve(floor, ceiling);
    #endregion

    #region Properties
    /// <summary>
    /// The absolute minimal value that can be generated by this pattern. Set to
    /// NaN to disable.
    /// </summary>
    [JsonProperty]
    public float Floor
    {
        readonly get => enableFloor ? floor : float.NaN;
        set
        {
            floor = value;
            enableFloor = float.IsFinite(floor);
        }
    }

    /// <summary>
    /// The absolute maximal value that can be generated by this pattern. Set to
    /// NaN to disable.
    /// </summary>
    [JsonProperty]
    public float Ceiling
    {
        readonly get => enableCeiling ? ceiling : float.NaN;
        set
        {
            ceiling = value;
            enableCeiling = float.IsFinite(ceiling);
        }
    }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a RNG pattern that uses a curve. Caps the output of the curve to
    /// be always between <paramref name="floor"/> and <paramref
    /// name="ceiling"/>.
    /// </summary>
    /// <param name="curve">The animation curve used to generate the random
    /// value.</param>
    /// <param name="floor">The absolute minimal value that can be generated by
    /// this pattern.</param>
    /// <param name="ceiling">The absolute maximal value that can be generated
    /// by this pattern.</param>
    [JsonConstructor]
    public CurveRNGModel(AnimationCurve curve, float floor, float ceiling)
    {
        this.curve = curve;
        this.floor = floor;
        this.ceiling = ceiling;

        enableFloor = float.IsFinite(floor);
        enableCeiling = float.IsFinite(ceiling);
    }

    /// <summary>
    /// Creates a RNG pattern that uses a curve. Disables any bounds to the
    /// output of the curve.
    /// </summary>
    /// <inheritdoc cref="CurveRNGModel(AnimationCurve, float, float)"/>
    public CurveRNGModel(AnimationCurve curve) :
        this(curve, float.NaN, float.NaN)
    { }
    #endregion

    #region Methods
    public override readonly string ToString() =>
        $"{GetType().Name} [{curve.length} keys, " +
        (float.IsFinite(floor) ? $"floor {floor}" : "no floor") +
        ", " +
        (float.IsFinite(ceiling) ? $"ceil {ceiling}" : "no ceil") +
        "]";
    #endregion
}
#endregion

#region Perlin
/// <summary>
/// A <see cref="IRNGModel"/> that generates a random value using perlin
/// noise. Perlin noise is special in that it generates smooth randomization,
/// useful for things like Brownian movement.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public struct PerlinRNGModel : IRNGModel
{
    #region Variables/Properties
    /// <summary>
    /// The speed at which the perlin generator moves at.
    /// </summary>
    [Tooltip("The speed at which the perlin generator moves at.")]
    [SerializeField, JsonProperty]
    private float crawlSpeed;

    /// <summary>
    /// The position of the perlin generation.
    /// </summary>
    [Tooltip("The position of the perlin generation.")]
    [SerializeField, JsonProperty]
    private Vector2 crawlPos;

    /// <summary>
    /// The direction of the perlin generation.
    /// </summary>
    [Tooltip("The direction of the perlin generation.")]
    [SerializeField, JsonProperty]
    private Vector2 crawlDir;

    public float RandomValue
    {
        get
        {
            crawlPos += crawlDir * crawlSpeed;
            return Mathf.PerlinNoise(crawlPos.x, crawlPos.y);
        }
    }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a perlin RNG pattern.
    /// </summary>
    /// <param name="crawlSpeed">The speed at which the perlin generator moves
    /// at.</param>
    /// <param name="crawlPos">The position of the perlin generation.</param>
    /// <param name="crawlDir">The direction of the perlin generation.</param>
    [JsonConstructor]
    public PerlinRNGModel(float crawlSpeed, Vector2 crawlPos, Vector2 crawlDir)
    {
        this.crawlSpeed = crawlSpeed;
        this.crawlPos = crawlPos;
        this.crawlDir = crawlDir;
    }

    /// <inheritdoc cref="PerlinRNGModel(float, Vector2, Vector2)"/>
    public PerlinRNGModel(float crawlSpeed = 0.1f) : this(crawlSpeed,
        RNGExt.RandomVector2(1000),
        RNGExt.OnCircle())
    { }
    #endregion

    #region Methods
    public override readonly string ToString() =>
        $"{GetType().Name} [speed {crawlSpeed}, " +
        $"pos {crawlPos}, " +
        $"dir {crawlDir}]";
    #endregion
}
#endregion
