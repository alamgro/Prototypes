using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_InitialPlatform : MonoBehaviour
{
    [SerializeField] float thresholdDistance;
    [SerializeField] float disapearAfter;

    private Rigidbody2D rigidB;

    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
        rigidB.isKinematic = true;
    }

    void Update()
    {
        //the platform falls when the player distance is greater than the threshold
        if (P3_GameManager.Instance.CurrentDistanceTraveled > thresholdDistance)
        {
            if (rigidB.isKinematic)
            {
                rigidB.isKinematic = false;
                Invoke(nameof(DisablePlatform), disapearAfter);
            }
        }
    }

    private void DisablePlatform()
    {
        gameObject.SetActive(false);
    }
}
