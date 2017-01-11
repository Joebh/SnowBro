using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class snowman : MonoBehaviour {

    public MonoBehaviour[] scriptsToPause;

    static public float score = -1f;

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
    void Update() {
        anim.SetFloat("horizontal_speed", rb2d.velocity.x);

        float absX = Mathf.Abs(rb2d.velocity.x);
        if (absX < 4f)
        {
            // controls how fast snowbro goes down
            if (rb2d.velocity.y > -12f)
            {
                rb2d.AddForce(new Vector2(0, -2f));
            }
        }
        else if (absX > 7f)
        {
            // how fast you can cut up hill
            if (rb2d.velocity.y < 10f)
            {
                rb2d.AddForce(new Vector2(0, 2f));
            }
        }
        else
        {
            rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
        }


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
            foreach (MonoBehaviour mb in scriptsToPause)
            {
                mb.enabled = true;
            }
        }
        else
        {
            score = Mathf.Round(Time.timeSinceLevelLoad * 100f) / 100f;
            SceneManager.LoadScene("intro");
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
