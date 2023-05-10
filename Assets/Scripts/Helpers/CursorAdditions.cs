using System.Collections;
using UnityEngine;

public class CursorAdditions : MonoBehaviour
{
    public bool showOnStart = false;

    private void Start()
    {
        if (showOnStart)
        {
            Cursor.visible = true; 
        }
    }

    public void ToggleCursor(bool visible)
    {
        Cursor.visible = visible;
    }
}