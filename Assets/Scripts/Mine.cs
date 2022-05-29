using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private GameObject player;
    public GameObject mineLight;
    private Animator animator;
    private AudioSource audiosource;
    public AudioClip explosionSound;
    public float mineSpeed = 1;

    private bool isMineArmed = false;

    float timeLeft;
    Color targetColor;

    void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        StartCoroutine(MineArmTime());
    }
    void Update()
    {
        
       if (timeLeft < -Time.deltaTime)
       {
          mineLight.GetComponent<SpriteRenderer>().color = targetColor;

          targetColor = Color.red;
          timeLeft = 1.0f;
       }
       else
       {
          mineLight.GetComponent<SpriteRenderer>().color = Color.Lerp(mineLight.GetComponent<SpriteRenderer>().color, targetColor, Time.deltaTime / timeLeft);

          timeLeft -= Time.deltaTime;
            
       }

        transform.Translate(Vector3.down * mineSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("MineExplosion");
            GetComponent<Collider2D>().enabled = false;
            mineLight.SetActive(false);
            Destroy(this.gameObject, 2.5f);
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            player.GetComponent<PlayerController>().PlayerDamage();
        }

        if (other.tag == "Laser")
        {
            animator.SetTrigger("MineExplosion");
            GetComponent<Collider2D>().enabled = false;
            Destroy(other.gameObject);
            mineLight.SetActive(false);
            Destroy(this.gameObject, 2.5f);
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);


        }
        if (other.tag == "GiantLaser")
        {
            animator.SetTrigger("MineExplosion");
            GetComponent<Collider2D>().enabled = false;
            mineLight.SetActive(false);
            Destroy(this.gameObject, 2.5f);
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        }
    }

    IEnumerator MineArmTime()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Collider2D>().enabled = true;

    }
}
