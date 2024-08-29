using NaughtyAttributes;
using UnityEngine;

public class ContainersDemo : MonoBehaviour
{
    #region Enums
    public enum DemoEnum
    {
        NotApplicable = -1,
        NotValid,
        Valid,
        PartiallyValid,
        Mouldy = 21
    }
    #endregion

    #region Variables
    [InfoBox("Fixed lists prevents the editor from changing the size of the " +
        "list or the ordering of the elements.")]
    public FixedList<int> fixedListSimple;

    public FixedList<MonoSelector<string>> fixedListComplex;

    [InfoBox("Unity dictionaries can be used in place of an actual " +
        "dictionary as it implements IDictionary. It features " +
        "element caching, so using it in a build won't slow the game down at " +
        "all. It is also serializable by unity.")]
    public UnityDictionary<string, Color> unityDictionary;

    [InfoBox("Enum dictionaries are wrappers of unity dictionaries that " +
        "allow enum values to be used as keys.")]
    public EnumDictionary<DemoEnum, Color> enumDictionary;

    // [InfoBox("The modifier chain is a priority list of modifiers used to " +
    //     "easily apply and maintain a list of addition/subtraction and " +
    //     "multiplication/division operations on a float. This can be used to " +
    //     "apply a string of buffs and debuffs to player health. Modifiers " +
    //     "implement priority, so certain operations can go in a certain order. " +
    //     "It is also possible to easily remove any operation as well.")]
    // public ModifierChain chainDemo;
    #endregion


    private void Reset()
    {
        // You don't have to instantiate any of these variables in real use. I
        // did so here for the purposes of demonstration.

        fixedListSimple = new() {
            15, -94, 0, 0, 5, 88, 1, 3, 45, 231
        };

        fixedListComplex = new() {
            new MonoSelector<string>(
                new SelectorElement<string>("Bob", 0.17f),
                new SelectorElement<string>("Mary", 0.47f),
                new SelectorElement<string>("John", 0.85f)
            ),
            new MonoSelector<string>(
                new SelectorElement<string>("Chris", 0.9f)
            ),
            new MonoSelector<string>(
                new SelectorElement<string>("Mould Man", 0.7f),
                new SelectorElement<string>("Eater of Greece", 0.4f),
                new SelectorElement<string>("Stephan", 0.52f)
            )
        };

        unityDictionary = new()
        {
            ["Red"] = Color.red,
            ["Blue"] = Color.blue,
            ["Green"] = Color.green,
            ["Yellow"] = Color.yellow
        };

        ColorUtility.TryParseHtmlString("#4d5e27", out Color moldColor);

        enumDictionary = new()
        {
            [DemoEnum.Valid] = Color.green,
            [DemoEnum.NotValid] = Color.red,
            [DemoEnum.PartiallyValid] = Color.yellow,
            [DemoEnum.NotApplicable] = Color.gray,
            [DemoEnum.Mouldy] = moldColor
        };

        // chainDemo = new();
        // chainDemo.Add(new(20, this, "BuffTypeA"), new(0, 1.2f));
        // chainDemo.Add(new(0, this, "BuffTypeB"), new(15, 1f));
        // chainDemo.Add(new(0, this, "BuffTypeC"), new(0, 0.8f));
        // chainDemo.Add(new(-5, this, "BuffTypeD"), new(-5, 5f));
    }
}