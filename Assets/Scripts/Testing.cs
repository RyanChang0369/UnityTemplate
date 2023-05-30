using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public ModifierChain test;

    // [FixedList, NonReorderable]
    public FixedList<Modifier> testFixedList = new()
    {
        new(15, 62),
        new(14, 98),
        new(0, 1),
        new(15, 62),
        new(14, 98),
        new(0, 1),
        new(15, 62),
        new(14, 98),
        new(0, 1),
    };

    public List<Modifier> testActualList = new()
    {
        new(15, 62),
        new(14, 98),
        new(0, 1),
        new(15, 62),
        new(14, 98),
        new(0, 1),
        new(15, 62),
        new(14, 98),
        new(0, 1),
    };

    public Range testRange = new(69, 420);

    private void Start()
    {
        test = new();
        test.Add(new(15, this, "Start"), new(17));
        test.Add(new(0, this, "Important"), new(0.5f, 0.5f));
        test.Add(new(0, 17, "Important"), new(50, 0.2f));
        
        test.Remove(new(0, this, "Important"));

        foreach (var item in test.chain)
        {
            print(item.Key);
        }
    }
}