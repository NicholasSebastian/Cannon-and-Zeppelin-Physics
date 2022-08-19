using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightCycle : MonoBehaviour
{
    [Range(0.01f, 0.2f)]
    public float clockSpeed = 0.08f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, clockSpeed, Space.World);
    }
}
