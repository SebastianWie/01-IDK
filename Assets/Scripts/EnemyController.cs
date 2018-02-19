using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public readonly int maxLife = 100;

    

    public float curLife;


    public float speed = 3.0f;
    public float jumpHeight = 2f;
    public float jumpSpeed = 0.1f;
  
    private Vector2 move;
    private Vector2 velo;
    private Vector2 gravPos;

    private Vector3 startScale;

    public bool grounded;
    private bool midJump;
    private float topJump;

 
    private Animator anim;

    private AudioSource sound;
    public Transform bloodPrefab;
    public Transform healthBar;

    public int poisoned;


    // Use this for initialization
    void Start()
    {
        curLife = maxLife;
        startScale = transform.localScale;
        sound = GetComponent<AudioSource>();
        //anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
        healthBar.parent.parent.GetComponent<UIController>().updateHB(transform.GetComponent<EnemyController>());
        isDead();
    }

    public void shot(float dmg)
    {
        StartCoroutine(bleeding());
        curLife -= dmg;
        healthBar.parent.parent.GetComponent<UIController>().updateHB(transform.GetComponent<EnemyController>());
        sound.Play();
        isDead();
    }

    private void isDead()
    {
        if (curLife <= 0)
        {
            Destroy(healthBar.gameObject);
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

        /*
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            topJump = transform.position.y + jumpHeight;
            midJump = true;
        }*/

        if (midJump && transform.position.y <= topJump)
        {

            transform.position = new Vector3(transform.position.x, transform.position.y + jumpHeight * jumpSpeed, transform.position.z);
        }
        else
        {
            midJump = false;
        }
        //Movement
        /*move = Vector2.zero;
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

        transform.position += new Vector3(move.x, 0);

        if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            //anim.SetBool("running", false);
        }
        */

        if (Input.GetKey(KeyCode.K)) poisoned = 15;
        else poisoned = 0;

        if (poisoned > 0 )
        {
            hurt(poisoned * Time.deltaTime);
        }
    }
}
