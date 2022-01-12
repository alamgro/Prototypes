using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_Platform : MonoBehaviour
{
    [SerializeField] private float timeToFall;

    private float fallTimer = 0f;
    private bool fall = false;

    void Update()
    {
        if (fall)
        {
            fallTimer += Time.deltaTime;
            if(fallTimer >= timeToFall)
            {
                DespawnPlatform();
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
        //Collision with the respawn collider
        if (collision.gameObject.CompareTag("Respawn"))
        {
            DespawnPlatform();
        }
    }

    private void DespawnPlatform()
    {
        fall = false;
        P3_GameManager.Instance.platformSpawner.SpawnPlatform();
        P3_GameManager.Instance.RelocateFallCollider(transform.position);
        gameObject.SetActive(false);
    }
}
