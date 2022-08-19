using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [HideInInspector]
    public Vector3 offset = Vector3.zero;
    public float cameraSmoothing = 0.1f;

    public void Focus(Vector3 target)
    {
        var camera = Camera.main.transform;
        var targetPosition = target + offset;
        var smoothFollow = Vector3.Lerp(camera.position, targetPosition, cameraSmoothing);

        // Move the camera to follow and look at the target.
        camera.position = smoothFollow;
        camera.LookAt(target);
    }
}
