using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int asteroidRotateSpeed = 3;

    private Animator animator;

    private GameObject SpawnManager;

    private CircleCollider2D collider;

    private AudioSource audioSource;
    public AudioClip onDeathSound;

    void Start()
    {
        SpawnManager = GameObject.Find("Spawn Manager");
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();

    }


    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * asteroidRotateSpeed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            animator.SetTrigger("AsteroidExplosion");
            Destroy(collider);
            AudioSource.PlayClipAtPoint(onDeathSound, transform.position);
            SpawnManager.GetComponent<SpawnManager>().StartTheGame();
            Destroy(this.gameObject, 2.5f);

        }
    }
}
