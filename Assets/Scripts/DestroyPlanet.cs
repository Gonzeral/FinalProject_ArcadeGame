using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlanet : MonoBehaviour
{
    // Material for destroyed planet
    public Material newMaterial; 
    // Used to check the material before destroying the planet, if planet is about to disappear you can not destroy planet
    public Material warningMaterial;
    private Scoring scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<Scoring>();
    }

    public void ChangePlanetMaterial(GameObject cube)
    {
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (renderer.material.name != warningMaterial.name + " (Instance)" && renderer.material.name != newMaterial.name + " (Instance)")
            {
                renderer.material = newMaterial;
                scoreManager.AddPoints(100); // Award points for destroying a planet
            }
            else
            {
                Debug.Log("Cannot destroy a cube that is about to disappear. Current material: " + renderer.material.name);
            }
        }
    }
}
