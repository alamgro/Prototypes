using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P5_Squisher : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 initalPos;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initalPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            transform.position = initalPos;
    }


    void FixedUpdate()
    {
        rb.velocity = Vector2.left * speed;
    }
}
