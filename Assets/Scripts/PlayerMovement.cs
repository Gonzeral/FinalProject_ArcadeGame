using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float ufoHeight = 1.5f;
    private GridControl gridControl; // Reference to GridControl
    private Vector2Int currentPosition = new Vector2Int(3 , 3); // Sets current position to center of grid
    private DestroyPlanet matChanger; // Reference DestroyPlanet script
    private Scoring scoreManager; // Reference scoring script
    private bool planetBelow = false; // Flag for live deduction

    void Start()
    {
        gridControl = FindObjectOfType<GridControl>();
        SetInitialPosition();
        matChanger = FindObjectOfType<DestroyPlanet>();
        scoreManager = FindObjectOfType<Scoring>();
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

        CheckForPlanet();
    }

    void JumpTo(int x, int y)
    {
        if (x >= 0 && x < gridControl.grid.GetLength(0) && y >= 0 && y < gridControl.grid.GetLength(1))
        {
            currentPosition = new Vector2Int(x, y);
            Vector3 targetPosition = new Vector3(x * 1.5f, ufoHeight, y * 1.5f);
            transform.position = targetPosition;

            // Create separate script for handling coloring later, for now:
            matChanger.ChangePlanetMaterial(gridControl.grid[x, y]);

            planetBelow = false;
            
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


    void CheckForPlanet()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (!planetBelow)
            {
                planetBelow = true;
                // Deduct life
                scoreManager.LoseLife();
            }

        }
    }

}
