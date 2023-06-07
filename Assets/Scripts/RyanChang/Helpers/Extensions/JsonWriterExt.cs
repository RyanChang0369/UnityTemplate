using System;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Contains methods pertaining to Newtonsoft's JsonWriter.
/// </summary>
public static class JsonWriterExt
{
    /// <summary>
    /// Writes the property name followed by the value of the property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="writer">The JsonWriter.</param>
    /// <param name="name">Name of the property used to serialize it.</param>
    /// <param name="property">The property to write.</param>
    public static void WriteProperty<T>(this JsonWriter writer, string name,
        T property)
    {
        writer.WritePropertyName(name);
        writer.WriteValue(property);
    }
}