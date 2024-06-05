using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

public float jumpHeight = 2.0f;
private Vector2Int currentPosition = new Vector2Int(0, 0);
private GridControl gridControl; // Reference to GridControl

void Start()
{
    gridControl = FindObjectOfType<GridControl>();
}


void Update()
{
    if (Input.GetKeyDown(KeyCode.UpArrow)) JumpTo(currentPosition.x, currentPosition.y - 1);
    if (Input.GetKeyDown(KeyCode.DownArrow)) JumpTo(currentPosition.x, currentPosition.y + 1);
    if (Input.GetKeyDown(KeyCode.LeftArrow)) JumpTo(currentPosition.x + 1, currentPosition.y);
    if (Input.GetKeyDown(KeyCode.RightArrow)) JumpTo(currentPosition.x - 1, currentPosition.y);
}

void JumpTo(int x, int y)
{
    if (x >= 0 && x < 6 && y >= 0 && y < 6)
    {
        currentPosition = new Vector2Int(x, y);
        Vector3 targetPosition = new Vector3(x * 1.5f, jumpHeight, y * 1.5f);
        transform.position = targetPosition;
        gridControl.grid[x, y].GetComponent<Renderer>().material.color = Color.red; // Change to desired color
    }
}
}
