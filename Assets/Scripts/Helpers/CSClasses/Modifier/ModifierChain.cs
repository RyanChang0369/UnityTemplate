using System.Collections.Generic;

public class ModifierChain
{
    public Dictionary<string, Modifier> chain = new();

    public void Add(string key, Modifier modifier)
    {
        chain.Add(key, modifier);
    }

    public bool Remove(string key)
    {
        return chain.Remove(key);
    }

    public float Modify(float input)
    {
        foreach (var pair in chain)
        {
            input = pair.Value.Modify(input);
        }

        return input;
    }
}