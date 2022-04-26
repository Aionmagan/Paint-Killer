using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    #region Singleton
    private static CameraUI instance;
    public static CameraUI Instance
    {
        get { return instance; }
    }
    #endregion

    private Text text;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        text = GetComponent<Text>();

        //subcribing fucntions to GamaManaer
        GameManager.Instance.timerCheck += TimerInfo;
        GameManager.Instance.playerWinner += PlayerWin;
    }

    /// <summary>
    /// this is the timer on the center 
    /// of the screen
    /// </summary>
    /// <param name="timer"></param>
    private void TimerInfo(int timer)
    {
        text.text = string.Format("Time: {0}:{1:00}", timer/60, timer%60);
    }


    /// <summary>
    /// this shows in the center of the
    /// screen which player wins
    /// </summary>
    /// <param name="player"></param>
    private void PlayerWin(int player)
    {
        if(player <= 0)
        {
            text.text = "Draw";
            return;
        }

        string winner = "Draw";

        switch(player)
        {
            case 1:
                winner = "Blue";
                text.color = Color.blue;
                break;
            case 2:
                winner = "Green";
                text.color = Color.green;
                break;
            case 3:
                winner = "Red";
                text.color = Color.red;
                break;
            case 4:
                winner = "Yellow";
                text.color = Color.yellow;
                break;
        }

        text.text = $"{winner} Player Wins";
        Time.timeScale = 0; 
    }

    /// <summary>
    /// move the timer to the center 
    /// when two or more players are 
    /// in the scene
    /// this is called by CameraManager.cs
    /// </summary>
    public void MoveTimer()
    {
        //RectTransform rt = GetComponent<RectTransform>();
        //rt.anchoredPosition = new Vector3(11, 0, 0)
    }
}
