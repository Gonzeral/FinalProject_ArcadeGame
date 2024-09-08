using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float ufoHeight = 1.5f; // Height position of UFO over grid
    private GridControl gridControl; // Reference to GridControl
    private Vector2Int currentPosition = new Vector2Int(3 , 3); // Sets current position to center of grid
    private DestroyPlanet matChanger; // Reference DestroyPlanet script
    private Scoring scoreManager; // Reference scoring script
    private bool planetBelow = false; // Flag for live deduction

    // Sound for losing life and moving UFO
    public AudioClip loseLifeSound;
    public AudioClip moveSound;
    private AudioSource audioSource;

    // Reference to GameOverManager script, checks GameOver state
    private GameOverManager gameOverManager;

    void Start()
    {
        // Find references to game components
        gridControl = FindObjectOfType<GridControl>();
        SetInitialPosition(); // Set initial position of player in centre of grid
        matChanger = FindObjectOfType<DestroyPlanet>();
        scoreManager = FindObjectOfType<Scoring>();

        // Setup audio source, add one if there is none
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Find GameOverManager to track game state
        gameOverManager = FindObjectOfType<GameOverManager>();
    }


    void Update()
    {
        // If game is over, player cannot move
        if(gameOverManager.IsGameOver)
        {
            return;
        }

        // Player movements with keyboard inputs (Arrow keys or WASD)
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
        {
            JumpTo(currentPosition.x, currentPosition.y - 1);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) 
        {
            JumpTo(currentPosition.x, currentPosition.y + 1);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
        {
            JumpTo(currentPosition.x + 1, currentPosition.y);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) 
        {
            JumpTo(currentPosition.x - 1, currentPosition.y);
        }

        // Check if player has a planet below
        CheckForPlanet();
    }

    // Method to move player to new position
    void JumpTo(int x, int y)
    {
        // Ensures player stays inside grid
        if (x >= 0 && x < gridControl.grid.GetLength(0) && y >= 0 && y < gridControl.grid.GetLength(1))
        {
            // Update player's current position
            currentPosition = new Vector2Int(x, y);
            Vector3 targetPosition = new Vector3(x * 1.5f, ufoHeight, y * 1.5f);
            transform.position = targetPosition;

            // Change material of planet below player (Destroys planet)
            matChanger.ChangePlanetMaterial(gridControl.grid[x, y]);

            // Resets flag
            planetBelow = false;

            // If sound is assigned, play movement sound
            if(moveSound != null)
            {
                audioSource.PlayOneShot(moveSound);
            }
            
        }
        else
        {
            // If player leaves grid, position is reset to centre of grid
            currentPosition = new Vector2Int(3,3);
            Vector3 targetPosition = new Vector3(currentPosition.x * 1.5f, ufoHeight, currentPosition.y * 1.5f);
            transform.position = targetPosition;

            // If the player is not immune, a life is lost
            if(!scoreManager.IsImmune())
            {
                scoreManager.LoseLife();
                // If sound is assigned, play lose life sound
                if(loseLifeSound != null)
                {
                    audioSource.PlayOneShot(loseLifeSound);
                }
            }
        }
    }

    // Method used to set starting position
    void SetInitialPosition()
    {
        //Set initial position to center position and move there
        Vector3 initialPosition = new Vector3(currentPosition.x * 1.5f, ufoHeight, currentPosition.y * 1.5f);
        transform.position = initialPosition;
    }

    // Method checking for planet below player, life is lost if not
    void CheckForPlanet()
    {
        // Raycast from player's position downwards to check for planet
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        // If ray does not hit a cube, player loses life
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (!planetBelow)
            {
                planetBelow = true;
                // Deduct life if player is not immune
                if(!scoreManager.IsImmune())
                {
                    scoreManager.LoseLife();
                    // If sound is assigned, play sound for losing life
                    if(loseLifeSound != null)
                    {
                        audioSource.PlayOneShot(loseLifeSound);
                    }
                }
            }

        }
    }

}
