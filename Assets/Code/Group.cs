using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {
    float lastFall = 0;

    // If we begin and immediately hit another block, game over!
    void Start() {
        if (!isValidGridPos()) {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        // ----------- Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            moveBlockIfPossible(new Vector3(-1, 0, 0));

        // ----------- Move right
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            moveBlockIfPossible(new Vector3(1, 0, 0));

        // ----------- Rotate 90 degrees counterclockwise
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            // Try rotation
            transform.Rotate(0, 0, -90);

            // Was this move valid?
            if (isValidGridPos()) {
                // Valid; update the grid
                updateGrid();
            } else {
                // Not valid; revert the movement
                transform.Rotate(0, 0, 90);
            }

        // ----------- Fall
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1) {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // Was this move valid?
            if (isValidGridPos()) {
                // Valid; update the grid
                updateGrid();
            } else {
                // Not valid; revert the movement
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                Playfield.deleteFullRows();

                // Spawn next object
                FindObjectOfType<Spawner>().spawnNext();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
    }

    // Move a block to a new location if the move is valid
    // Otherwise revert the movement
    void moveBlockIfPossible(Vector3 v3) {
        Vector3 originalPos = transform.position;
        
        // Modify position
        transform.position += v3;

        // Was this move valid?
        if (isValidGridPos()) {
            // Valid; update the grid
            updateGrid();
        } else {
            // Not valid; revert the movement
            transform.position = originalPos;
        }
    }

    bool isValidGridPos() {
        foreach (Transform child in transform) {
            Vector2 v2 = Playfield.roundVec2(child.position);

            if (!Playfield.isInsideBorder(v2)) 
                return false;

            // Block in grid cell (and not part of the same group)?
            if (Playfield.grid[(int)v2.x, (int)v2.y] != null &&
                Playfield.grid[(int)v2.x, (int)v2.y].parent != transform)
                return false;
        }
        return true;
    }

    // Handles the adding and removing of blocks from the board
    void updateGrid() {
        // Remove old children from grid
        for (int y = 0; y < Playfield.height; y++) {
            for (int x = 0; x < Playfield.width; x++) {
                if (Playfield.grid[x, y] != null) {
                    if (Playfield.grid[x, y].parent == transform) {
                        Playfield.grid[x, y] = null;
                    }
                }
            }
        }

        // Add new children to grid
        foreach (Transform child in transform) {
            Vector2 v2 = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v2.x, (int)v2.y] = child;
        }
    }
}
