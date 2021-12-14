using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rigidB;
    private Vector2 moveDirection;

    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        #region MOVEMENT
        moveDirection.x = Input.GetAxisRaw("Horizontal") * speed;
        moveDirection.y = rigidB.velocity.y;
        #endregion
        
        rigidB.velocity = moveDirection;

        #region JUMP
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        #endregion


        


    }

    public float Speed { get => speed; set => speed = value; }
}
