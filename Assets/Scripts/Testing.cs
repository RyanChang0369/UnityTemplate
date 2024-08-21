using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using NaughtyAttributes;
using Newtonsoft.Json;
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

    public EnumDictionary<TestEnum, Range> testEnumDictionary = new();

    public UnityDictionary<string, int> testUnityDict = new();

    public Selector<float> testSelector;

    public Range testRange = new(69, 420);

    /// <summary>
    /// The genotype of the fish.
    /// </summary>
    public struct Genotype : IEquatable<Genotype>
    {
        /// <summary>
        /// A bit array is a handy way to efficiently store the genes.
        /// </summary>
        private BitArray genes;

        public Genotype(BitArray genes)
        {
            this.genes = genes;
        }

        public override bool Equals(object obj)
        {
            if (obj is Genotype gn)
            {
                return Equals(gn);
            }

            return false;
        }

        public bool Equals(Genotype other)
        {
            if (genes.Length == other.genes.Length)
            {
                for (int i = 0; i < genes.Length; i++)
                {
                    if (genes[i] != other.genes[i])
                        return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Taken from https://stackoverflow.com/a/3125721.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = 17;

            for (int i = 0; i < genes.Length; i += 32)
            {
                int subhash = 0;
                int len = Mathf.Min(32, genes.Length - i);

                for (int j = 0; j < len; j++)
                {
                    subhash |= (genes[i + j] ? 1 : 0) << j;
                }

                hash = hash * 23 + subhash;
            }

            return (int)hash;
        }
    }

    private void Start()
    {
        BitArray ba0 = new(16, false);
        ba0[12] = true;

        BitArray ba1 = new(16, false);
        ba1[12] = true;

        Dictionary<BitArray, float> baDict = new();
        baDict[ba0] = 16;
        baDict[ba1] = -51.002f;

        print(ba0.Equals(ba1));

        BitVector32 bv0 = new(9);

        BitVector32 bv1 = new(9);

        Dictionary<BitVector32, float> bvDict = new();
        bvDict[bv0] = 16;
        bvDict[bv1] = -51.002f;

        print(bv0.Data == bv1.Data);

        Genotype gn0 = new(ba0);
        Genotype gn1 = new(ba1);

        Dictionary<Genotype, float> gnDict = new();
        gnDict[gn0] = 16;
        gnDict[gn1] = -51.002f;

        print(gn0.Equals(gn1));
    }

    private void Update()
    {
        print(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    [Button]
    private void TestArraySlices()
    {
        int[] array = new int[] { 14, 19, 52, -8, 0, 0, 15, 4, 2, 3, 6 };
        int[] slice = array[0..2];
        slice[1] = 63422;

        print("Done");
    }

    [Button]
    private void TestArrayParam()
    {
        int[] array = new int[] { 14, 19, 52, -8, 0, 0, 15, 4, 2, 3, 6 };
        ArrayParam(array[0..8]);
    }

    private void ArrayParam(int[] array)
    {
        array[4] = -9000;
    }

    [Button]
    private void TestCompare()
    {
        int a = 90;
        int b = 90 + 1;
        print(a.CompareTo(b));
    }

    [Button]
    private void TestLayers()
    {
        for (int i = 0; i < 32; i++)
        {
            int thingy = 948 & (1 << i);

            if (thingy != 0)
                print(LayerMask.LayerToName(i));
        }

        print((LayerMask)3);
    }

    [Button]
    private void StressTestUnityDict()
    {
        testUnityDict.Clear();

        HashSet<string> randomHashes = new();

        for (int i = 0; i < 500; i++)
        {
            // Testing random new writes.
            var hash = RNGExt.RandomHashString();
            randomHashes.Add(hash);
            testUnityDict[hash] = RNGExt.RandomInt();
        }

        for (int i = 0; i < 500; i++)
        {
            // Testing random new writes and writes to old data.
            if (RNGExt.PercentChance(0.5f))
            {
                // Writes to old data.
                testUnityDict[randomHashes.GetRandomValue()] = RNGExt.RandomInt();
            }
            else
            {
                // Writes to new data.
                var hash = RNGExt.RandomHashString();
                randomHashes.Add(hash);
                testUnityDict[hash] = RNGExt.RandomInt();
            }
        }

        for (int i = 0; i < 285; i++)
        {
            // Random deletes
            string toRemove = randomHashes.GetRandomValue();
            randomHashes.Remove(toRemove);
            testUnityDict.Remove(toRemove);
        }

        testUnityDict.TestValidation();
    }

    [Button]
    private void TestUnityDictSaveLoad()
    {
        SaveLoadDict(testUnityDict);
        SaveLoadDict(testEnumDictionary);
    }

    private void SaveLoadDict<T>(T dictionary) where T : IUnityDictionary
    {
        var serialized = JsonConvert.SerializeObject(dictionary);
        print(serialized);
        var thingy = JsonConvert.DeserializeObject<T>(serialized);
        print(thingy);
    }
}