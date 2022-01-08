using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
 * Script to resize the match the desired camera width depending on the device aspect ratio
 */

public class CameraResize : MonoBehaviour
{
    [SerializeField] private SpriteRenderer widthMarker;
    private CinemachineVirtualCamera cam;

    void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();

        cam.m_Lens.OrthographicSize = widthMarker.bounds.size.x * Screen.height / Screen.width * 0.5f; ;
    }

}
