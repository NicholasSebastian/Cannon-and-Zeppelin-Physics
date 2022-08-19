using UnityEngine;

public class ZeppelinControl : MonoBehaviour
{
    [Header("Zeppelin Configuration")]
    public GameObject zeppelinModel;
    public float movementSpeed = 8.0f;
    public float zeppelinMass = 1.0f;

    [Header("Starting Configuration")]
    public float distanceApart;
    [Range(40, 90)]
    public int rotation1;
    [Range(40, 90)]
    public int rotation2;

    [Header("Camera Configuration")]
    public Vector3 cameraOffset;

    private Rigidbody zeppelin1;
    private Rigidbody zeppelin2;
    private new CameraFollow camera;

    void Awake()
    {
        camera = Camera.main.GetComponent<CameraFollow>();
    }

    void Start()
    {
        camera.offset = cameraOffset;
        var offset = distanceApart / 2;

        // Spawn the two zeppelins.
        zeppelin1 = SpawnZeppelin("Zeppelin 1", -offset, 0, 0, 90 - rotation1);
        zeppelin2 = SpawnZeppelin("Zeppelin 2", offset, 0, 0, rotation2 - 90);

        // Move the zeppelins.
        zeppelin1.velocity = zeppelin1.transform.forward * movementSpeed;
        zeppelin2.velocity = zeppelin2.transform.forward * movementSpeed;

        LogParameters();
    }

    void FixedUpdate()
    {
        // Camera tracking.
        var distance = zeppelin1.transform.position - zeppelin2.transform.position;
        var centerPoint = zeppelin1.transform.position - (distance / 2);

        camera.Focus(centerPoint);
    }

    private void LogParameters()
    {
        var zeppelinBounciness = zeppelin1.GetComponent<Collider>().material.bounciness;

        // Display on the UI:
        OutputUI.SetText(
            "Zeppelin Masses: " + zeppelinMass,
            "Zeppelin Bounciness: " + zeppelinBounciness,
            "Initial Velocities: " + movementSpeed
        );
    }

    private Rigidbody SpawnZeppelin(string name, float x, float y, float z, int rotationY)
    {
        // Instantiate a zeppelin.
        var position = new Vector3(x, y, z);
        var rotation = Quaternion.Euler(0, rotationY, 0);
        var instance = Instantiate(zeppelinModel, position, rotation);
        instance.name = name;

        // Set the zeppelin's mass.
        var rigidbody = instance.GetComponent<Rigidbody>();
        rigidbody.mass = zeppelinMass;

        return rigidbody;
    }
}
