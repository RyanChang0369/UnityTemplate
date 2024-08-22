using System;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Customizable RNG. Replaces Range.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
[JsonObject(MemberSerialization.OptIn)]
// [JsonConverter(typeof(RNGPatternConverter))]
public class RNGPattern
{
    #region Variables
    /// <summary>
    /// The model used to generate the RNG.
    /// </summary>
    [Tooltip("The model used to generate the RNG.")]
    [SerializeReference]
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    public IRNGModel model;

    /// <summary>
    /// If true, then the Select method has been called and this will return
    /// <see cref="selectedVal"/> when Select is called.
    /// </summary>
    private bool selected = false;

    /// <summary>
    /// The selected value.
    /// </summary>
    private float selectedVal = 0;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a custom RNG using <paramref name="model"/>.
    /// </summary>
    /// <param name="model">The pattern.</param>
    public RNGPattern(IRNGModel model)
    {
        this.model = model;
    }

    /// <summary>
    /// Creates an RNG pattern using the <see cref="SingleRNGModel"/>.
    /// </summary>
    /// <param name="value">The value used by the model.</param>
    public RNGPattern(float value)
    {
        model = new SingleRNGModel(value);
    }

    /// <summary>
    /// Creates an RNG pattern using the <see cref="LinearRNGModel"/>
    /// </summary>
    /// <param name="min">The maximal value of the model.</param>
    /// <param name="max">The minimal value of the model.</param>
    public RNGPattern(float min, float max)
    {
        model = new LinearRNGModel(min, max);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Evaluates the <see cref="RNGPattern"/> and returns the value generated.
    /// </summary>
    /// <returns>The value generated.</returns>
    public float Evaluate() => model.RandomValue;

    /// <summary>
    /// Evaluates the <see cref="RNGPattern"/> once. When this function is called
    /// again, return the return value of this function the first time it was
    /// called.
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
    public void Reset() => selected = false;

    public override string ToString() => "Custom RNG" +
        (selected ? " [Selected]" : "") +
        ": " + model;
    #endregion
}