using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Start()
    {
        DebugExt.DrawCrossCube(Vector3.zero,
            Quaternion.Euler(0, 45, 45), Color.blue, Color.green, 10, 4);
    }
}