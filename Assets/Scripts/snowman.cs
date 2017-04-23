using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class snowman : MonoBehaviour {
    
    public move_ground ground;
    public GameObject explosion;

    static public float score = -1f;

    private bool dead = false;

    private float maxAngle = 200f;

    // number of arms
    private int arms = 2;

    // number of body 3 is the largest
    private int bodyDamage = 0;
    private Rigidbody2D rb2d;
    private Animator anim;

    private GameObject jumpToGameObject = null;
    private bool jumpToHottub = false;

    private bool playingAnimation = false;
    private Vector3 groundTravelled;
    private GameObject previousGround;
    private float speed = 75f;
    private float minSpeed = 50f;
    private float maxSpeed = 100f;
    private float boosted = 0;
    private float gravity = 3f;
    private float horizontalAngle = 0;
    private bool dragging = false;
    private GameObject groundToAddShadowTo;
    private GameObject otherPiece;
    private LineRenderer lineRenderer;
    
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();

        Camera.main.projectionMatrix = Matrix4x4.Ortho(-Camera.main.orthographicSize * 1.6f, //left
            Camera.main.orthographicSize * 1.6f, //right
            -Camera.main.orthographicSize, //bottom
            Camera.main.orthographicSize, //top
            0.3f, //near 
            1000f); //far
        
        Vector3[] shadowPositions = new Vector3[40];
        lineRenderer.GetPositions(shadowPositions);
        for (int i = 0; i < shadowPositions.Length; i++)
        {
            Vector3 shadowPosition = gameObject.transform.position;
            shadowPosition.y -= .5f;
            shadowPosition.z = -2f;
            shadowPositions[i] = shadowPosition;
        }

        lineRenderer.SetPositions(shadowPositions);

        Invoke("AddShadow", .1f);
    }

    void AddShadow()
    {
        if (groundToAddShadowTo && !playingAnimation)
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

        if (playingAnimation)
        {
            if (jumpToHottub)
            {
                rb2d.constraints = RigidbodyConstraints2D.None;
                BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
                CircleCollider2D circleCollider = gameObject.GetComponent<CircleCollider2D>();
                collider.isTrigger = true;
                circleCollider.isTrigger = true;

                Physics2D.gravity = new Vector2(0, -3f);
                rb2d.AddForce(new Vector2(
                    (jumpToGameObject.transform.position.x - gameObject.transform.position.x) * 100, 
                    300));

                jumpToHottub = false;
            }

            if (gameObject.transform.position.y < jumpToGameObject.transform.position.y)
            {
                GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                Invoke("OnAnimationDone", 3);
                enabled = false;
                gameObject.GetComponent<Renderer>().enabled = false;
            }

            return;
        }
        
        UpdateShadows();       

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

            float difference = horizontalAngle - possibleAngle;
            float anglePerFrame = maxAngle * Time.deltaTime;
            
            if (Mathf.Abs(difference) > anglePerFrame)
            {
                if (possibleAngle < horizontalAngle)
                {
                    horizontalAngle -= anglePerFrame;
                }
                else if (possibleAngle > horizontalAngle)
                {
                    horizontalAngle += anglePerFrame;
                }
            }
            else
            {
                horizontalAngle = possibleAngle;                
            }
            
            
        }

        if (horizontalAngle < 30 && horizontalAngle > -30)
        {
            speed += (10 * Time.deltaTime);

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
        }

        if (horizontalAngle > 60 || horizontalAngle < -60)
        {
            speed -= (5 * Time.deltaTime);

            if (speed < minSpeed)
            {
                speed = minSpeed;
            }
        }
        anim.SetFloat("horizontal_angle", horizontalAngle);

        float value = (speed + boosted) * gravity * Time.deltaTime;
        float across = value * Mathf.Sin(Mathf.Deg2Rad * horizontalAngle);
        float down =  Mathf.Cos(Mathf.Deg2Rad * horizontalAngle) * value;
        
        if (boosted != 0)
        {
            boosted -= 5 * Time.deltaTime;

            if (boosted < 0)
            {
                boosted = 0;
            }
        }
        rb2d.velocity = new Vector2(across, down);
        ground.speed = down;

        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
        }
        
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
            playingAnimation = false;

            ground.enabled = true;
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

    void disableScripts()
    {
        ground.enabled = false;

        playingAnimation = true;
        rb2d.velocity = new Vector3(0, 0, 0);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name.StartsWith("hottub"))
        {            
            anim.SetTrigger("DeathByHottub");
            jumpToGameObject = other.collider.gameObject;
            jumpToHottub = true;
            disableScripts();
            dead = true;            
        }
        else if (other.collider.name.StartsWith("martini"))
        {
            anim.SetTrigger("TakeShot");
            bodyDamage++;
            Destroy(other.collider.gameObject);
            disableScripts();
            anim.SetInteger("body_damage", bodyDamage);
        }
        else if (other.collider.name.StartsWith("bro")) {
            anim.SetTrigger("HighFive");
            arms--;
        }
        else if (other.collider.name.StartsWith("protein"))
        {
            Destroy(other.collider.gameObject);
            boosted += 30;
        }
    }
}
