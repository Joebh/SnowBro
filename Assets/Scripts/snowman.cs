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
    
    private Vector3 startPosition;
    private bool dragging = false;
    private int directionTravelling = 0;
    
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        anim = GetComponent<Animator>();

        Camera.main.projectionMatrix = Matrix4x4.Ortho(-Camera.main.orthographicSize * 1.6f, //left
            Camera.main.orthographicSize * 1.6f, //right
            -Camera.main.orthographicSize, //bottom
            Camera.main.orthographicSize, //top
            0.3f, //near 
            1000f); //far

        foreach (MonoBehaviour mb in scriptsToPause)
        {
            mb.enabled = false;
        }

        anim.SetTrigger("DropIn");
    }

    // Update is called once per frame
    void Update() {
        if (!enabled)
        {
            return;
        }
        anim.SetFloat("horizontal_speed", rb2d.velocity.x);

        float absX = Mathf.Abs(rb2d.velocity.x);
        float forceUp = 0;
        if (absX < 6f)
        {
            // controls how fast snowbro goes down
            if (rb2d.velocity.y > -12f)
            {
                forceUp = (absX - 6) * 20;
            }
        }
        else
        {
            // how fast you can cut up hill
            if (rb2d.velocity.y < 10f)
            {
                forceUp = (absX - 6) * 30;
            }
        }
        //else
        //{
        //    rb2d.velocity = new Vector3(rb2d.velocity.x, 0, 0);
        //}
        
        rb2d.AddForce(new Vector2(0, forceUp * Time.deltaTime));

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            Vector2 direction = Input.mousePosition - startPosition;
            float magnitude = 0;

            if (direction.x > 0)
            {
                if (directionTravelling == -1)
                {
                    startPosition = Input.mousePosition;
                }
                directionTravelling = 1;
                if (rb2d.velocity.x < 14f)
                {
                    magnitude = direction.magnitude;
                }
            }
            else
            {
                if (directionTravelling == 1)
                {
                    // switched directions
                    startPosition = Input.mousePosition;
                }
                directionTravelling = -1;
                if (rb2d.velocity.x > -14f)
                {
                    magnitude = -direction.magnitude;
                }
            }

            rb2d.AddForce(new Vector2(magnitude * Time.deltaTime * 10, 0));
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            directionTravelling = 0;
            startPosition = Input.mousePosition;
        }

        //if (Input.touchCount > 0)
        //{
        //    foreach (Touch touch in Input.touches)
        //    {
        //        switch (touch.phase)
        //        {
        //            case TouchPhase.Began:
        //                /* this is a new touch */
        //                isSwipe = true;
        //                fingerStartTime = Time.time;
        //                fingerStartPos = touch.position;
        //                break;

        //            case TouchPhase.Canceled:
        //                /* The touch is being canceled */
        //                isSwipe = false;
        //                Debug.Log("canceled");
        //                break;

        //            case TouchPhase.Ended:

        //                float gestureTime = Time.time - fingerStartTime;
        //                float gestureDist = (touch.position - fingerStartPos).magnitude;

        //                if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
        //                {
        //                    Vector2 direction = touch.position - fingerStartPos;
        //                    Vector2 swipeType = Vector2.zero;

        //                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        //                    {
        //                        // the swipe is horizontal:
        //                        swipeType = Vector2.right * Mathf.Sign(direction.x);
        //                    }
        //                    else {
        //                        // the swipe is vertical:
        //                        swipeType = Vector2.up * Mathf.Sign(direction.y);
        //                    }

        //                    if (swipeType.x != 0.0f)
        //                    {
        //                        if (swipeType.x > 0.0f)
        //                        {
        //                            // MOVE RIGHT
        //                            rb2d.AddForce(new Vector2(0, 2f));
        //                        }
        //                        else {
        //                            // MOVE LEFT
        //                            rb2d.AddForce(new Vector2(0, -2f));
        //                        }
        //                    }

        //                    if (swipeType.y != 0.0f)
        //                    {
        //                        if (swipeType.y > 0.0f)
        //                        {
        //                            // MOVE UP
        //                        }
        //                        else {
        //                            // MOVE DOWN
        //                        }
        //                    }

        //                }
        //                Debug.Log("ended");
        //                break;
        //        }
        //    }
        //}

        //if (direction == 1)
        //{
        //    if (rb2d.velocity.x < 12f)
        //    {
        //        rb2d.AddForce(new Vector2(30f, 0));
        //    }
        //}
        //else if (direction == -1)
        //{
        //    if (rb2d.velocity.x > -12f)
        //    {
        //        rb2d.AddForce(new Vector2(-30f, 0));
        //    }
        //}
    }

    void OnEnable()
    {
        Debug.Log("enabled");
    }

    void OnDisabled()
    {
        Debug.Log("disabled");
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
            enabled = true;

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

            enabled = false;
            rb2d.velocity = new Vector3(0, 0, 0);
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
