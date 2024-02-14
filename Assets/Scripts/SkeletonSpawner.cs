using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject skelly;
    public Transform[] spawnPoints;
    public float minSpawnTime = 6f;
    public float maxSpawnTime = 12f;

    void Start()
    {
        // Start spawning skeletons repeatedly
        InvokeRepeating("SpawnSkeleton", Random.Range(minSpawnTime, maxSpawnTime), Random.Range(minSpawnTime, maxSpawnTime));
    }

    void SpawnSkeleton()
    {
        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate a skeleton at the chosen spawn point
        GameObject skeleton = Instantiate(skelly, spawnPoint.position, Quaternion.identity);

        // Get the skeleton's script (assuming you have a SkeletonController script)
        SkeletonController skeletonController = skeleton.GetComponent<SkeletonController>();

        // Set the target for the skeleton to the player (assuming player is tagged as "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            skeletonController.SetTarget(player.transform);
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player is tagged as 'Player'.");
        }
    }
}
