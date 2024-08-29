using System;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// <see cref="JsonConverter"/> for <see cref="Range"/>.
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public class RangeConverter : JsonConverter<Range>
{
    public override Range ReadJson(JsonReader reader, System.Type objectType,
        Range existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        reader.ReadProperty("pattern", out Range.RangePattern pattern);

        switch (pattern)
        {
            case Range.RangePattern.Single:
                return new(reader.ReadProperty<float>("value"));
            case Range.RangePattern.Linear:
                return new(
                    reader.ReadProperty<float>("min"),
                    reader.ReadProperty<float>("max")
                );
            case Range.RangePattern.Curves:
                return new(
                    reader.ReadProperty<AnimationCurve>("curve")
                );
            default:
                throw new NotImplementedException();
        }
    }

    public override void WriteJson(JsonWriter writer, Range value,
        JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WriteProperty("pattern", value.mode.GetName());

        switch (value.mode)
        {
            case Range.RangePattern.Single:
                writer.WriteProperty("value", value.singleValue);
                break;
            case Range.RangePattern.Linear:
                writer.WriteProperty("min", value.scalarMin);
                writer.WriteProperty("max", value.scalarMax);
                break;
            case Range.RangePattern.Curves:
                writer.WriteProperty("curve", value.curve.ToJson());
                break;
            case Range.RangePattern.Perlin:
                writer.WriteProperty("crawlSpeed", value.perlinCrawlSpeed);
                break;
        }

        writer.WriteEndObject();
    }
}