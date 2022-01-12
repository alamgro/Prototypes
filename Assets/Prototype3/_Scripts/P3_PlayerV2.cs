using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class P3_PlayerV2 : MonoBehaviour
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

    enum States { STANDING, FLYING, IDLE } //Spikes states
    FSMstateMachine<States> fsm = new FSMstateMachine<States>();
    /*
    private int currentHeat = 0;
    private float machineCurrentUseTime = 0f;
    private float overheatCurrentTime = 0f; //The amount of time that the machine has been in Overheat state
    private bool firstTap = true;
    private bool isOverheated = false;

    private Rigidbody2D rigidB;
    private Collider2D playerCollider;
    private Camera cam;
    private float camHorizontalExtents; //cam width
    */
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
