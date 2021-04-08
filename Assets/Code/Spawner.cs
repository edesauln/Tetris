using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject[] groups;

    // Start is called before the first frame update
    void Start() {
        spawnNext();
    }

    public void spawnNext() {
        // Random block
        int i = Random.Range(0, groups.Length);

        // Spawn block at current position
        Instantiate(groups[i], transform.position, Quaternion.identity);
    }
}
