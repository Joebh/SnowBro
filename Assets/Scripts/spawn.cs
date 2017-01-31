using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class spawn : MonoBehaviour {
    
    public GameObject[] objects;
    public float speed = 7f;
    private GameObject spawned;

	// Use this for initialization
	void Start () {
	}
    
    void Spawn()
    {
        GameObject obj = objects[Random.Range(0, objects.GetLength(0))];
        Vector3 vector = obj.transform.position;
        vector.x = Random.Range(-6, 6);
        spawned = Instantiate(obj, vector, Quaternion.identity);
        spawned.GetComponent<AutoMoveAndRotate>().moveUnitsPerSecond.value = new Vector3(0, speed, 0);
        speed += .2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled && spawned == null)
        {
            Spawn();
        }
    }
}
