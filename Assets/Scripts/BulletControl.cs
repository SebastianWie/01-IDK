using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {

    private const int dmg = 25;

    private float startTime;
    public float bulletSpeed = 5f;

    Transform gunner;

    Rigidbody2D body;


    // Use this for initialization
    void Start () {

        startTime = Time.time;
        body = GetComponent<Rigidbody2D>();
     }

    public void instatiate(Transform friendly, Vector3 mousePos, bool flipped)
    {
        gunner = friendly;
        transform.LookAt(new Vector3(mousePos.x, mousePos.y, 0));
        if (!flipped)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.x);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.x+180);
        }
    }
    
    void isHit()
    {
        RaycastHit2D[] raycast = new RaycastHit2D[1];
        if (body.Cast(Vector3.left, raycast, bulletSpeed * Time.deltaTime) != 0)
        { 
            if (raycast[0].transform != gunner)
            {
                if (raycast[0].transform.CompareTag("Player"))
                {
                    raycast[0].transform.GetComponent<EnemyController>().shot(dmg);
                }
                Destroy(transform.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (Time.time - startTime > 10)
        {
            Destroy(transform.gameObject);
        }
        isHit();
        transform.Translate(bulletSpeed * Vector3.left *Time.deltaTime);
	}
}
