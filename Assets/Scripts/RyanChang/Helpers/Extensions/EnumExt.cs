using System;

/// <summary>
/// Extension methods for <see cref="System.Enum"/>
/// </summary>
/// 
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
public static class EnumExt
{
    #region Flags
    /// <summary>
    /// Determines if any of the flags in <paramref name="flags"/> are set in
    /// <paramref name="enum"/>.
    /// </summary>
    /// <typeparam name="TEnum">Any Enum type.</typeparam>
    /// <param name="enum">The Enum to test.</param>
    /// <param name="flags">The flags to test against.</param>
    /// <returns></returns>
    public static bool HasAnyFlag<TEnum>(this TEnum @enum, TEnum flags)
        where TEnum : struct, Enum
    {
        return (Convert.ToInt64(@enum) & Convert.ToInt64(flags)) != 0;
    }
    #endregion

    #region System.Enum Generics
    /// <summary>
    /// Determines the name of the enum value.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <typeparam name="TEnum">Any enum type.</typeparam>
    /// <returns></returns>
    public static object GetName<TEnum>(this TEnum enumValue)
        where TEnum : struct, Enum => Enum.GetName(typeof(TEnum), enumValue);

    /// <summary>
    /// Retrieves the values from the <typeparamref name="TEnum"/> enum type.
    /// </summary>
    /// <typeparam name="TEnum">Any enum type.</typeparam>
    /// <returns></returns>
    public static Array GetValues<TEnum>()
        where TEnum : struct, Enum => Enum.GetValues(typeof(TEnum));
    #endregion
}
