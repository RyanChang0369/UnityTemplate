using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public static Testing singleton;

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
        this.InstantiateSingleton(ref singleton);

        test = new();
        test.Add(new(15, this, "Start"), new(17));
        test.Add(new(0, this, "Important"), new(0.5f, 0.5f));
        test.Add(new(0, 17, "Important"), new(50, 0.2f));
        
        test.Remove(new(0, this, "Important"));

        foreach (var item in test.chain)
        {
            print(item.Key);
        }

        StringBuilder sb = new();
        sb.AppendTitle("Sample Text");
        sb.AppendLineAutoNewLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            prefix: "|=|", suffix: "|=|");
        sb.AppendHeader("Sample Header");
        sb.AppendLineAutoNewLine("For example, the call to the String.Concat method in the following C# example appears to change the value of a string variable named value. In fact, the Concat method returns a value object that has a different value and address from the value object that was passed to the method. Note that the example must be compiled using the /unsafe compiler option.");
        // File.WriteAllText($"{Application.dataPath}/test.txt", sb.ToString());

        this.RequireComponent(out CircleCollider2D other);
    }
}