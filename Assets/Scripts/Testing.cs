using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Start()
    {
        DebugExt.UseDebug(Color.blue, Color.green, 10);
        DebugExt.DrawCrossCube(Vector3.zero,
            Quaternion.Euler(0, 45, 45), 4);
    }

    private void OnDrawGizmos()
    {
        DebugExt.UseGizmos(Color.red);
        DebugExt.DrawCrossRect(new Rect(5, 6, 10, 15));
    }
}