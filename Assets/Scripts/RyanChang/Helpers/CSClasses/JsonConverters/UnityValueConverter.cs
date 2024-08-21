using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>

/// </summary>
/// <summary>
/// A unity value converter that uses Unity's JsonUtility to convert
/// Unity-specific objects and structs. Required to get Newtonsoft to play
/// nicely with those Unity objects.
///
/// <br/>
///
/// Authors: Ryan Chang (2024)
/// </summary>
public class UnityValueConverter : JsonConverter
{
    /// <summary>
    /// Returns true if we can convert this objectType.
    /// </summary>
    /// <param name="objectType">The type we are checking for if we can
    /// convert.</param>
    /// <returns></returns>
    public override bool CanConvert(Type objectType)
    {
        // Am I missing a type? Add it here.
        return    objectType == typeof(Vector2)
               || objectType == typeof(Vector2Int)
               || objectType == typeof(Vector3)
               || objectType == typeof(Vector3Int)
               || objectType == typeof(Vector4)
               || objectType == typeof(Quaternion)
               || objectType == typeof(Color)
               || objectType == typeof(Color32)
               || objectType == typeof(Rect)
               || objectType == typeof(RectInt)
               || objectType == typeof(Bounds)
               || objectType == typeof(BoundsInt);
    }

    public override object ReadJson(JsonReader reader, Type objectType,
        object existingValue, JsonSerializer serializer)
    {
        if (objectType == typeof(Vector2))
        {
            var arr = reader.ReadArray<float>();
            return new Vector2(arr[0], arr[1]);
        }
        else if (objectType == typeof(Vector2Int))
        {
            var arr = reader.ReadArray<int>();
            return new Vector2Int(arr[0], arr[1]);
        }
        else if (objectType == typeof(Vector3))
        {
            var arr = reader.ReadArray<float>();
            return new Vector3(arr[0], arr[1], arr[2]);
        }
        else if (objectType == typeof(Vector3Int))
        {
            var arr = reader.ReadArray<int>();
            return new Vector3Int(arr[0], arr[1], arr[2]);
        }
        else if (objectType == typeof(Vector4))
        {
            var arr = reader.ReadArray<float>();
            return new Vector4(arr[0], arr[1], arr[2], arr[3]);
        }
        else if (objectType == typeof(Quaternion))
        {
            var arr = reader.ReadArray<float>();
            return new Quaternion(arr[0], arr[1], arr[2], arr[3]);
        }
        else if (objectType == typeof(Color))
        {
            ColorUtility.TryParseHtmlString(
                reader.Value.ToString(),
                out Color color
            );
            return color;
        }
        else if (objectType == typeof(Color32))
        {
            ColorUtility.TryParseHtmlString(
                reader.Value.ToString(),
                out Color color
            );
            return (Color32)color;
        }
        else if (objectType == typeof(Rect))
        {
            var arr = reader.ReadArray<float>();
            return new Rect(arr[0], arr[1], arr[2], arr[3]);
        }
        else if (objectType == typeof(RectInt))
        {
            var arr = reader.ReadArray<int>();
            return new RectInt(arr[0], arr[1], arr[2], arr[3]);
        }
        else if (objectType == typeof(Bounds))
        {
            Assert.AreEqual(reader.TokenType, JsonToken.StartObject);

            return new Bounds(
                reader.ReadProperty<Vector2>(nameof(Bounds.center)),
                reader.ReadProperty<Vector2>(nameof(Bounds.size))
            );
        }
        else if (objectType == typeof(BoundsInt))
        {
            Assert.AreEqual(reader.TokenType, JsonToken.StartObject);

            return new Bounds(
                reader.ReadProperty<Vector2>(nameof(BoundsInt.center)),
                reader.ReadProperty<Vector2>(nameof(BoundsInt.size))
            );
        }
        else
        {
            return JsonUtility.FromJson(reader.Value.ToString(), objectType);
        }
    }

    public override void WriteJson(JsonWriter writer, object value,
        JsonSerializer serializer)
    {
        switch (value)
        {
            case Vector2 v:
                writer.WriteArray(v[0], v[1]);
                break;
            case Vector2Int v:
                writer.WriteArray(v[0], v[1]);
                break;
            case Vector3 v:
                writer.WriteArray(v[0], v[1], v[2]);
                break;
            case Vector3Int v:
                writer.WriteArray(v[0], v[1], v[2]);
                break;
            case Vector4 v:
                writer.WriteArray(v[0], v[1], v[2], v[3]);
                break;
            case Quaternion q:
                writer.WriteArray(q[0], q[1], q[2], q[3]);
                break;
            case Color c:
                writer.WriteValue('#' + ColorUtility.ToHtmlStringRGBA(c));
                break;
            case Color32 c:
                writer.WriteValue('#' + ColorUtility.ToHtmlStringRGBA(c));
                break;
            case Rect r:
                writer.WriteArray(r.x, r.y, r.width, r.height);
                break;
            case RectInt r:
                writer.WriteArray(r.x, r.y, r.width, r.height);
                break;
            case Bounds b:
                writer.WriteStartObject();
                writer.WriteProperty(nameof(Bounds.center), b.center.ToJson());
                writer.WriteProperty(nameof(Bounds.size), b.size.ToJson());
                writer.WriteEndObject();
                break;
            case BoundsInt b:
                writer.WriteStartObject();
                writer.WriteProperty(nameof(BoundsInt.center), b.center.ToJson());
                writer.WriteProperty(nameof(BoundsInt.size), b.size.ToJson());
                writer.WriteEndObject();
                break;
            default:
                writer.WriteValue(JsonUtility.ToJson(value));
                break;
        }
    }
}
