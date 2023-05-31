using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains methods pertaining to C# list, dictionaries, or other IEnumerable
/// classes.
/// </summary>
public static class EnumerableExt
{
    /// <summary>
    /// Returns true if the IEnumerable is null or contains no elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable">IEnumerable to check.</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }

        /// <summary>
    /// Adds addition to a dictionary with a list as its value.
    /// <typeparam name="TKey">The key.</typeparam>
    /// <typeparam name="TMem">The member of the list within the dictionary.</typeparam>
    /// <param name="dict">The dictionary to add to.</param>
    /// <param name="key">The key that holds the list to add addition to.</param>
    /// <param name="addition">What to add to the internal list.</param>
    /// </summary>
    public static void AddToDictList<TKey, TMem>(this Dictionary<TKey, List<TMem>> dict, TKey key, TMem addition)
    {
        if (!dict.ContainsKey(key))
            dict[key] = new List<TMem>();

        dict[key].Add(addition);
    }

    #region List
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
    /// If index is a valid index in list, then replace the element at index
    /// with obj. Otherwise, add obj to the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">List to modify.</param>
    /// <param name="obj">Object to add.</param>
    /// <param name="index">Index to add th object at.</param>
    public static void AddOrReplace<T>(this IList<T> list, T obj, int index)
    {
        if (index < list.Count)
            list[index] = obj;
        else
            list.Add(obj);
    }
    #endregion
}