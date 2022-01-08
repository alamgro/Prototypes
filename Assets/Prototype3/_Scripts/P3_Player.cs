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
    [SerializeField] private float reduceHeatTimeRate; //the delay between each amount of heat reduced
    [SerializeField] private int heatToReduce; //This is the heat amount to reduce every X (reduceHeatTimeRate) seconds
    [Space]
    [SerializeField] private float increaseHeatTimeRate; //the delay between each amount of heat increased
    [SerializeField] private int heatToIncrease; //This is the heat amount to increase every X (increaseHeatTimeRate) seconds when using the machine

    [Header("Other settings")]
    [SerializeField] private Image UI_HeatBar;
    [SerializeField] private Joystick joystick;

    private int currentHeat = 0;
    private float machineCurrentUseTime = 0f;
    private float overheatCurrentTime = 0f;
    private bool firstTap = true;
    private bool isOverheated = false;

    private Rigidbody2D rigidB;
    private Collider2D playerCollider;
    private Camera cam;
    private float camHorizontalExtents; //cam width

    void Start()
    {
        #region GET COMPONENTS
        rigidB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        cam = Camera.main;
        #endregion

        //Get cam horizontal extents
        camHorizontalExtents = cam.orthographicSize * Screen.width / Screen.height;
        print("Cam extents: " + camHorizontalExtents);

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
                isOverheated = false;

            return;
        }
        #endregion

        #region FUEL LOGIC
        if (joystick.Direction != Vector2.zero)
        {
            //If it is not the first tap, then start adding heat over time
            if (!firstTap)
            {
                machineCurrentUseTime += Time.deltaTime;

                if (machineCurrentUseTime >= increaseHeatTimeRate)
                {
                    CurrentHeat += heatToIncrease;
                    machineCurrentUseTime = 0f;
                }
            }
            else //If it is the first tap, increase heat by default 
            {
                firstTap = false;
                CurrentHeat += heatToIncrease;
            }

            //Update heat bar
            UI_HeatBar.fillAmount = (float)CurrentHeat / maxHeat;
        }
        else
        {
            firstTap = true;
            machineCurrentUseTime = 0f;
        }
        #endregion
     
    }

    private void FixedUpdate()
    {
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
        }
    }
}
