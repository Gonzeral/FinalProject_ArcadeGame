using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
    // Cube prefab for planets
    public GameObject GridCube;
    // Arrow prefab for warning of grid movements
    public GameObject WarningPrefab;
    // Grid structure = 6x6 grid
    public GameObject[,] grid = new GameObject[6, 6]; 

    // Following variables in seconds
    public float moveInterval = 2.5f; // When to repeat grid movement
    public float animationDuration = 0.5f; // Duration of the animation 
    public float disappearInterval = 5.0f; // When to repeat disappearing cubes
    public float invisibilityDuration = 5.0f; // Duration of the cube being invisible when disappearing
    public float warningDuration = 2f; // Duration of the warning before cube disappears
    public Material warningMaterial; // Material to warn player about disappearing cube
    public Material undestroyedPlanetMat; // Material of undestroyed planets
    private GameObject currentWarning; // Used for the arrow object instantiated for the row / column movement warning


    // Difficulty scaling parameters
    public float difficultyIncreaseInterval = 5f; // Interval in seconds to increase difficulty
    public float moveIntervalDecrement = 0.1f; // Amount to decrease move interval
    public float disappearIntervalDecrement = 0.2f; // Amount to decrease disappear interval
    public float warningDurationDecrement = 0.1f; // Amount to decrease warning duration
    public float minMoveInterval = 1.0f; // Minimum interval for row/column movements
    public float minDisappearInterval = 1.5f; // Minimum interval for disappearing cubes
    public float minWarningDuration = 0.5f; // Minimum duration for warnings

    // Variables to track timings
    private float currentMoveInterval;
    private float currentDisappearInterval;
    private float currentWarningDuration;

    void Start()
    {
        // Initialize default values for timing trackers
        currentMoveInterval = moveInterval;
        currentDisappearInterval = disappearInterval;
        currentWarningDuration = warningDuration;

        // Nested loop to create grid
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                // Instantiate the cubes with prefab GridCube (planet), 1.5f spacing
                Vector3 position = new Vector3(x * 1.5f, 0, y * 1.5f);
                grid[x, y] = Instantiate(GridCube, position, Quaternion.identity);
            }
        }

        // Call methods in fixed intervals / Methods below
        InvokeRepeating("MoveRandomRowOrColumn", currentMoveInterval, currentMoveInterval);
        InvokeRepeating("MakeCubeDisappear", currentDisappearInterval, currentDisappearInterval);

        InvokeRepeating("IncreaseDifficulty", difficultyIncreaseInterval, difficultyIncreaseInterval);
    }

    void Update()
    {

    }

    // Method to choose random column or row in grid
    void MoveRandomRowOrColumn()
    {
        // Clean up warning signal before starting new row / col movement
        if (currentWarning != null)
        {
            Destroy(currentWarning);
        }

        // 50 % chance to move either a row or a column and direction up or down
        bool moveRow = Random.value > 0.5f;
        bool upOrDown = Random.value > 0.5f;
        int index = Random.Range(0, 6);

        Vector3 warningPosition;
        Quaternion warningRotation;

        // Set position and rotation for warning arrow
        if(moveRow)
        {
            if (upOrDown)
            {
                warningPosition = new Vector3(index * 1.5f, 0.5f, -1.5f);
                warningRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                warningPosition = new Vector3(index * 1.5f, 0.5f, 6 * 1.5f);
                warningRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            if (upOrDown)
            {
                warningPosition = new Vector3(-1.5f, 0.5f, index * 1.5f);
                warningRotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                warningPosition = new Vector3(6 * 1.5f, 0.5f, index * 1.5f);
                warningRotation = Quaternion.Euler(0, -90, 0);
            }
        }

        // Instantiate warning arrow at defined position
        currentWarning = Instantiate(WarningPrefab, warningPosition, warningRotation);
        // Start coroutine to move row or column after warning duration
        StartCoroutine(WarnGridMovement(moveRow, index, upOrDown));

        // Coroutine handling warning before movement of grid
        IEnumerator WarnGridMovement(bool moveRow, int index, bool upOrDown)
        {
            yield return new WaitForSeconds(currentWarningDuration);

            // Remove warning arrow
            if(currentWarning != null)
            {
                Destroy(currentWarning);
            }

            // Move selected row or column after warning duration
            if (moveRow)
            {
                StartCoroutine(MoveRow(index, upOrDown));
            }
            else
            {
                StartCoroutine(MoveColumn(index, upOrDown));
            }
        }
    }

    // Coroutine handling moving of a row in the grid
    IEnumerator MoveRow(int rowIndex, bool upOrDown)
    {
        if (upOrDown)
        {
            // Save reference of cube that leaves grid
            GameObject temp = grid[rowIndex, 5];
            // Deactivate cube leaving grid so it is not visible for player while repositioning
            temp.SetActive(false); 
            for (int i = 5; i > 0; i--)
            {
                // Shift cubes from column i-1 to i / Use coroutine to call animation function
                grid[rowIndex, i] = grid[rowIndex, i - 1];
                StartCoroutine(AnimateMovement(grid[rowIndex, i], new Vector3(rowIndex * 1.5f, 0, i * 1.5f)));
            }
            // Update the reappearing cube to its new position on the other side of the grid / Await execution of repositioning before reactivating cube object
            grid[rowIndex, 0] = temp;
            yield return StartCoroutine(AnimateMovement(temp, new Vector3(rowIndex * 1.5f, 0, 0)));
            temp.transform.position = new Vector3(rowIndex * 1.5f, 0, 0);
            temp.SetActive(true);
        }
        else
        {
            // Save reference of cube that leaves grid
            GameObject temp = grid[rowIndex, 0];
            // Deactivate cube leaving grid so it is not visible for player while repositioning
            temp.SetActive(false); 
            for (int i = 0; i < 5; i++)
            {
                // Shift cubes from column i+1 to i / Use coroutine to call animation function
                grid[rowIndex, i] = grid[rowIndex, i + 1];
                StartCoroutine(AnimateMovement(grid[rowIndex, i], new Vector3(rowIndex * 1.5f, 0, i * 1.5f)));
            }
            // Update the reappearing cube to its new position on the other side of the grid / Await execution of repositioning before reactivating cube object
            grid[rowIndex, 5] = temp;
            yield return StartCoroutine(AnimateMovement(temp, new Vector3(rowIndex * 1.5f, 0, 5 * 1.5f)));
            temp.transform.position = new Vector3(rowIndex * 1.5f, 0, 5 * 1.5f);
            temp.SetActive(true); 
        }
    }

    IEnumerator MoveColumn(int colIndex, bool upOrDown)
    {
        if (upOrDown)
        {
            // Save reference of cube that leaves grid
            GameObject temp = grid[5, colIndex];
            // Deactivate cube leaving grid so it is not visible for player while repositioning
            temp.SetActive(false);
            for (int i = 5; i > 0; i--)
            {
                // Shift cubes from row i-1 to i / Use coroutine to call animation function
                grid[i, colIndex] = grid[i - 1, colIndex];
                StartCoroutine(AnimateMovement(grid[i, colIndex], new Vector3(i * 1.5f, 0, colIndex * 1.5f)));
            }
            // Update the reappearing cube to its new position on the other side of the grid / Await execution of repositioning before reactivating cube object
            grid[0, colIndex] = temp;
            yield return StartCoroutine(AnimateMovement(temp, new Vector3(0, 0, colIndex * 1.5f)));
            temp.transform.position = new Vector3(0, 0, colIndex * 1.5f); 
            temp.SetActive(true); 
        }
        else
        {
            // Save reference of cube that leaves grid
            GameObject temp = grid[0, colIndex];
            // Deactivate cube leaving grid so it is not visible for player while repositioning
            temp.SetActive(false);
            for (int i = 0; i < 5; i++)
            {
                // Shift cubes from row i+1 to i / Use coroutine to call animation function
                grid[i, colIndex] = grid[i + 1, colIndex];
                StartCoroutine(AnimateMovement(grid[i, colIndex], new Vector3(i * 1.5f, 0, colIndex * 1.5f)));
            }
            // Update the reappearing cube to its new position on the other side of the grid / Await execution of repositioning before reactivating cube object
            grid[5, colIndex] = temp;
            yield return StartCoroutine(AnimateMovement(temp, new Vector3(5 * 1.5f, 0, colIndex * 1.5f)));
            temp.transform.position = new Vector3(5 * 1.5f, 0, colIndex * 1.5f);
            temp.SetActive(true);
        }
    }

    // Animation coroutine used for grid movement / Takes the cube object and the new position as input
    IEnumerator AnimateMovement(GameObject cube, Vector3 targetPosition)
    {
        // Get current position of cube and set start time
        Vector3 startPosition = cube.transform.position;
        float timePassed = 0;

        // Until the animation is over
        while (timePassed < animationDuration)
        {
            // Linearly interpolate positions between starting point and target position / Make time pass by
            cube.transform.position = Vector3.Lerp(startPosition, targetPosition, timePassed / animationDuration);
            timePassed += Time.deltaTime;
            yield return null;
        }
        // When the animation time is over = set end position for object (targetPosition)
        cube.transform.position = targetPosition;
    }

    // Choose a random cube in the grid and start coroutine to make cube disappear
    void MakeCubeDisappear()
    {
        int rowIndex = Random.Range(0, 6);
        int colIndex = Random.Range(0, 6);
        StartCoroutine(DisappearReappearCube(rowIndex, colIndex));
    }

    // Coroutine making cubes disappear and reappear
    IEnumerator DisappearReappearCube(int rowIndex, int colIndex)
    {
        // Save the current material of the random cube and warn player by changing material
        GameObject cube = grid[rowIndex, colIndex];
        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.material = warningMaterial;
        // Wait for the warning to be over and then deactivate cube 
        yield return new WaitForSeconds(currentWarningDuration);
        cube.SetActive(false);

        // Change surrounding cubes to undestroyed material
        ChangeSurroundingCubesMaterial(rowIndex, colIndex, undestroyedPlanetMat);

        // Wait before cube is reactivated and make planet undestroyed again
        yield return new WaitForSeconds(invisibilityDuration); 
        renderer.material = undestroyedPlanetMat;
        cube.SetActive(true);
        
    }

    // Method changing material of surrounding cubes
    void ChangeSurroundingCubesMaterial(int rowIndex, int colIndex, Material material)
    {
        // Define the eight possible directions including diagonals
        int[] dx = {-1, 1, 0, 0, -1, -1, 1, 1}; // directions for row: left, right, up, down, and diagonals
        int[] dy = {0, 0, -1, 1, -1, 1, -1, 1}; // directions for col: left, right, up, down, and diagonals
        
        // Change material of adjacent cubes
        for (int i = 0; i < 8; i++)
        {
            int newRow = rowIndex + dx[i];
            int newCol = colIndex + dy[i];
            
            // Ensure the new indices are within the grid bounds
            if (newRow >= 0 && newRow < 6 && newCol >= 0 && newCol < 6)
            {
                GameObject surroundingCube = grid[newRow, newCol];
                Renderer adjRenderer = surroundingCube.GetComponent<Renderer>();
                adjRenderer.material = material;
            }
        }
    }

    // Increases game difficulty by adjusting intervals
    void IncreaseDifficulty()
    {
        // Decrease the move interval but ensure it doesn't go below the minimum
        if (currentMoveInterval > minMoveInterval)
        {
            currentMoveInterval -= moveIntervalDecrement;
            currentMoveInterval = Mathf.Max(currentMoveInterval, minMoveInterval);
            CancelInvoke("MoveRandomRowOrColumn");
            InvokeRepeating("MoveRandomRowOrColumn", currentMoveInterval, currentMoveInterval);
        }

        // Decrease the disappear interval but ensure it doesn't go below the minimum
        if (currentDisappearInterval > minDisappearInterval)
        {
            currentDisappearInterval -= disappearIntervalDecrement;
            currentDisappearInterval = Mathf.Max(currentDisappearInterval, minDisappearInterval);
            CancelInvoke("MakeCubeDisappear");
            InvokeRepeating("MakeCubeDisappear", currentDisappearInterval, currentDisappearInterval);
        }

        // Decrease the warning duration but ensure it doesn't go below the minimum
        if (currentWarningDuration > minWarningDuration)
        {
            currentWarningDuration -= warningDurationDecrement;
            currentWarningDuration = Mathf.Max(currentWarningDuration, minWarningDuration);
        }

        // Log updated values for debugging
        Debug.Log("Current Move Interval: " + currentMoveInterval);
        Debug.Log("Current Disappear Interval: " + currentDisappearInterval);
        Debug.Log("Current Warning Duration: " + currentWarningDuration);
    }

}
