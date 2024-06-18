using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPlayGame : MonoBehaviour
{
    //private void Start()
    //{
    //    PlayerPrefs.SetInt("HighScore", 0);
    //}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
