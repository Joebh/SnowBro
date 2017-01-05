using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowman : MonoBehaviour {
        
    private float health = 100f;
    private Rigidbody2D rb2d;
    private Animator anim;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("d"))
        {
            if (rb2d.velocity.x < 12f)
            {
                rb2d.AddForce(new Vector2(30f, 0));
            }
        }
        else if (Input.GetKey("a"))
        {
            if (rb2d.velocity.x > -12f)
            {
                rb2d.AddForce(new Vector2(-30f, 0));
            }
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name == "hottub")
        {
            Destroy(other.collider.gameObject);
        }
        else if (other.collider.name == "shot")
        {
            Destroy(other.collider.gameObject);
        }
        else if (other.collider.name == "bro") {
            Destroy(other.collider.gameObject);
        }
    }
}
