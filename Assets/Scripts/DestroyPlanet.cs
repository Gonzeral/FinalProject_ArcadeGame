using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlanet : MonoBehaviour
{
    // Material for destroyed planet
    public Material newMaterial; 
    // Used to check the material before destroying the planet, if planet is about to disappear you can not destroy planet
    public Material warningMaterial;
    // Reference to Scoring script, needed to add points when planet is destroyed
    private Scoring scoreManager;

    void Start()
    {
        // Find reference to the scoring system
        scoreManager = FindObjectOfType<Scoring>();
    }

    // Function to change material and add points then planet is destroyed
    public void ChangePlanetMaterial(GameObject cube)
    {
        // Get the renderer component of cube (To check material)
        Renderer renderer = cube.GetComponent<Renderer>();
        // Check if there is a renderer
        if (renderer != null)
        {
            // Check that current material is not warningMaterial or newMaterial
            // " (Instance)" is added by Unity for materials applied at runtime
            if (renderer.material.name != warningMaterial.name + " (Instance)" && renderer.material.name != newMaterial.name + " (Instance)")
            {
                // Change material ("Destroy planet")
                renderer.material = newMaterial;
                scoreManager.AddPoints(100); // Award points for destroying a planet
            }
            else
            {
                // Log message if planet cannot be destroyed and why
                Debug.Log("Cannot destroy a cube that is about to disappear. Current material: " + renderer.material.name);
            }
        }
    }
}
