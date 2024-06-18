using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text NowScoreText;
    public Text RealScoreText;
    public static int NowTotalScore = 0;
    protected int RealTotalScore = 0;
    void Start()
    {
        RealTotalScore = PlayerPrefs.GetInt("HighScore");
        NowScoreText.text = "���� ���� : " + NowTotalScore.ToString();
        RealScoreText.text = "�ְ����� : " + RealTotalScore.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (RealTotalScore < NowTotalScore)
            {
                RealTotalScore  = NowTotalScore;
                PlayerPrefs.SetInt("HighScore", RealTotalScore);
                SceneManager.LoadScene("FirstScene");
                return;
            }
        }
    }

    void End()
    {

    }
}
