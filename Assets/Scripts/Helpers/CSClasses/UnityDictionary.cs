using System;
using System.Collections.Generic;

/// <summary>
/// Used to display a dictionary in the unity inspector. Use if you want to
/// allow the user to modify a dictionary that won't be changed anywhere else.
/// </summary>
/// <typeparam name="Key">A unity-serializable value.</typeparam>
/// <typeparam name="Value">A unity-serializable value.</typeparam>
[Serializable]
public class UnityDictionary<Key, Value>
{
    [Serializable]
    public struct InspectorKeyValuePair
    {
        public Key key;

        public Value value;
    }

    public List<InspectorKeyValuePair> keyValuePairs;
    private int prevKVPsHash;

    private Dictionary<Key, Value> internalDict;
    public Dictionary<Key, Value> Dictionary 
    {
        get
        {
            int KVPsHash = keyValuePairs.GetHashCode();
            if (internalDict == null || KVPsHash != prevKVPsHash)
            {
                GenerateInternalDict();
                prevKVPsHash = KVPsHash;
            }

            return internalDict;
        }
    }

    private void GenerateInternalDict()
    {
        internalDict = new();

        foreach (var keyValue in keyValuePairs)
        {
            internalDict[keyValue.key] = keyValue.value;
        }
    }
}