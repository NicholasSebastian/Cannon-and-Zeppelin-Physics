using System.Collections;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    [Header("Blast Configuration")]
    public float blastMagnitude = 40.0f;
    public float cannonballMass = 1.0f;
    [Range(0.1f, 0.9f)]
    public float cannonballSize = 0.8f;
    public float cannonballDrag = 0.5f;
    public PhysicMaterial cannonballMaterial;

    [Header("Control Configuration")]
    public float rotationSpeed = 2;
    [Range(1, 90)]
    public int horizontalRange = 45;
    [Range(1, 45)]
    public int verticalRange = 30;

    private GameObject cannonball = null;
    private Transform barrel;
    private new CameraFollow camera;

    void Awake()
    {
        barrel = transform.GetChild(0);
        camera = Camera.main.GetComponent<CameraFollow>();
    }

    void Start()
    {
        camera.offset = Camera.main.transform.position - transform.position;
    }

    void Update()
    {
        // Poll for cannonball shot.
        if (cannonball == null && Input.GetMouseButtonDown(0))
        {
            Shoot();
            LogParameters();
        }
    }

    void FixedUpdate()
    {
        // Poll for cannon steering.
        RotateHorizontal();
        RotateVertical();

        // Camera tracking.
        if (cannonball)
            camera.Focus(cannonball.transform.position);
        else
            camera.Focus(barrel.transform.position);
    }

    private void Shoot()
    {
        // Instantiate a cannonball.
        cannonball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cannonball.transform.localScale = Vector3.one * cannonballSize;
        cannonball.transform.position = barrel.position;

        // Set the cannonball's physics material.
        cannonball.GetComponent<Collider>().material = cannonballMaterial;

        // Set the cannonball's rigidbody.
        var rigidbody = cannonball.AddComponent<Rigidbody>();
        rigidbody.mass = cannonballMass;
        rigidbody.drag = cannonballDrag;

        // Apply impulse force in the direction of the barrel.
        rigidbody.AddForce(barrel.up * blastMagnitude, ForceMode.Impulse);

        // Attach the CannonballCollision script.
        cannonball.AddComponent<CannonballCollision>();
    }

    private void RotateHorizontal()
    {
        // Get player input.
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            float currentAngle = NormalizeAngle(transform.rotation.eulerAngles.y);

            // Checks the current angle against the maximum angle.
            bool canTurnRight = currentAngle < horizontalRange;
            bool canTurnLeft = currentAngle > -horizontalRange;

            bool isTurningRight = horizontal > 0;
            bool isTurningLeft = horizontal < 0;

            // Rotate if within limits.
            if ((isTurningLeft && canTurnLeft) || (isTurningRight && canTurnRight))
                transform.Rotate(Vector3.up, horizontal * rotationSpeed, Space.World);
        }
    }

    private void RotateVertical()
    {
        // Get player input.
        float vertical = Input.GetAxis("Vertical");

        if (vertical != 0)
        {
            float currentAngle = NormalizeAngle(transform.rotation.eulerAngles.x);

            // Checks the current angle against the maximum angle.
            bool canTurnDown = currentAngle > 45 - verticalRange;
            bool canTurnUp = currentAngle < 45 + verticalRange;

            bool isTurningDown = vertical > 0;
            bool isTurningUp = vertical < 0;

            // Rotate if within limits.
            if ((isTurningUp && canTurnUp) || (isTurningDown && canTurnDown))
                transform.Rotate(Vector3.up, vertical * rotationSpeed, Space.Self);
        }
    }

    private void LogParameters()
    {
        var launchAngle = (Vector3.Angle(Vector3.down, barrel.up) - 90);

        // Display on the UI:
        OutputUI.AppendText(
            "Impulse Magnitude: " + blastMagnitude,
            "Cannonball Mass: " + cannonballMass,
            "Cannonball Size: " + cannonballSize,
            "Launch Angle: " + launchAngle.ToString("F1"),
            "Air Drag: " + cannonballDrag,
            "Cannonball Bounciness: " + cannonballMaterial.bounciness
        );

        var coroutine = SnapshotVelocity(cannonball);
        StartCoroutine(coroutine);
    }

    private static IEnumerator SnapshotVelocity(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);

        var rigidbody = obj.GetComponent<Rigidbody>();
        var velocity = rigidbody.velocity.magnitude;

        // Display the initial velocity on the UI:
        OutputUI.AppendText("Initial Velocity: " + velocity.ToString("F2"));
    }

    private static float NormalizeAngle(float angle)
    {
        // Converts (0 to 360) angles into (-180 to 180) angles.
        return (angle > 180) ? (angle - 360) : angle;
    }
}
