using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int points;
    public CollectibleManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Scoring scoreManager = FindObjectOfType<Scoring>();
            if (scoreManager != null)
            {
                scoreManager.AddPoints(points);
            }
            manager.usedPositions.Remove(new Vector2Int(Mathf.RoundToInt(transform.position.x / 1.5f), Mathf.RoundToInt(transform.position.z / 1.5f)));
            Destroy(gameObject);
        }
    }
}
