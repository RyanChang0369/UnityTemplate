using System.Text;

/// <summary>
/// Contains methods that extend string builder.
/// </summary>
public static class StringBuilderExt
{
    public static void AppendSectionBreak(this StringBuilder sb)
    {
        sb.AppendLine("\r\n--------------------------------------------------------------------------------\r\n");
    }
}