using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public float brickMass = 1.0f;
    public List<Wall> walls;

    void Start()
    {
        SpawnWalls();
    }

    private void SpawnWalls()
    {
        foreach (var wall in walls)
            switch (wall.shape)
            {
                case Wall.Shape.Square:
                    CreateSquareWall(wall.position, wall.rotation, wall.height, brickMass);
                    break;

                case Wall.Shape.Triangle:
                    CreateTriangleWall(wall.position, wall.rotation, wall.height, brickMass);
                    break;
            }

        // Display the Brick Mass on the UI:
        OutputUI.SetText("Brick Mass: " + brickMass);
    }

    private static void CreateSquareWall(Vector3 position, float rotation, int size, float mass)
    {
        bool isOdd = size % 2 != 0;
        float offsetX = (isOdd ? (size + 1) : size) / 2;

        // For each coordinate around the given 'position' offset by the given 'size':
        for (float y = 0.5f; y < size + 0.5f; y++)
        {
            float initialOffset = isOdd ? 1 : 0.5f;
            for (float x = initialOffset - offsetX; x < offsetX; x++)
            {
                // Create a brick.
                var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.name = "Brick";
                block.transform.position = position + new Vector3(x, y, 0);
                block.transform.RotateAround(position, Vector3.up, rotation);

                // Attach a Rigidbody with the given 'mass'.
                block.AddComponent<Rigidbody>().mass = mass;
            }
        }
    }

    private static void CreateTriangleWall(Vector3 position, float rotation, int size, float mass)
    {
        bool isOdd = size % 2 != 0;
        float offsetX = (isOdd ? (size + 1) : size) / 2;

        // For each height level:
        for (float level = 0; level < size; level++)
        {
            // Calculate the required parameters considering the offsets.
            float y = level + 0.5f;
            float initialOffset = isOdd ? 1 : 0.5f;
            float narrowingOffset = level * 0.5f;
            float minX = (initialOffset - offsetX) + narrowingOffset;
            float maxX = offsetX - narrowingOffset;

            // For each coordinate around the given 'position' with the above parameters:
            for (float x = minX; x < maxX; x++)
            {
                // Create a brick:
                var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.name = "Brick";
                block.transform.position = position + new Vector3(x, y, 0);
                block.transform.RotateAround(position, Vector3.up, rotation);

                // Attach a Rigidbody with the given 'mass'.
                block.AddComponent<Rigidbody>().mass = mass;
            }
        }
    }
}

[System.Serializable]
public struct Wall
{
    public Vector3 position;
    [Range(-90.0f, 90.0f)]
    public float rotation;
    public Shape shape;
    [Range(1, 10)]
    public int height;

    public enum Shape { Square, Triangle };
}
