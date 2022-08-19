using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ZeppelinCollision : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Vector3 preCollisionDirection;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        preCollisionDirection = transform.forward;
    }

    void FixedUpdate()
    {
        var currentVelocity = rigidbody.velocity.magnitude;
        var angleFromForward = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
        var angleFromCollision = Vector3.SignedAngle(preCollisionDirection, transform.forward, Vector3.up); ;

        // Display on the UI:
        OutputUI.UpdateText(
            name + " velocity: " + currentVelocity.ToString("F2"),
            name + " angle from north: " + angleFromForward.ToString("F1"),
            name + " angle change from collision: " + angleFromCollision.ToString("F1")
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject.transform;
        var collisionAngle = Vector3.SignedAngle(transform.forward, other.forward, Vector3.up);
        var collisionForce = (collision.impulse / Time.fixedDeltaTime).magnitude;

        OutputUI.ClearText();

        // Display on the UI:
        OutputUI.AppendText(
            "Collision Angle: " + collisionAngle.ToString("F1"),
            "Collision Force: " + collisionForce.ToString("F2")
        );

        preCollisionDirection = transform.forward;
    }
}
