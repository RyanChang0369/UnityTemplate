/// <summary>
/// Contains methods pertaining to miscellaneous things.
/// </summary>
public static class MiscExt
{
    public static void Swap<T>(ref T a, ref T b)
    {
        T t = a;
        a = b;
        b = t;
    }
}
