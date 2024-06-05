using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
    public GameObject GridCube;
    public GameObject[,] grid = new GameObject[6, 6];

    public float moveInterval = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                Vector3 position = new Vector3(x * 1.5f, 0, y * 1.5f);
                grid[x, y] = Instantiate(GridCube, position, Quaternion.identity);
            }
        }

        InvokeRepeating("MoveRandomRowOrColumn", moveInterval, moveInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveRandomRowOrColumn()
    {
        bool moveRow = Random.value > 0.5f;
        int index = Random.Range(0, 6);
        if (moveRow)
        {
            MoveRow(index);
        }
        else
        {
            MoveColumn(index);
        }
    }

    void MoveRow(int rowIndex)
    {
        // Store the rightmost element temporarily
        GameObject temp = grid[rowIndex, 5];
        // Shift all elements in the row to the right
        for (int i = 5; i > 0; i--)
        {
            grid[rowIndex, i] = grid[rowIndex, i - 1];
            // Update the position of the moved element
            grid[rowIndex, i].transform.position = new Vector3(rowIndex * 1.5f, 0, i * 1.5f);
        }
        // Move the temporarily stored element to the leftmost position
        grid[rowIndex, 0] = temp;
        grid[rowIndex, 0].transform.position = new Vector3(rowIndex * 1.5f, 0, 0);
    }

    void MoveColumn(int colIndex)
    {
        // Store the bottommost element temporarily
        GameObject temp = grid[5, colIndex];
        // Shift all elements in the column down
        for (int i = 5; i > 0; i--)
        {
            grid[i, colIndex] = grid[i - 1, colIndex];
            // Update the position of the moved element
            grid[i, colIndex].transform.position = new Vector3(i * 1.5f, 0, colIndex * 1.5f);
        }
        // Move the temporarily stored element to the topmost position
        grid[0, colIndex] = temp;
        grid[0, colIndex].transform.position = new Vector3(0, 0, colIndex * 1.5f);
    }
}
