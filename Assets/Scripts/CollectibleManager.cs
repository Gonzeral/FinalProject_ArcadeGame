using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    // Prefab for cow-collectible
    public GameObject cowPrefab;
    // Prefab for gemstone-collectible
    public GameObject gemstonePrefab;
    // Spawn interval in seconds for cow-collectible
    public float cowSpawnInterval = 5.0f;
    // Spawn interval in seconds for gemstone-collectible
    public float gemstoneSpawnInterval = 13.0f;
    // Duration in seconds while collectible is active before disappearing
    public float collectibleDuration = 4.0f;
    // Reference to GridControl script (used for grid dimensions)
    private GridControl gridControl;
    // List of grid positions currently used by active collectibles
    public List<Vector2Int> usedPositions = new List<Vector2Int>();

    void Start()
    {
        // Find GridControl component in scene
        gridControl = FindObjectOfType<GridControl>();
        // Start spawning cows and gemstones according to defined intervals (and points awarded)
        StartCoroutine(SpawnCollectible(cowPrefab, cowSpawnInterval, 200));
        StartCoroutine(SpawnCollectible(gemstonePrefab, gemstoneSpawnInterval, 1000));
    }

    // Coroutine handling collectibles spawning
    IEnumerator SpawnCollectible(GameObject prefab, float interval, int points)
    {
        // While game is running, spawn collectibles
        while (true)
        {
            // Wait for the interval to pass before spwaning new collectible
            yield return new WaitForSeconds(interval);
            // Get a random position in the grid that is not occupied
            Vector2Int spawnPosition = GetRandomGridPosition();

            //Instantiate a collectible at the chosen position
            GameObject collectible = Instantiate(prefab, new Vector3(spawnPosition.x * 1.5f, 0.5f, spawnPosition.y * 1.5f), Quaternion.identity);
            //Set points for the collectible and link it to the manager
            collectible.GetComponent<Collectible>().points = points;
            collectible.GetComponent<Collectible>().manager = this;
            // Add the position to the list of used positions
            usedPositions.Add(spawnPosition);

            // Wait for the duration of the collectible before removing it
            yield return new WaitForSeconds(collectibleDuration);
            // If the collectible still exists in the scene, destroy it and delete from used positions
            if (collectible != null)
            {
                Destroy(collectible);
                usedPositions.Remove(spawnPosition);
            }
        }
    }

    // Returns a random position currently not used by a collectible
    Vector2Int GetRandomGridPosition()
    {
        Vector2Int position;
        // Loop until position not in use is found
        do
        {
            // Get a random x and y value in grid
            int x = Random.Range(0, gridControl.grid.GetLength(0));
            int y = Random.Range(0, gridControl.grid.GetLength(1));
            // Create a 2D vector with the x and y values
            position = new Vector2Int(x, y);
        } while (usedPositions.Contains(position)); // Do it again if position is in use
        // Return the free position
        return position;
    }
}
