using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [System.Serializable]
    public struct Grid
    {
        public int cols, rows;
        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;
    public GameObject gridTile;
    public List<Vector2> availablePoints = new List<Vector2>();

    private void Awake()
    {
        room = GetComponentInParent<Room>();
        grid.cols = room.Width - 8;
        grid.rows = room.Height - 8;
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset+= room.transform.localPosition.x;

        for (int y = 0; y < grid.rows;  y++)
        {
            for (int x = 0; x < grid.cols; x++)
            {
                GameObject gameObject = Instantiate(gridTile, transform);
                gameObject.GetComponent<Transform>().position = new Vector2(x - (grid.cols - grid.horizontalOffset), y - (grid.rows - grid.verticalOffset));
                gameObject.name = "X: " + x + ", Y: " + y;
                availablePoints.Add(gameObject.transform.position);
                gameObject.SetActive(false);
            }
        }

        GetComponentInParent<ObjectRoomSpawner>().InitialiseObjectSpawning();
    }
}
