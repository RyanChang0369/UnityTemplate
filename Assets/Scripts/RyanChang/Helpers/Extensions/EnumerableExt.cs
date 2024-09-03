using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains methods pertaining to C# list, dictionaries, or other IEnumerable
/// classes.
/// </summary>
public static class EnumerableExt
{
    #region Misc Collection
    /// <summary>
    /// Returns true if <see cref="collection"/> is null or contains no
    /// elements.
    /// </summary>
    /// <param name="collection">The collection.</param>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) =>
        collection == null || !collection.Any();

    /// <inheritdoc cref="IsNullOrEmpty{T}(IEnumerable{T})"/>
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) =>
        collection == null || collection.Count == 0;

    /// <summary>
    /// Determines whether <see cref="collection"/> contains any elements.
    /// </summary>
    /// <inheritdoc cref="IsNullOrEmpty{T}(IEnumerable{T})"/>
    public static bool IsEmpty<T>(this ICollection<T> collection) =>
        collection.Count == 0;

    /// <summary>
    /// Determines whether <see cref="collection"/> contains none elements.
    /// </summary>
    /// <inheritdoc cref="IsNullOrEmpty{T}(IEnumerable{T})"/>
    public static bool NotEmpty<T>(this ICollection<T> collection) =>
        collection.Count > 0;

    /// <summary>
    /// Returns true if <paramref name="index"/> is a valid indexer into
    /// <paramref name="collection"/>. If <paramref name="collection"/> is null
    /// or empty, returns false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection to evaluate.</param>
    /// <param name="index">The index to evaluate.</param>
    /// <returns>True if <paramref name="index"/> is a valid indexer into
    /// <paramref name="collection"/>, false otherwise.</returns>
    public static bool IndexInRange<T>(this IEnumerable<T> collection,
        int index)
    {
        if (collection == null)
            return false;

        return index >= 0 && index < collection.Count();
    }

    /// <summary>
    /// Returns true if <paramref name="index"/> is a valid indexer into
    /// <paramref name="array"/>. If <paramref name="array"/> is null
    /// or empty, returns false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array to evaluate.</param>
    /// <param name="index">The index to evaluate.</param>
    /// <returns>True if <paramref name="index"/> is a valid indexer into
    /// <paramref name="array"/>, false otherwise.</returns>
    public static bool IndexInRange<T>(this T[] array, int index)
    {
        if (array == null)
            return false;

        return index >= 0 && index < array.Length;
    }

    /// <summary>
    /// Ensures that <paramref name="index"/> ranges from 0 to <paramref
    /// name="collection"/> count. If it lies outside of that bound, then wrap
    /// it.
    /// </summary>
    public static int WrapAroundLength<T>(this int index,
        ICollection<T> collection)
    {
        return index.WrapAroundLength(collection.Count);
    }

    /// <summary>
    /// Ensures that <paramref name="index"/> ranges from 0 to <paramref
    /// name="length"/>. If it lies outside of that bound, then wrap it.
    /// </summary>
    /// <remarks>
    /// Adapted from https://stackoverflow.com/a/1082938.
    /// </remarks>
    public static int WrapAroundLength(this int index, int length)
    {
        return (index % length + length) % length;
    }

    /// <summary>
    /// Ensures that <paramref name="index"/> ranges from <paramref
    /// name="from"/> to <paramref name="to"/> (including <paramref
    /// name="to"/>). If it lies outside of that bound, then wrap it.
    /// </summary>
    public static int WrapAround(this int index, int from, int to)
    {
        return index.WrapAroundLength(to - from + 1) + from;
    }

    /// <summary>
    /// Alias for <see cref="Enumerable.SequenceEqual"/>, with added checks for
    /// nullity and reference matching.
    /// </summary>
    /// <typeparam name="T">The IEquatable type.</typeparam>
    /// <param name="collection1">The first enumerable.</param>
    /// <param name="collection2">The second enumerable.</param>
    /// <returns></returns>
    public static bool EnumerableMatches<T>(this IEnumerable<T> collection1,
        IEnumerable<T> collection2)
    {
        if (collection1 == collection2)
            return true;
        else if (collection1 == null || collection2 == null)
            return false;

        return collection1.SequenceEqual(collection2);
    }

    /// <inheritdoc cref="EnumerableMatches{T}(IEnumerable{T}, IEnumerable{T})"/>
    /// <param name="comparer">The comparer to use for the IEnumerable.</param>
    public static bool EnumerableMatches<T>(this IEnumerable<T> collection1,
        IEnumerable<T> collection2, IEqualityComparer<T> comparer)
    {
        if (collection1 == collection2)
            return true;
        else if (collection1 == null || collection2 == null)
            return false;

        return collection1.SequenceEqual(collection2, comparer);
    }

    #region Extrema
    /// <summary>
    /// Returns an element in <paramref name="collection"/> with a maximal value
    /// determined by <paramref name="selector"/>.
    /// </summary>
    /// <param name="collection">The collection to operate on.</param>
    /// <param name="selector">Selects the value used for the
    /// comparison.</param>
    /// <typeparam name="T">The type in <paramref
    /// name="collection"/>.</typeparam>
    /// <typeparam name="TSelect">The value used in the comparison.</typeparam>
    public static T WithMaxValue<T, TSelect>(this IEnumerable<T> collection,
        Func<T, TSelect> selector) where TSelect : IComparable<TSelect>
    {
        return collection.Aggregate(
            (a, b) => selector(a).CompareTo(selector(b)) > 0 ? a : b
        );
    }

    /// <inheritdoc cref="WithMaxValue{T, TSelect}(IEnumerable{T}, Func{T,
    /// TSelect})"/>
    /// <summary>
    /// Returns an element in <paramref name="collection"/> with a minimal value
    /// determined by <paramref name="selector"/>.
    /// </summary>
    public static T WithMinValue<T, TSelect>(this IEnumerable<T> collection,
        Func<T, TSelect> selector) where TSelect : IComparable<TSelect>
    {
        return collection.Aggregate(
            (a, b) => selector(a).CompareTo(selector(b)) < 0 ? a : b
        );
    }
    #endregion
    #endregion

    #region List
    /// <summary>
    /// Swaps the values in the collection at <paramref name="indexA"/> and
    /// <paramref name="indexB"/>.
    /// </summary>
    /// <param name="collection">The list/array to operate on.</param>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    public static void Swap<T>(this IList<T> collection, int indexA, int indexB)
    {
        (collection[indexB], collection[indexA]) =
            (collection[indexA], collection[indexB]);
    }

    /// <summary>
    /// Removes everything past (and including) index from.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">List to modify.</param>
    /// <param name="from">Index to remove from.</param>
    public static void TrimToEnd<T>(this IList<T> list, int from)
    {
        while (from < list.Count)
        {
            list.RemoveAt(from);
        }
    }

    /// <summary>
    /// Returns either the element of <paramref name="list"/> at <paramref
    /// name="index"/> if <paramref name="index"/> is within range of <paramref
    /// name="list"/>, or null if it is not.
    /// </summary>
    public static T AtIndexOrNull<T>(this IList<T> list, int index)
        where T : class => AtIndexOrValue(list, index, null);

    /// <summary>
    /// Returns either the element of <paramref name="list"/> at <paramref
    /// name="index"/> if <paramref name="index"/> is within range of <paramref
    /// name="list"/>, or the value of <paramref name="defaultValue"/> if it is
    /// not. 
    /// </summary>
    public static T AtIndexOrValue<T>(this IList<T> list, int index, T defaultValue)
    {
        if (list == null || !list.IndexInRange(index))
        {
            return defaultValue;
        }

        return list[index];
    }

    /// <summary>
    /// If index is a valid index in list, then replace the element at index
    /// with obj. Otherwise, add obj to the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">List to modify.</param>
    /// <param name="obj">Object to add.</param>
    /// <param name="index">Index to add the object at.</param>
    public static void AddOrReplace<T>(this IList<T> list, T obj, int index)
    {
        if (index < list.Count)
            list[index] = obj;
        else
            list.Add(obj);
    }

    /// <summary>
    /// If index is a valid index in list, then replace the element at index
    /// with obj. Otherwise, add obj to the list, buffering new elements with
    /// defaults.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">List to modify.</param>
    /// <param name="obj">Object to add.</param>
    /// <param name="index">Index to add the object at.</param>
    public static void AddOrReplaceWithBuffer<T>(this IList<T> list, T obj,
        int index)
    {
        if (index < list.Count)
            list[index] = obj;
        else
        {
            while (index > list.Count)
            {
                list.Add(default);
            }

            list.Add(obj);
        }
    }
    #endregion

    #region Dictionary
    #region Value Getters
    /// <summary>
    /// Tries to get the value from <paramref name="dictionary"/>. If found,
    /// returns that value. Otherwise, return the value assigned to <paramref
    /// name="defaultValue"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of key in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of value in the dictionary.</typeparam>
    /// <returns>The value from <paramref name="dictionary"/>, or <paramref
    /// name="defaultValue"/>.</returns>
    /// <inheritdoc cref="GetOrCreate{TKey, TValue}(IReadOnlyDictionary{TKey,
    /// TValue}, TKey)"/>
    public static TValue GetValueOrDefault<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> dictionary,
        TKey key, TValue defaultValue = default)
    {
        if (dictionary.TryGetValue(key, out TValue value))
        {
            return value;
        }

        return defaultValue;
    }

    /// <summary>
    /// Tries to get the value from <paramref name="dictionary"/>. If found,
    /// returns that value. Otherwise, return <see cref="null"/>.
    /// </summary>
    /// <returns>
    /// The value from <paramref name="dictionary"/>, or null.
    /// </returns>
    /// <inheritdoc cref="GetValueOrDefault{TKey,
    /// TValue}(IReadOnlyDictionary{TKey, TValue}, TKey, TValue)"/>
    public static TValue GetValueOrNull<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> dictionary,
        TKey key) where TValue : class =>
        GetValueOrDefault(dictionary, key, null);

    /// <summary>
    /// Tries to get the value from <paramref name="dictionary"/>. If found,
    /// returns that value. Otherwise, return <see cref="new()"/>.
    /// </summary>
    /// <returns>
    /// The value from <paramref name="dictionary"/>, or new().
    /// </returns>
    /// <inheritdoc cref="GetValueOrDefault{TKey,
    /// TValue}(IReadOnlyDictionary{TKey, TValue}, TKey, TValue)"/>
    public static TValue GetValueOrNew<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> dictionary,
        TKey key) where TValue : new()
    {
        if (dictionary.TryGetValue(key, out TValue val))
            return val == null ? new() : val;

        return new();
    }
    #endregion

    #region Dictionary List
    /// <summary>
    /// Adds an addition to a list in the dictionary. Creates the list and adds
    /// the addition if the specified key does not exist in the dictionary.
    /// <typeparam name="TKey">The type of key to add.</typeparam>
    /// <typeparam name="TValue">The type of value to add.</typeparam>
    /// <param name="dict">The dictionary to add to.</param>
    /// <param name="key">The specified key.</param>
    /// <param name="value">What to add to the list.</param>
    /// </summary>
    public static void AddToDictList<TKey, TValue>(
        this IReadOnlyDictionary<TKey, List<TValue>> dict,
        TKey key, TValue value)
    {
        dict.GetValueOrNew(key).Add(value);
    }

    /// <summary>
    /// Adds or replaces <paramref name="obj"/> to an internal list in a
    /// dictionary of lists <paramref name="dictionary"/>. See <see
    /// cref="AddOrReplace{T}(IList{T}, T, int)"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of key to add.</typeparam>
    /// <typeparam name="TValue">The type of value to add.</typeparam>
    /// <param name="dict">The dictionary of lists.</param>
    /// <param name="key">The key that targets the list to add to. Must be
    /// non-null.</typeparam>
    /// <param name="obj">What to add to the list.</param>
    /// <param name="index">Index to add the object at.</param>
    /// <param name="buffered">If true, add a buffer to the internal list. See
    /// <see cref="AddOrReplaceWithBuffer{T}(IList{T}, T, int)"/></param>
    public static void AddOrReplaceToDictList<TKey, TValue>(
        this IReadOnlyDictionary<TKey, List<TValue>> dict, TKey key,
        TValue obj, int index, bool buffered = false)
    {
        if (buffered)
        {
            dict.GetValueOrNew(key).AddOrReplaceWithBuffer(obj, index);
        }
        else
        {
            dict.GetValueOrNew(key).AddOrReplace(obj, index);
        }
    }
    #endregion
    #endregion
}
