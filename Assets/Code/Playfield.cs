using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour {
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    // In the event that a rotation makes a shape's coordinates not round numbers
    public static Vector2 roundVec2(Vector2 v) {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    // Checking whether a shape is within the border
    public static bool isInsideBorder(Vector2 pos) {
        return ((int)pos.x >= 0 &&
                (int)pos.x < width &&
                (int)pos.y >= 0);
    }

    // Delete every block in a row
    public static void deleteRow(int y) {
        for (int x = 0; x < width; x++) {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // Move each block in a row down the board
    public static void decreaseRow(int y) {
        for (int x = 0; x < width; x++) {
            if (grid[x, y] != null) {
                // Move one down
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // Moves all rows above y down one level
    public static void decreaseRowsAbove(int y) {
        for (int i = y; i < height; i++)
            decreaseRow(i);
    }

    // Whether a row is full
    public static bool isRowFull(int y) {
        for (int x = 0; x < width; x++)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    // Deletes all rows that have been completed
    public static void deleteFullRows() {
        for (int y = 0; y < height; y++) {
            if (isRowFull(y)) {
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                y--;
            }
        }
    }
}
