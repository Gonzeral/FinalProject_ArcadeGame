using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public GameObject cowPrefab;
    public GameObject gemstonePrefab;
    public float cowSpawnInterval = 5.0f;
    public float gemstoneSpawnInterval = 13.0f;
    public float collectibleDuration = 4.0f;

    private GridControl gridControl;
    public List<Vector2Int> usedPositions = new List<Vector2Int>();

    void Start()
    {
        gridControl = FindObjectOfType<GridControl>();
        StartCoroutine(SpawnCollectible(cowPrefab, cowSpawnInterval, 200));
        StartCoroutine(SpawnCollectible(gemstonePrefab, gemstoneSpawnInterval, 1000));
    }

    IEnumerator SpawnCollectible(GameObject prefab, float interval, int points)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Vector2Int spawnPosition = GetRandomGridPosition();

            GameObject collectible = Instantiate(prefab, new Vector3(spawnPosition.x * 1.5f, 0.5f, spawnPosition.y * 1.5f), Quaternion.identity);
            collectible.GetComponent<Collectible>().points = points;
            collectible.GetComponent<Collectible>().manager = this;
            usedPositions.Add(spawnPosition);

            yield return new WaitForSeconds(collectibleDuration);
            if (collectible != null)
            {
                Destroy(collectible);
                usedPositions.Remove(spawnPosition);
            }
        }
    }

    Vector2Int GetRandomGridPosition()
    {
        Vector2Int position;
        do
        {
            int x = Random.Range(0, gridControl.grid.GetLength(0));
            int y = Random.Range(0, gridControl.grid.GetLength(1));
            position = new Vector2Int(x, y);
        } while (usedPositions.Contains(position));
        return position;
    }
}
