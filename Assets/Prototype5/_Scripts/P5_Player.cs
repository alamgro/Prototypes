using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class P5_Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject particlesCrush;

    private Rigidbody2D rb;
    private Vector2 movDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        #region MOVEMENT
        movDir.x = Input.GetAxisRaw(GameConstants.InputName.Horizontal) * speed;
        #endregion

        #region JUMP
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        #endregion

        movDir.y = rb.velocity.y;
        
    }

    private void FixedUpdate()
    {
        rb.velocity = movDir;
    }

    private void OnDisable()
    {
        Instantiate(particlesCrush, transform.position, Quaternion.identity);
    }

}
