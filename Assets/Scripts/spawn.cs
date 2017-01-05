using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class spawn : MonoBehaviour {

    public GameObject[] objects;
    public float spawnMin = 1f;
    public float spawnMax = 2f;
    public float speed = -7f;

	// Use this for initialization
	void Start () {
        Spawn();
	}

    void Spawn()
    {
        GameObject obj = objects[Random.Range(0, objects.GetLength(0))];
        GameObject clone = Instantiate(obj, obj.transform.position, Quaternion.identity);
        clone.GetComponent<AutoMoveAndRotate>().moveUnitsPerSecond.value = new Vector3(0, speed, 0);

        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
