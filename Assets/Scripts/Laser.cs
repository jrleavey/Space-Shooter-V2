using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserSpeed = 8.0f;

    private GameObject player;

    
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        LaserMovement();
    }

    public void LaserMovement()
    {
        if (this.gameObject.tag == "Laser")
        {
            transform.Translate(Vector3.up * Time.deltaTime * laserSpeed);
        }

        if (this.gameObject.tag == "EnemyLaser")
        {
            transform.Translate(Vector3.down * Time.deltaTime * laserSpeed);
        }

        if (transform.position.y > 8 | transform.position.y < -6)
        {
            Destroy(this.gameObject);

            if (transform.parent !=null)
            {
                Destroy(transform.parent.gameObject);
            }
        }     
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player" && this.gameObject.tag == "EnemyLaser")
        {
            player.GetComponent<PlayerController>().PlayerDamage();

            Destroy(this.gameObject);

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}



