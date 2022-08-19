using System.Collections;
using UnityEngine;

// Calculating Collision Force:
// https://stackoverflow.com/questions/36387753/getting-collision-contact-force

[RequireComponent(typeof(Rigidbody))]
public class CannonballCollision : MonoBehaviour
{
    public int decayTime = 5;
    public int voidHeight = -10;

    private new Rigidbody rigidbody;
    private Vector3 startingPosition;
    private Vector3 previousPosition = Vector3.zero;
    private float maxHeight;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        // If the cannonball falls into the void, destroy it.
        if (transform.position.y < voidHeight)
            Destroy(gameObject);

        // Keep track of the cannonball's max height.
        if (transform.position.y > maxHeight)
            maxHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        var distanceFromCannon = Vector3.Distance(transform.position, startingPosition);
        var currentVelocity = rigidbody.velocity.magnitude;
        var currentDirection = (transform.position - previousPosition).normalized;
        var horizontalAngle = (Vector3.Angle(Vector3.down, currentDirection) - 90);

        // Display on the UI:
        OutputUI.UpdateText(
            "Distance From Cannon: " + distanceFromCannon.ToString("F2"),
            "Maximum Height: " + maxHeight.ToString("F2"),
            "Current Velocity: " + currentVelocity.ToString("F2"),
            "Horizontal Angle: " + horizontalAngle.ToString("F1")
        );

        previousPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        // If the cannonball hits something, destroy it in a few seconds.
        var coroutine = DestroyInSeconds(gameObject, decayTime);
        StartCoroutine(coroutine);

        // Display on the UI.
        var shootingRange = Vector3.Distance(transform.position, startingPosition);
        var collisionForce = (collision.impulse / Time.fixedDeltaTime).magnitude;

        OutputUI.SetOneTimeText(
            "Shooting Range: " + shootingRange.ToString("F2"),
            "Collision Force: " + collisionForce.ToString("F2")
        );
    }

    private static IEnumerator DestroyInSeconds(GameObject obj, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);

        // Clear the UI.
        OutputUI.ClearText();
    }
}
