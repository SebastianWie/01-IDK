using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private Transform crosshair;
    private Transform profil;
    private Transform curChar;
    private Transform healthBar;
    private Transform health;
    private Transform weaponFrame;

    public Transform enemies;
    public Transform mainChar;
    public Transform enemyHealthbar;
    public Transform actGun;

    //Crosshair
    private Vector3 mousePos;
    private Camera myCam;

    //Healthbar
    private float fullLifeScale = 4.86f;
    private float maxLife;

    //reload
    private int doReload;
    private float reloadSpeed;

    // Use this for initialization
    void Start () {
        maxLife = mainChar.GetComponent<CharacterContr2D>().maxLife;
        myCam = transform.parent.GetComponent<Camera>();
        crosshair = transform.Find("Fadenkreuz");
        profil = transform.Find("Profil");
        healthBar = transform.Find("Healthbar");
        weaponFrame = transform.Find("WeaponFrame");
        health = healthBar.GetChild(0);
        profil.Find("DeadProfil").gameObject.SetActive(false);
        health.localScale = new Vector3(mainChar.GetComponent<CharacterContr2D>().curLife* fullLifeScale / maxLife, health.localScale.y, health.localScale.z);
        weaponFrame.Find("WeaponFrameReload").localScale = new Vector3(1, 0, 1);
        setHealthBars();
    }

    public void setHealthBars()
    {
        foreach (Transform enemy in enemies)
        {
            Transform hb =Instantiate(enemyHealthbar, enemy.position + new Vector3(0, 0.85f, 0), Quaternion.Euler(0, 0, 0), transform.Find("EnemyHB"));
            enemy.GetComponent<EnemyController>().healthBar = hb;
        }
    }

    public void updateHB(EnemyController enemy)
    {
        enemy.healthBar.Find("Lebensbalken").localScale = new Vector3(enemy.curLife / maxLife,1,1);
    }
	
    public void setMainChar(Transform character)
    {
        mainChar = character;
        maxLife = mainChar.GetComponent<CharacterContr2D>().maxLife;
        health.localScale = new Vector3(mainChar.GetComponent<CharacterContr2D>().curLife * fullLifeScale / maxLife, health.localScale.y, health.localScale.z);
    }

    public void instanceMagazine(int count)
    {
        //Anzahl Kugeln einblenden
    }

    public void updateBullets(int count)
    {
        weaponFrame.Find("Patronen").GetChild(count).GetChild(0).gameObject.SetActive(false);
    }

    
    public IEnumerator reload(float speed)
    {
        reloadSpeed = speed;
        doReload = actGun.GetComponent<AttachedCharControl>().magazineSize - actGun.GetComponent<AttachedCharControl>().bullets;
        for (int x = actGun.GetComponent<AttachedCharControl>().bullets; x < actGun.GetComponent<AttachedCharControl>().magazineSize; x++)
        {
            yield return new WaitForSeconds(speed);
            weaponFrame.Find("Patronen").GetChild(x).GetChild(0).gameObject.SetActive(true);
            actGun.GetComponent<AttachedCharControl>().bullets++;
        }
        actGun.GetComponent<AttachedCharControl>().ready = true;
        doReload = 0;
        weaponFrame.Find("WeaponFrameReload").localScale = new Vector3(1, 0, 1);
    }


    public void lifechange(float value)
    {
        if (health.localScale.x + value * fullLifeScale / maxLife > 0)
        {
            health.localScale += new Vector3(value * fullLifeScale / maxLife, 0, 0);
        }
        else
        {
            health.localScale = new Vector3(0, health.localScale.y, health.localScale.z);
            isDead();
        }

    }

    public void isDead()
    {
        profil.Find("DeadProfil").gameObject.SetActive(true);
        crosshair.gameObject.SetActive(false);

    }


	// Update is called once per frame
	void Update () {
        //Crosshair
        mousePos = myCam.ScreenToWorldPoint(Input.mousePosition);
        crosshair.position = new Vector3(mousePos.x, mousePos.y, 0);


        foreach(Transform enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().healthBar.position = enemy.position + new Vector3(0, 0.85f, 0);
        }

        if (doReload > 0)
        {
            weaponFrame.Find("WeaponFrameReload").localScale += new Vector3(0, Time.deltaTime / (doReload*reloadSpeed)*0.9f, 0);
        }

    }
}
