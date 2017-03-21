using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class snowman : MonoBehaviour {

    public MonoBehaviour[] scriptsToPause;
    public move_ground ground;

    static public float score = -1f;

    private bool dead = false;

    // number of arms
    private int arms = 2;

    // number of body 3 is the largest
    private int bodyDamage = 0;
    private Rigidbody2D rb2d;
    private Animator anim;

    private Vector3 groundTravelled;
    private GameObject previousGround;
    private float gravity = 8f;
    private bool turningRight = true;
    private float horizontalAngle = 0;
    private bool dragging = false;
    private GameObject groundToAddShadowTo;
    private GameObject otherPiece;
    private LineRenderer lineRenderer;
    
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = Vector2.zero;
        anim = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();

        Camera.main.projectionMatrix = Matrix4x4.Ortho(-Camera.main.orthographicSize * 1.6f, //left
            Camera.main.orthographicSize * 1.6f, //right
            -Camera.main.orthographicSize, //bottom
            Camera.main.orthographicSize, //top
            0.3f, //near 
            1000f); //far
        
        anim.SetTrigger("TurnRight");

        Invoke("AddShadow", .1f);
    }

    void AddShadow()
    {
        if (groundToAddShadowTo)
        {
            Vector3[] shadowPositions = new Vector3[40];
            lineRenderer.GetPositions(shadowPositions);

            if (groundTravelled != null)
            {
                for (int i = shadowPositions.Length - 1; i >= 1; i--)
                {
                    shadowPositions[i] = shadowPositions[i - 1];
                }
            }
            
            Vector3 shadowPosition = gameObject.transform.position;
            shadowPosition.y -= .5f;
            shadowPosition.z = -2f;
            shadowPositions[0] = shadowPosition;

            lineRenderer.SetPositions(shadowPositions);
        }

        Invoke("AddShadow", .1f);
    }

    void UpdateShadows()
    {     
        if (groundTravelled != null)
        {
            Vector3[] shadowPositions = new Vector3[40];
            lineRenderer.GetPositions(shadowPositions);
            for (int i = 1; i < shadowPositions.Length; i++)
            {
                if (groundToAddShadowTo != previousGround)
                {
                    shadowPositions[i].y -= (groundTravelled.y - otherPiece.transform.position.y);
                    shadowPositions[i].z -= (groundTravelled.z - otherPiece.transform.position.z);
                }
                else
                {
                    shadowPositions[i].y -= (groundTravelled.y - groundToAddShadowTo.transform.position.y);
                    shadowPositions[i].z -= (groundTravelled.z - groundToAddShadowTo.transform.position.z);
                }
            }

            lineRenderer.SetPositions(shadowPositions);
        }

        previousGround = groundToAddShadowTo;
        groundTravelled = groundToAddShadowTo.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (!enabled)
        {
            return;
        }

        UpdateShadows();

        anim.SetFloat("horizontal_angle", horizontalAngle);
        
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            Vector2 screenPointOfSnowman = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float possibleAngle = (Mathf.Atan2(screenPointOfSnowman.y - Input.mousePosition.y, screenPointOfSnowman.x - Input.mousePosition.x) * 180 / Mathf.PI) - 90;
            
            if (possibleAngle < -90)
            {
                possibleAngle = (possibleAngle + 180) * -1;
            }

            if (possibleAngle < -80)
            {
                possibleAngle = -80;
            }
            else if (possibleAngle > 80)
            {
                possibleAngle = 80;
            }
            
            horizontalAngle = possibleAngle;

            // if not turning right
            if (!turningRight && horizontalAngle > 0)
            {
                turningRight = true;
            }
            else if (turningRight && horizontalAngle < 0) {
                turningRight = false;
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

        if (arms == 0 || bodyDamage == 3)
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
            
            score = Mathf.Round(ground.distanceTravelled * 1000.0f) / 1000.0f;
            SceneManager.LoadScene("intro");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            if (collider.name == "first_piece" || collider.name == "second_piece")
            {
                otherPiece = groundToAddShadowTo;
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
        else if (other.collider.name.StartsWith("martini"))
        {
            anim.SetTrigger("TakeShot");
            bodyDamage++;
            anim.SetInteger("body_damage", bodyDamage);

            
        }
        else if (other.collider.name.StartsWith("bro")) {
            anim.SetTrigger("HighFive");
            arms--;
        }
    }
}
