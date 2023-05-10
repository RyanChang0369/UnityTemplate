using System;
using System.Collections.Generic;

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