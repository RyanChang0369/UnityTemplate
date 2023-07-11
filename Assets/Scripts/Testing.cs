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
        this.InstantiateSingleton(ref singleton);

        List<string> list1 = new() { "orb", "borb" };

        list1.AddOrReplace("florb", 1);
        // list1.AddOrReplaceWithBuffer("norb", 12);
        // list1.AddOrReplaceWithBuffer("scorb", 16);
        // list1.AddOrReplaceWithBuffer("desaclorb", 1);

        print(list1);

        // testEnumDictionary[TestEnum.Mould] = "17 gallons of mould";

        // testUnityDict["mould, gallons"] = 17;

        // testUnityDict.Remove("The orb is a bord");
    }
}