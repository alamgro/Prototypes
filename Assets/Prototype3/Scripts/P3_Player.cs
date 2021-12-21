using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_Player : MonoBehaviour
{
    [SerializeField] private float impulseForce;

    private Rigidbody2D rigidB;
    private Camera cam;
    private Vector3 mousePosition;

    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        mousePosition.z = cam.nearClipPlane;
    }



    void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition); //Get mouse position

        #region IMPULSE

        if (Input.GetMouseButton(0))
        {
            Vector2 impulseDirection = -mousePosition;

            rigidB.AddForce(impulseDirection.normalized * impulseForce, ForceMode2D.Impulse);
        }
        #endregion
    }
}
