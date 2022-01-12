using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class P3_Player : MonoBehaviour
{
    [Header("Player settings")]
    [SerializeField] private float maxVelocity;
    //decelerationSmoothnes: This is a value that will "stop" the rigidbody velocity to be greater than the maxVelocity. The greater this value, the smoother the deceleration
    [Range(0.9f, 0.99f)]
    [SerializeField] private float decelerationSmoothnes;
    [Header("Impulse machine settings")]
    [SerializeField] private float impulseForce;
    [SerializeField] private int maxHeat;
    [SerializeField] private float overheatCooldown;
    [Space]
    [SerializeField] private float coolingTimeRate; //the delay between each amount of heat reduced
    [SerializeField] private int heatToReduce; //This is the heat amount to reduce every X (reduceHeatTimeRate) seconds
    [Space]
    [SerializeField] private float increaseHeatTimeRate; //the delay between each amount of heat increased
    [SerializeField] private int heatToIncrease; //This is the heat amount to increase every X (increaseHeatTimeRate) seconds when using the machine
    [Header("Other settings")]
    [SerializeField] private Image UI_HeatBar;
    [SerializeField] private Joystick joystick;

    private int currentHeat = 0;
    private int lastHeat = 0;
    private float heatingTimer = 0f; 
    private float coolingTimer = 0f; 
    private float overheatCurrentTime = 0f; //The amount of time that the machine has been in Overheat state
    private bool firstTap = true;
    private bool isOverheated = false;

    private Rigidbody2D rigidB;
    private Collider2D playerCollider;
    private float camHorizontalExtents; //cam width

    void Start()
    {
        #region GET COMPONENTS
        rigidB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        #endregion

        //Get cam horizontal extents
        camHorizontalExtents = P3_GameManager.Instance.camHorizontalExtents;

        #region VARIABLES SETUP
        maxVelocity *= maxVelocity;
        impulseForce /= 10f; //Divide by 10 so it is possible to use greater values in the inspector
        UI_HeatBar.fillAmount = 0f;
        #endregion
    }

    private void Update()
    {
        #region CHECK PLAYER ON EDGES
        if (transform.position.x > camHorizontalExtents + playerCollider.bounds.extents.x) //when player goes to the right edge
            transform.position = new Vector2(-(camHorizontalExtents + playerCollider.bounds.extents.x), transform.position.y); //Reposition on left side
        else if (transform.position.x < -(camHorizontalExtents + playerCollider.bounds.extents.x)) //when player goes to the left edge
            transform.position = new Vector2(camHorizontalExtents + playerCollider.bounds.extents.x, transform.position.y); //Reposition on right side
        #endregion

        #region CHECK OVERHEAT STATE
        if (isOverheated)
        {
            overheatCurrentTime += Time.deltaTime;

            if (overheatCurrentTime >= overheatCooldown)
            {
                isOverheated = false;
                overheatCurrentTime = 0f;
            }

            //The player can not keep moving if the machine is overheated
            return;
        }
        #endregion

        #region FUEL LOGIC
        lastHeat = CurrentHeat; //Store the last heat amount

        if (joystick.Direction != Vector2.zero)
        {
            //If it is not the first tap, then start adding heat over time
            if (!firstTap)
            {
                heatingTimer += Time.deltaTime;

                //heat up the machine at a specific time rate.
                if (heatingTimer >= increaseHeatTimeRate)
                {
                    CurrentHeat += heatToIncrease;
                    heatingTimer = 0f;
                }
            }
            else //If it is the first tap, increase heat by default 
            {
                firstTap = false;
                CurrentHeat += heatToIncrease;
            }
            
        }
        else
        {
            firstTap = true;
            heatingTimer = 0f;

            #region MACHINE COOLING
            coolingTimer += Time.deltaTime;
            //Cooldown the machine at a specific time rate.
            if (coolingTimer >= coolingTimeRate && CurrentHeat != 0)
            {
                CurrentHeat -= heatToReduce;
                coolingTimer = 0f;
            }
            #endregion
        }
        #endregion

    }

    private void FixedUpdate()
    {
        //Update current distance traveled
        P3_GameManager.Instance.CurrentDistanceTraveled = (int)transform.position.y;

        //Avoid player going faster than the maxVelocity
        if (rigidB.velocity.sqrMagnitude > maxVelocity)
            rigidB.velocity *= 0.95f; //smoothness of the slowdown is controlled by this, 

        //Avoid the player to move when the machine is overheated
        if (isOverheated)
            return;

        //Add force using the joystick direction
        if (joystick.Direction != Vector2.zero)
            rigidB.AddForce(-joystick.Direction * impulseForce, ForceMode2D.Impulse);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            P3_GameManager.Instance.RestartGame();
        }
    }

    private void UpdateHeatBarUI()
    {
        if (CurrentHeat != lastHeat)
        {
            //Update heat bar
            UI_HeatBar.fillAmount = (float)CurrentHeat / maxHeat;
            //print("Updating Heatbar UI");
        }
    }

    public int CurrentHeat
    {
        get { return currentHeat; }
        set
        {
            currentHeat = value;
            if (currentHeat >= maxHeat)
            {
                currentHeat = maxHeat;
                isOverheated = true; //Enter in "overheat" state
            }
            else if (currentHeat <= 0)
                currentHeat = 0;

            UpdateHeatBarUI();
        }
    }


}
