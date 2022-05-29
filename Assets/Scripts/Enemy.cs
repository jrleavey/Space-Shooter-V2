using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private Collider2D collider;

    private GameObject Player;
    public GameObject enemyLaser;
    public GameObject enemyShield;
    public GameObject enemyBomb;
    private GameObject spawnManager;

    public float enemySpeed = 4f;
    public float fireRate =  1f;
    private float nextFire = 2f;

    public int enemyID;

    private AudioSource audioSource;
    public AudioClip onDeathSound;
    public AudioClip laserSound;
    public AudioClip deployBomb;

    public bool shieldOn;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        spawnManager = GameObject.Find("Spawn Manager");

        int shieldChance = Random.Range(0, 100);

        if (shieldChance >= 50)
        {
            shieldOn = true;
        }
        else
        {
            shieldOn = false;
        }
        enemyShield.SetActive(shieldOn);

        if(enemyID != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }

    }

    void Update()
    {
        EnemyMovement();
        EnemyBoundary();
        EnemyCombat();
    }

    public void EnemyMovement()
    {

        switch(enemyID)
        {
            case 0: //base enemy
                transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);
                break;
            case 1: // bomber
                transform.Translate(Vector3.up * enemySpeed * Time.deltaTime);
                break;
            case 2: // harasser
                transform.Translate(Vector3.up * enemySpeed * Time.deltaTime);
                break;
            case 3: // sniper
                transform.Translate(Vector3.up * enemySpeed * Time.deltaTime);
                if (transform.position.y <= 4.5)
                {
                    transform.position = new Vector3(transform.position.x, 4.5f, 0);
                }
                break;
        }      
    }

    public void EnemyBoundary()
    {

        switch (enemyID)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;

        }
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    public void EnemyCombat()
    {
        switch (enemyID)
        {
            case 0:
                if (Time.time > nextFire && collider.isActiveAndEnabled == true && Time.time > 3f)
                {
                    fireRate = Random.Range(2f, 7f);
                    nextFire = Time.time + fireRate;
                    Instantiate(enemyLaser, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
                    AudioSource.PlayClipAtPoint(laserSound, transform.position);
                }
                break;
            case 1:
                if (Time.time > nextFire && collider.isActiveAndEnabled == true && Time.time > 3f)
                {
                    fireRate = Random.Range(3f, 7f);
                    nextFire = Time.time + fireRate;
                    Instantiate(enemyBomb, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
                    AudioSource.PlayClipAtPoint(deployBomb, transform.position);
                }
                break;
            case 2:
                break;
            case 3:
                if (Time.time > nextFire && collider.isActiveAndEnabled == true && Time.time > 3f && transform.position.y == 4.5f)
                {
                    EnemyStrafe();
                    float transformOffset = Player.transform.position.x - transform.position.x;
                    if (transformOffset <= .01f && transformOffset >= -.01f)
                    {
                        fireRate = 3f;
                        nextFire = Time.time + fireRate;
                        Instantiate(enemyLaser, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
                        AudioSource.PlayClipAtPoint(laserSound, transform.position);
                    }
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.tag == "Player")
        {
            Player.GetComponent<PlayerController>().PlayerDamage();


            if( shieldOn == true)
            {
                enemyShield.SetActive(false);
                shieldOn = false;
                return;
            }
            else
            {
                animator.SetTrigger("Death");
                enemySpeed = 0;
                GetComponent<Collider2D>().enabled = false;
                AudioSource.PlayClipAtPoint(onDeathSound, transform.position);
                spawnManager.GetComponent<SpawnManager>()._enemyDeathcount++;

                Destroy(this.gameObject, 2.5f);

            }
        }
        if (other.tag == "Laser")
        {
            if (shieldOn == true)
            {
                Destroy(other.gameObject);
                enemyShield.SetActive(false);
                shieldOn = false;
                return;
            }
            else
            {
                Destroy(other.gameObject);
                PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
                player.AddScore(10);
                animator.SetTrigger("Death");
                enemySpeed = 0;
                GetComponent<Collider2D>().enabled = false;
                AudioSource.PlayClipAtPoint(onDeathSound, transform.position);
                spawnManager.GetComponent<SpawnManager>()._enemyDeathcount++;
                Destroy(this.gameObject, 2.3f);
            }
        }
        if (other.tag == "GiantLaser")
        {
            if(enemyShield.activeInHierarchy)
            {
                enemyShield.SetActive(false);
                shieldOn = false;
            }

            PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
            player.AddScore(10);
            animator.SetTrigger("Death");
            enemySpeed = 0;
            GetComponent<Collider2D>().enabled = false;
            AudioSource.PlayClipAtPoint(onDeathSound, transform.position);
            spawnManager.GetComponent<SpawnManager>()._enemyDeathcount++;
            Destroy(this.gameObject, 2.3f);
        }
    }    

    public IEnumerator DelayFireMethod()
    {
        yield return new WaitForSeconds(3f);
    }

    public void EnemyStrafe()
    {
        float transformOffset = Player.transform.position.x - transform.position.x;
        if (transformOffset > 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else if (transformOffset < 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime);

        }
    }
}
