using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class snowman : MonoBehaviour {

    public MonoBehaviour[] scriptsToPause;
    public GameObject shadow;
    public move_ground ground;

    static public float score = -1f;

    private bool dead = false;

    // number of arms
    private int arms = 2;

    // number of body 3 is the largest
    private int body = 3;
    private Rigidbody2D rb2d;
    private Animator anim;

    private float gravity = 10f;
    private float horizontalAngle = 0;
    private bool dragging = false;
    private GameObject groundToAddShadowTo;
    
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
    }

    // Update is called once per frame
    void Update() {
        if (!enabled)
        {
            return;
        }

        if (groundToAddShadowTo)
        {
            Vector3 shadowPosition = gameObject.transform.position;
            shadowPosition.y -= 0.8f;
            shadowPosition.z += 6;
            Instantiate(shadow, shadowPosition, Quaternion.identity, groundToAddShadowTo.transform);
        }
        anim.SetFloat("horizontal_angle", horizontalAngle);
        
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            Vector2 screenPointOfSnowman = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float possibleAngle = (Mathf.Atan2(screenPointOfSnowman.y - Input.mousePosition.y, screenPointOfSnowman.x - Input.mousePosition.x) * 180 / Mathf.PI) - 90;
          
            if (possibleAngle < 80 && possibleAngle > -80)
            {
                horizontalAngle = possibleAngle;
            }
        }
        
        float across = gravity * Mathf.Sin(Mathf.Deg2Rad * horizontalAngle);
        float down =  Mathf.Cos(Mathf.Deg2Rad * horizontalAngle) * gravity;

        rb2d.velocity = new Vector2(across, down);
        ground.speed = down;

        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
        }
        
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            if (collider.name == "first_piece" || collider.name == "second_piece")
            {
                groundToAddShadowTo = collider.gameObject;
            }
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
