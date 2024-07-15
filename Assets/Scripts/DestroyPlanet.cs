using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlanet : MonoBehaviour
{
    public Material newMaterial; 

    public void ChangePlanetMaterial(GameObject cube)
    {
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = newMaterial;
        }
    }
}
