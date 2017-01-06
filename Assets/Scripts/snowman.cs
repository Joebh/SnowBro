using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowman : MonoBehaviour {

    public MonoBehaviour[] scriptsToPause;

    private bool dead = false;

    // number of arms
    private int arms = 2;

    // number of body 3 is the largest
    private int body = 3;
    private Rigidbody2D rb2d;
    private Animator anim;

    private int direction;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        anim = GetComponent<Animator>();
	}

    public void Left()
    {
        direction = -1;
    }
    public void Right()
    {
        direction = 1;
    }

    public void DonePressing()
    {
        direction = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (direction == 1)
        {
            if (rb2d.velocity.x < 12f)
            {
                rb2d.AddForce(new Vector2(30f, 0));
            }
        }
        else if (direction == -1)
        {
            if (rb2d.velocity.x > -12f)
            {
                rb2d.AddForce(new Vector2(-30f, 0));
            }
        }
	}

    void OnAnimationDone()
    {
        anim.SetTrigger("Walk");

        if (arms == 0 || body == 0)
        {
            dead = true;
        }

        if (!dead)
        {
            Debug.Log("Starting game after animation");
            foreach (MonoBehaviour mb in scriptsToPause)
            {
                mb.enabled = true;
            }
        }
        else
        {
            Debug.Log("End game");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag != "Ground")
        {
            Destroy(other.collider.gameObject);

            foreach (MonoBehaviour mb in scriptsToPause)
            {
                mb.enabled = false;
            }
        }

        if (other.collider.name.StartsWith("hottub"))
        {            
            anim.SetTrigger("DeathByHottub");
            dead = true;            
        }
        else if (other.collider.name.StartsWith("shot"))
        {
            anim.SetTrigger("TakeShot");
            body--;
        }
        else if (other.collider.name.StartsWith("bro")) {
            anim.SetTrigger("HighFive");
            arms--;
        }
    }
}
