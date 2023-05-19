using UnityEngine;

public class Testing : MonoBehaviour
{
    public ModifierChain test;

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