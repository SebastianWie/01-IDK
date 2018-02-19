using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedCharControl : MonoBehaviour {

    private Vector3 mouse;

    private Vector3 startScale;
    public Vector2 size;
    private SpriteRenderer rend;

    public bool ready = true;
    private AudioSource sound;

    public int magazineSize;
    public int bullets;
    public Transform bullet;
    private int recoil = 10;
    private float rate = 0.2f;
    private float reloadSpeed = 0.5f;

    private bool noForce = true;

    
    // Use this for initialization
    void Start () {
        magazineSize = 6;
        bullets = magazineSize;
        sound = GetComponent<AudioSource>();
        startScale = transform.localScale;
        rend = GetComponent<SpriteRenderer>();
        size = rend.bounds.size;

	}

    IEnumerator shooting()
    {
        ready = false;
        noForce = false;
        bullets--;
        Camera.main.transform.Find("UI").GetComponent<UIController>().updateBullets(bullets);
        transform.Rotate(0, 0, recoil*Mathf.Sign(transform.localScale.x));
        Physics2D.Raycast(transform.position, new Vector3(mouse.x, mouse.y, 0));
        
        Transform bul = Instantiate(bullet,transform.position,transform.rotation);
        bul.GetComponent<BulletControl>().instatiate(transform.parent, mouse, mouse.x > transform.position.x);
        
        transform.Find("Mündungsfeuer").gameObject.SetActive(true);
        sound.Play();
        yield return new WaitForSeconds(0.1f);
        transform.Rotate(0, 0, recoil * Mathf.Sign(transform.localScale.x));
        noForce = true;
        transform.Find("Mündungsfeuer").gameObject.SetActive(false);
        yield return new WaitForSeconds(rate-0.1f);
        if (bullets > 0)
        {
            ready = true;
        }
        else reload();
    }

    private void reload()
    {
        UIController ui = Camera.main.transform.Find("UI").GetComponent<UIController>();
        StartCoroutine(ui.reload(reloadSpeed));
    }



    // Update is called once per frame
    void FixedUpdate () {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (noForce)
        {
            transform.LookAt(new Vector3(mouse.x, mouse.y, 0));
            //transform.LookAt(new Vector3(mouse.x, mouse.y - size.y / 2, 0));


            if (mouse.x <= transform.position.x)
            {
                transform.localScale = new Vector3(startScale.x * Mathf.Sign(transform.parent.localScale.x), startScale.y);
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.x);
            }
            else
            {
                transform.localScale = new Vector3(-startScale.x * Mathf.Sign(transform.parent.localScale.x), startScale.y);
                transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.x);
            }
        }

        if (Input.GetMouseButtonDown(0) && ready)
        {
            if (transform.tag == "Weapon")
            {
                StartCoroutine(shooting());
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && ready && transform.tag == "Weapon")
        {
            ready = false;
            reload();
        }
    }
}
