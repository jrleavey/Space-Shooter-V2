using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public float powerupSpeed = 3;

    public int powerUpID;

    private AudioSource audiosource;
    public AudioClip powerUpSound;

    public float distance;

    Transform playerLocation;
    Rigidbody2D move;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        move = GetComponent<Rigidbody2D>();
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        PowerupFall();
        TractorBeamLogic();
    }

    public void PowerupFall()
    {
        transform.Translate(Vector3.down * Time.deltaTime * powerupSpeed);

        if (transform.position.y <= -7)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.transform.GetComponent<PlayerController>();
            AudioSource.PlayClipAtPoint(powerUpSound, transform.position);

            switch (powerUpID)
            {
                case 0:
                    player.TripleShotActivated();
                    break;
                case 1:
                    player.SpeedBoostActivated();
                    break;
                case 2:
                    player.ShieldActive();
                    break;
                case 3:
                    player.CollectedAmmo();
                    break;
                case 4:
                    player.AddLife();
                    break;
                case 5:
                    player.GiantLaserActivated();
                    break;
                case 6:
                    player.CollectedStunHazard();
                    break;

            }
            Destroy(this.gameObject);
        }
    }
    void TractorBeamLogic()
    {
        if (playerLocation != null)
        {
            distance = Vector3.Distance(transform.position, playerLocation.transform.position); ;           
        }
        if (Input.GetKey(KeyCode.C) && distance < 5)
        {
            move.AddForce((playerLocation.transform.position - transform.position) * 1);
        }
    }
}
