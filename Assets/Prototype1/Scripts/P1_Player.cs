using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_Player : MonoBehaviour
{
    [SerializeField] private float speed; //Player movement speed
    [Header("Jump attributes")]
    [SerializeField] private float jumpForce; //Player jump force
    [SerializeField] private Transform jumpOriginPoint; //Origin point of the jump collider. Player can jump if this overlaping with something
    [SerializeField] private LayerMask noPlayerLayerMask;
    [Header("Melee attack attributes")]
    [SerializeField] private int attackMeleeDamage;
    [SerializeField] private float attackMeleeCooldown; //How long it takes to attack again
    [SerializeField] private float knockbackForce; //Knockback force when attacking 
    [SerializeField] private float attackRadius; //Radius
    [SerializeField] private Transform attackMeleeOrigin; //Origin point of the attack collider
    [Header("Shoot attack attributes")]
    [SerializeField] private int attackShootDamage;
    [SerializeField] private float attackShootCooldown; //How long it takes to attack again
    [SerializeField] private GameObject attackProjectilePfb;

    private float attackMeleeTimer = 0f;
    private float attackShootTimer = 0f;
    private float currentSpeed;
    private Rigidbody2D rigidB;
    private Collider2D playerCollider;
    private Vector2 moveDirection;

    void Start()
    {
        #region GET COMPONENTS
        rigidB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        #endregion

        currentSpeed = speed;
        attackMeleeTimer = attackMeleeCooldown;
        transform.localScale = new Vector2(1f, transform.localScale.y);
    }

    void Update()
    {
        #region MOVEMENT
        moveDirection.x = Input.GetAxisRaw("Horizontal") * speed;
        moveDirection.y = rigidB.velocity.y;
        rigidB.velocity = moveDirection;

        if(moveDirection.x > 0)
            transform.localScale = new Vector2(1f, transform.localScale.y);
        if (moveDirection.x < 0)
            transform.localScale = new Vector2(-1f, transform.localScale.y);

        #endregion

        #region JUMP
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Physics2D.OverlapBox(jumpOriginPoint.position, new Vector2(playerCollider.bounds.size.x, 0.1f), 0f, noPlayerLayerMask))
            {
                print("Brincando");
                rigidB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        #endregion

        #region MELEE ATTACK
        attackMeleeTimer += Time.deltaTime; //When the timer is greater than the cooldown, the player is able to attack
        if (Input.GetMouseButtonDown(0) && attackMeleeTimer >= attackMeleeCooldown)
            MeleeAttack();
        #endregion

        #region SHOOT ATTACK
        attackShootTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && attackShootTimer >= attackShootCooldown)
            ShootAttack();
        #endregion
    }

    private void MeleeAttack()
    {
        attackMeleeTimer = 0f; //Restart attack cooldown timer
        //print("¡Atacando!");
        Collider2D[] collidersHit = Physics2D.OverlapCapsuleAll(attackMeleeOrigin.position, new Vector2(0.9f, 1.9f), CapsuleDirection2D.Vertical, 0f);

        foreach (Collider2D colliderHit in collidersHit)
        {
            //Calculate knockback direction and force
            Vector2 knockbackVector = (colliderHit.transform.position - transform.position).normalized * knockbackForce; 
            //Add knockback force to hit object
            colliderHit.attachedRigidbody.AddForce(knockbackVector, ForceMode2D.Impulse);
            print(colliderHit.gameObject.name);
        }
    }

    private void ShootAttack()
    {
        attackShootTimer = 0f; //Restart attack cooldown timer
        //print("¡Atacando!");

        Instantiate(attackProjectilePfb, transform.position, attackProjectilePfb.transform.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if(attackMeleeOrigin && attackRadius > 0f)
            Gizmos.DrawWireSphere(attackMeleeOrigin.position, attackRadius);
    }

    public float Speed { get => speed; set => speed = value; }
}
