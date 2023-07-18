using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public static Testing singleton;

    public ModifierChain test;

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

    public enum TestEnum
    {
        NotApplicable = -1,
        NotValid,
        Valid,
        PartiallyValid,
        Mould = 21
    }

    public EnumDictionary<TestEnum, GameObject> testEnumDictionary = new();

    public UnityDictionary<string, int> testUnityDict = new();

    public Selector<float> testSelector;

    public Range testRange = new(69, 420);

    private void Start()
    {
        Dictionary<string, List<float>> test = new();

        test.AddToDictList("Key", 20.5f);
        test.AddToDictList("Key", 0.5f);
    }
}