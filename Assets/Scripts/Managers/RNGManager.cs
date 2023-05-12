using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class RNGManager : MonoBehaviour
{
    public static RNGManager singleton;

    [Header("User settings")]
	[Tooltip("If true, then start with a new seed for the random number " +
        "generation.")]
    public bool startWithNewSeed = true;

    [Tooltip("Seed for random number generator")]
    public int seed;

    private void Awake()
    {
        if (startWithNewSeed || seed == 0)
        {
            System.Random rand = new();
            seed = rand.Next();
        }
    }

    private void Start()
    {
        this.InstantiateSingleton(ref singleton);
    }
}