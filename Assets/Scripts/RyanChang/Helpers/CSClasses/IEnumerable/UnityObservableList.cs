using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

/// <summary>
/// A list that is aware of any changes created upon itself. Use this as a Unity
/// Editor compatible ObservableCollection.
/// </summary>
///
/// <remarks>
/// Authors: Ryan Chang (2024)
/// </remarks>
[Serializable]
public class UnityObservableList<T> : IList<T>, INotifyCollectionChanged
{
    [SerializeField]
    private List<T> internalList;

    #region IList Implementation
    public T this[int index]
    {
        get => internalList[index];
        set
        {
            internalList[index] = value;
            CollectionChanged.Invoke(
                this,
                new(
                    NotifyCollectionChangedAction.Replace,
                    changedItem: value,
                    index: index
                )
            );
        }
    }

    public int Count => internalList.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        internalList.Add(item);
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Add,
                newItem: item,
                oldItem: null
            )
        );
    }

    public void Clear()
    {
        internalList.Clear();
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Reset
            )
        );
    }

    public bool Contains(T item)
    {
        return internalList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        internalList.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return internalList.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return internalList.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        internalList.Insert(index, item);
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Add,
                newItem: item,
                oldItem: null,
                index: index
            )
        );
    }

    public bool Remove(T item)
    {
        bool removed = internalList.Remove(item);

        if (removed)
        {
            CollectionChanged.Invoke(
                this,
                new(
                    NotifyCollectionChangedAction.Remove,
                    newItem: null,
                    oldItem: item
                )
            );
        }

        return removed;
    }

    public void RemoveAt(int index)
    {
        var item = internalList[index];
        internalList.RemoveAt(index);

        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Remove,
                newItem: null,
                oldItem: item,
                index: index
            )
        );
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return internalList.GetEnumerator();
    }
    #endregion

    #region INotifyCollectionChanged Implementation
    public event NotifyCollectionChangedEventHandler CollectionChanged;
    #endregion

    #region Constructors
    public UnityObservableList(IEnumerable<T> collection)
    {
        internalList = new(collection);
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Add,
                // Create copy of internalList so people don't mess with it.
                newItems: new List<T>(collection),
                oldItems: null
            )
        );
    }

    public UnityObservableList(int capacity)
    {
        internalList = new(capacity);
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Add,
                // Create copy of internalList so people don't mess with it.
                newItems: new List<T>(capacity),
                oldItems: null
            )
        );
    }
    #endregion

    #region Methods
    public void AddRange(IEnumerable<T> collection)
    {
        int startIndex = internalList.Count;
        internalList.AddRange(collection);
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Add,
                newItems: new List<T>(collection),
                oldItems: null,
                startingIndex: startIndex
            )
        );
    }

    public ReadOnlyCollection<T> AsReadOnly() => internalList.AsReadOnly();

    public int BinarySearch(int index, int count, T item, IComparer<T> comparer) =>
        internalList.BinarySearch(index, count, item, comparer);

    public int BinarySearch(T item) =>
        internalList.BinarySearch(item);

    public int BinarySearch(T item, IComparer<T> comparer) =>
        internalList.BinarySearch(item, comparer);

    public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) =>
        internalList.ConvertAll(converter);

    public void CopyTo(T[] array) => internalList.CopyTo(array);

    public void CopyTo(int index, T[] array, int arrayIndex, int count) =>
        internalList.CopyTo(index, array, arrayIndex, count);

    public bool Exists(Predicate<T> match) =>
        internalList.Exists(match);

    public T Find(Predicate<T> match) =>
        internalList.Find(match);

    public List<T> FindAll(Predicate<T> match) =>
        internalList.FindAll(match);

    public int FindIndex(int startIndex, int count, Predicate<T> match) =>
        internalList.FindIndex(startIndex, count, match);

    public int FindIndex(int startIndex, Predicate<T> match) =>
        internalList.FindIndex(startIndex, match);

    public int FindIndex(Predicate<T> match) =>
        internalList.FindIndex(match);

    public T FindLast(Predicate<T> match) =>
        internalList.FindLast(match);

    public int FindLastIndex(int startIndex, int count, Predicate<T> match) =>
        internalList.FindLastIndex(startIndex, count, match);

    public int FindLastIndex(int startIndex, Predicate<T> match) =>
        internalList.FindLastIndex(startIndex, match);

    public int FindLastIndex(Predicate<T> match) =>
        internalList.FindLastIndex(match);

    public void ForEach(Action<T> action) =>
        internalList.ForEach(action);

    public List<T> GetRange(int index, int count) =>
        internalList.GetRange(index, count);

    public int IndexOf(T item, int index, int count) =>
        internalList.IndexOf(item, index, count);

    public int IndexOf(T item, int index) => internalList.IndexOf(item, index);

    public void InsertRange(int index, IEnumerable<T> collection)
    {
        internalList.InsertRange(index, collection);
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Add,
                // Create copy of internalList so people don't mess with it.
                newItems: new List<T>(collection),
                oldItems: null,
                startingIndex: index
            )
        );
    }

    public int LastIndexOf(T item) =>
        internalList.LastIndexOf(item);

    public int LastIndexOf(T item, int index) =>
        internalList.LastIndexOf(item, index);

    public int LastIndexOf(T item, int index, int count) =>
        internalList.LastIndexOf(item, index, count);

    public int RemoveAll(Predicate<T> match)
    {
        var removed = internalList.FindAll(match);
        int count = internalList.RemoveAll(match);

        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Remove,
                newItems: null,
                oldItems: removed
            )
        );

        return count;
    }

    public void RemoveRange(int index, int count)
    {
        var removed = internalList.GetRange(index, count);
        internalList.RemoveRange(index, count);

        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Remove,
                newItems: null,
                oldItems: removed
            )
        );
    }
    public void Reverse(int index, int count)
    {
        List<T> old = internalList.GetRange(index, count);
        internalList.Reverse(index, count);
        
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Move,
                newItems: internalList.GetRange(index, count),
                oldItems: old,
                startingIndex: index
            )
        );
    }

    public void Reverse() => Reverse(0, Count - 1);

    public void Sort(Comparison<T> comparison)
    {
        List<T> old = new(internalList);
        internalList.Sort(comparison);
        
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Move,
                newItems: new List<T>(internalList),
                oldItems: old
            )
        );
    }

    public void Sort(int index, int count, IComparer<T> comparer)
    {
        List<T> old = internalList.GetRange(index, count);
        internalList.Sort(index, count, comparer);
        
        CollectionChanged.Invoke(
            this,
            new(
                NotifyCollectionChangedAction.Move,
                newItems: internalList.GetRange(index, count),
                oldItems: old,
                startingIndex: index
            )
        );
    }

    public void Sort() => Sort(Comparer<T>.Default);

    public void Sort(IComparer<T> comparer) => Sort(0, Count - 1, comparer);

    public T[] ToArray() => internalList.ToArray();

    public void TrimExcess() => internalList.TrimExcess();
    
    public bool TrueForAll(Predicate<T> match) => internalList.TrueForAll(match);
    #endregion
}