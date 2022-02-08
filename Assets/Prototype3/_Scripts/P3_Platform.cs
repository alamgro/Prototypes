using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_Platform : MonoBehaviour
{
    private Rigidbody2D rigidB;
    private float timeToFall;
    private float fallTimer = 0f;
    private bool fall = false;

    private void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (fall)
        {
            fallTimer += Time.deltaTime;
            if(fallTimer >= timeToFall)
            {
                rigidB.isKinematic = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.GetContact(0).normal.y <= -1.0f)
            {
                //Debug.Log("Tocando plataforma", gameObject);
                fallTimer = 0f;
                fall = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision with the respawn collider
        if (collision.CompareTag("Respawn"))
        {
            DespawnPlatform();
        }
    }

    private void DespawnPlatform()
    {
        fall = false;
        fallTimer = 0f;
        P3_GameManager.Instance.platformSpawner.SpawnPlatform(); //Spawn other platform
        P3_GameManager.Instance.RelocateFallCollider(transform.position); //Move falling area
        rigidB.isKinematic = true;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        timeToFall = P3_GameManager.Instance.CurrentPlatformLifetime;
    }
}
