using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

public float ufoHeight = 1.5f;
private GridControl gridControl; // Reference to GridControl
private Vector2Int currentPosition = new Vector2Int(3 , 3); // Sets current position to center of grid
private DestroyPlanet matChanger; // Reference DestroyPlanet script

void Start()
{
    gridControl = FindObjectOfType<GridControl>();
    SetInitialPosition();
    matChanger = FindObjectOfType<DestroyPlanet>();
}


void Update()
{
    if(Input.GetKeyDown(KeyCode.UpArrow)) 
    {
        JumpTo(currentPosition.x, currentPosition.y - 1);
    }
    if(Input.GetKeyDown(KeyCode.DownArrow)) 
    {
        JumpTo(currentPosition.x, currentPosition.y + 1);
    }
    if(Input.GetKeyDown(KeyCode.LeftArrow)) 
    {
        JumpTo(currentPosition.x + 1, currentPosition.y);
    }
    if(Input.GetKeyDown(KeyCode.RightArrow)) 
    {
        JumpTo(currentPosition.x - 1, currentPosition.y);
    }
}

void JumpTo(int x, int y)
{
    if (x >= 0 && x < gridControl.grid.GetLength(0) && y >= 0 && y < gridControl.grid.GetLength(1))
    {
        currentPosition = new Vector2Int(x, y);
        Vector3 targetPosition = new Vector3(x * 1.5f, ufoHeight, y * 1.5f);
        transform.position = targetPosition;

        //
        // Create separate script for handling coloring later, for now:
        matChanger.ChangePlanetMaterial(gridControl.grid[x, y]);
        //
        //gridControl.grid[x, y].GetComponent<Renderer>().material.color = Color.red; // Change to desired color
    }
    else
    {
        Debug.Log("Player outside of boundary");
    }
}

void SetInitialPosition()
{
    //Set initial position to center position and move there
    Vector3 initialPosition = new Vector3(currentPosition.x * 1.5f, ufoHeight, currentPosition.y * 1.5f);
    transform.position = initialPosition;
}

}
