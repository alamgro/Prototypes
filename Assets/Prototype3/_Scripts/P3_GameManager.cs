using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class P3_GameManager : MonoBehaviour
{
    void Start()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("P3_Game");
    }
}
