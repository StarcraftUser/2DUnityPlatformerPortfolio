using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int statePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public RawImage[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public Text UIGameOver;
    public GameObject UIRestertButton;
    AudioSource audioSource;
    public AudioClip audioDie;
    public AudioClip audiofall;
    public AudioClip audioGameOver;

    // Start is called before the first frame update
    public void NextStage()
    {
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerRePosition();

            UIStage.text = "STAGE " + (stageIndex + 1).ToString();
        }
        else
        {
            totalPoint += statePoint;
            statePoint = 0;
            ScoreManager.NowTotalScore = totalPoint;
            //Time.timeScale = 0.0f;
            SceneManager.LoadScene("VictoryScene");
            return;
        }

        totalPoint += statePoint;
        statePoint = 0;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health > 1)
            {
                PlayerRePosition();
            }
            HealthDown();
        }
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            health--;
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            PlaySound("DIE");
            player.OnDie();
            UIRestertButton.SetActive(true);
            UIGameOver.gameObject.SetActive(true);
        }
    }

    void PlayerRePosition()
    {
        PlaySound("FALL");
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UIPoint.text = (statePoint + totalPoint).ToString();
    }

    public void GameReset()
    {
        PlaySound("GAMEOVER");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FALL":
                audioSource.clip = audiofall;
                break;
            case "GAMEOVER":
                audioSource.clip = audioGameOver;
                break;
        }
        audioSource.Play();
    }
}
