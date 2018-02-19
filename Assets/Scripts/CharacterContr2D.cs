using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContr2D : MonoBehaviour
{
    public readonly int maxLife = 100;

    public float curLife;


    public float speed = 3.0f;
    public float jumpHeight = 2f;
    public float jumpSpeed = 0.1f;
    //public float grav = 1f;

    //private Rigidbody2D body;
    private Vector2 move;
    private Vector2 velo;
    private Vector2 gravPos;

    private Vector3 startScale;

    public bool grounded;
    private bool midJump;
    private float topJump;

    //private RaycastHit2D[] raycast = new RaycastHit2D[5];
    private Animator anim;

    private AudioSource sound;
    public Transform bloodPrefab;

    private Vector3 camDist;
    public Camera myCam;

    public int poisoned;


    // Use this for initialization
    void Start()
    {
        curLife = maxLife;
        myCam = Camera.main;
        myCam.transform.position = new Vector3(transform.position.x, transform.position.y, myCam.transform.position.z);
        camDist = myCam.transform.position - transform.position;
        Cursor.visible = false;
        //body = GetComponent<Rigidbody2D>();
        startScale = transform.localScale;
        sound = GetComponent<AudioSource>();
        //anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        grounded = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }

    public void hurt(float dmg)
    {
        curLife -= dmg;
        myCam.transform.GetChild(0).GetComponent<UIController>().lifechange(-dmg);
        if (curLife <= 0)
        {
            Destroy(transform.gameObject);
        }
    }


    public void shot(float dmg)
    {
        StartCoroutine(bleeding());
        curLife -= dmg;
        sound.Play();
        myCam.transform.GetChild(0).GetComponent<UIController>().lifechange(-dmg);
        if (curLife <= 0)
        {
            Destroy(transform.gameObject);
        }
    }

    IEnumerator bleeding()
    {
        Transform blood = Instantiate(bloodPrefab, transform.position, transform.rotation, transform);
        yield return new WaitWhile(blood.GetComponent<ParticleSystem>().IsAlive);
        Destroy(blood.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

        //Gravity on/ off?
        /*if (body.Cast(new Vector2(0, -1), raycast, -(velo.y + (speed + jumpHeight) * Physics2D.gravity.y * Time.deltaTime) * Time.deltaTime) == 0) //unten frei
        if(!grounded)
        {
            velo += grav * Physics2D.gravity * Time.deltaTime;
            gravPos = velo * Time.deltaTime;
            transform.position += new Vector3(gravPos.x, gravPos.y, 0);
            //grounded = false;
        }
        else
        {
            velo = Vector2.zero;*/
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            topJump = transform.position.y + jumpHeight;
            midJump = true;
        }
        //grounded = true;
        //  }
        //midJump
        if (midJump && transform.position.y <= topJump /*&& body.Cast(new Vector2(0, 1), raycast, jumpHeight * jumpSpeed) == 0*/)
        {

            transform.position = new Vector3(transform.position.x, transform.position.y + jumpHeight * jumpSpeed, transform.position.z);
        }
        else
        {
            midJump = false;
        }
        //Movement
        move = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            move += new Vector2(-speed, 0) * Time.deltaTime;
            transform.localScale = startScale;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += new Vector2(speed, 0) * Time.deltaTime;
            transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }
        //raycast = new RaycastHit2D[5];
        //if (body.Cast(move, raycast, speed * Time.deltaTime) == 0)
        {
            transform.position += new Vector3(move.x, 0);
        }
        if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            //anim.SetBool("running", false);
        }

        //Kamera Positionieren
        myCam.transform.position = transform.position + camDist;

        if (Input.GetKey(KeyCode.G)) poisoned = 15;
        else poisoned = 0;

        if (poisoned > 0)
        {
            hurt(poisoned * Time.deltaTime);
        }
    }
}
