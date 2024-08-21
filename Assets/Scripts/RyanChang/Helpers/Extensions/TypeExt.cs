using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Contains functions that extend types and reflection.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public static class TypeExt
{
    #region Query
    #region Derived Types
    /// <summary>
    /// Returns all types that can be derived from <typeparamref name="T"/>
    /// based on the provided assembly.
    /// </summary>
    /// <param name="assembly">The assembly to search for the derived
    /// types.</param>
    /// <param name="type">The type to search for the derived types.</param>
    /// <returns>The derived types.</returns>
    public static IEnumerable<Type> FindAllDerivedTypes(Assembly assembly,
        Type type)
    {
        return assembly
            .GetTypes()
            .Where(t =>
                t != type &&
                type.IsAssignableFrom(t)
                );
    }

    /// <inheritdoc cref="FindAllDerivedTypes{T}(Assembly)"/>
    /// <typeparam name="T">The type to search for the derived types.</typeparam>
    public static IEnumerable<Type> FindAllDerivedTypes<T>(Assembly assembly)
        => FindAllDerivedTypes(assembly, typeof(T));

    /// <inheritdoc cref="FindAllDerivedTypes(Assembly, Type)"/>
    public static IEnumerable<Type> FindAllDerivedTypes(Type type)
        => FindAllDerivedTypes(Assembly.GetAssembly(type), type);

    /// <inheritdoc cref="FindAllDerivedTypes{T}()"/>
    public static IEnumerable<Type> FindAllDerivedTypes<T>()
        => FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
    #endregion

    #region Field Value
    /// <summary>
    /// Finds and returns the field value for <paramref name="obj"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The instance of the object.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="flags">Any flags.</param>
    /// <returns></returns>
    public static T GetFieldValue<T>(this object obj, string fieldName,
        BindingFlags flags = BindingFlags.Default)
    {
        var field = obj.GetType().GetField(fieldName, flags);

        return (T)field.GetValue(obj);
    }

    /// <summary>
    /// Tries to find the field value for <paramref name="obj"/>.
    /// </summary>
    /// <param name="fieldValue">If found, the field value will be placed here.
    /// Otherwise, null will be placed here.</param>
    /// <returns>True if field value was found.</returns>
    /// <inheritdoc cref="GetFieldValue{T}(object, string, BindingFlags)"/>
    public static bool TryGetFieldValue<T>(this object obj, string fieldName,
        BindingFlags flags, out T fieldValue)
    {
        var field = obj.GetType().GetField(fieldName, flags);

        if (field == null)
        {
            fieldValue = default;
            return false;
        }

        try
        {
            fieldValue = (T)field.GetValue(obj);
            return true;
        }
        catch (System.InvalidCastException)
        {
            fieldValue = default;
            return false;
        }
    }
    #endregion
    #endregion
}
