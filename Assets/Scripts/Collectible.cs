using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Points awarded when collecting specific collectible
    public int points;
    // Reference to CollectibleManager, tracks state of collectible
    public CollectibleManager manager;

    // Collecting sound
    public AudioClip collectSound;
    // Audio source for collectSound
    private AudioSource audioSource;

    void Start()
    {
        // Find AudioSource component in the scene
        audioSource = FindObjectOfType<AudioSource>();
        // Check if AudioSource is in scene, else add new one to game object
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Called when object enters collider
    void OnTriggerEnter(Collider other)
    {
        // Check the object's tag (Must be player)
        if (other.CompareTag("Player"))
        {
            // Find Scoring component in scene, used to add points to score
            Scoring scoreManager = FindObjectOfType<Scoring>();
            if (scoreManager != null)
            {
                // Add points from collectible to score
                scoreManager.AddPoints(points);
            }
            // Remove position from list of used collectible positions
            manager.usedPositions.Remove(new Vector2Int(Mathf.RoundToInt(transform.position.x / 1.5f), Mathf.RoundToInt(transform.position.z / 1.5f)));

            // Check for sound and play it when available
            if(collectSound != null)
            {
                audioSource.PlayOneShot(collectSound);
            }

            //Destroy the collectible after it has been collected
            Destroy(gameObject);
        }
    }
}
