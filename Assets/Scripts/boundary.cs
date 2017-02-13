using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundary : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            return;
        }
        Destroy(other.gameObject);
    }

}
