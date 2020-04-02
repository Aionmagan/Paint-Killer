using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;
public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }
    #endregion

    //this can also be replaced with Func<> 
    public delegate void TimerCheck(in int timer);
    public event TimerCheck timerCheck;

    //this can also be replaced with Func<>
    public delegate void PlayerWinner(in int player);
    public event PlayerWinner playerWinner;

    //death is not used at the momment
    //kills are use to evaluate 
    private int[] kills = new int[4];
    private int[] deaths = new int[4];

    public List<GameObject> players;
    private bool timerStarted = false;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this.gameObject);

        Screen.SetResolution(1280,720,true);

        players.Clear();
    }

    private void Update()
    {
        //simple way to close the game
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        
    }

    /// <summary>
    /// evaluate all kills by all players 
    /// and choose the highest one to determine 
    /// winner 
    /// </summary>
    private void Evaluate()
    {
        int indexOfPlayer = -1;
        int moreKills = 0;

        //compare kills
        for(int i = 0; i < players.Count; ++i)
        {
            for(int j = 0; j < players.Count; ++j)
            {
                //same player so continue to next 
                //loop iteration
                if (i == j) { continue; }
                
                if(kills[i] > kills[j])
                {
                    if (moreKills < kills[i])
                    {
                        moreKills = kills[i];
                        indexOfPlayer = i;
                    }
                }
            }
        }

        playerWinner(indexOfPlayer+1);
        
        //load levels in a few seconds 
        StartCoroutine(LoadLevel());
    }

    /// <summary>
    /// starts game timer 
    /// this function is called 
    /// by the Cameramanager.cs 
    /// using the instance 
    /// </summary>
    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    /// <summary>
    /// adds player to the list to be used in the 
    /// courutine (CameraChange())
    /// </summary>
    public void AddPlayer(in GameObject player)
    {
        players.Add(player);

        if (players.Count > 1 && !timerStarted)
        {
            timerStarted = true;
            GameManager.Instance.StartTimer();
            CameraUI.Instance.MoveTimer();
        }
    }

    /// <summary>
    /// called by Health.cs
    /// using the instance
    /// </summary>
    /// <param name="player"></param>
    public void AddKill(in int player)
    {
        kills[player-1]++;
    }

    /// <summary>
    /// called by Health.cs
    /// using the instance
    /// unused for evaluation
    /// lefts behind for future updates
    /// if any
    /// </summary>
    /// <param name="player"></param>
    public void AddDeath(in int player)
    {
        deaths[player-1]++;
    }

    /// <summary>
    /// loads the same level(test/0) 
    /// in 4 seconds after Evaluation 
    /// was called and completed
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadLevel()
    {
        yield return new WaitForSecondsRealtime(4);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// game timer 'the one that appears in
    /// the center of the screen'
    /// information pulled by CameraUI.cs
    /// using the instance
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        //5mins
        int timer = 300;

        while(timer >= 0)
        {
            //if (timerCheck != null) { timerCheck(timer); }
            timerCheck?.Invoke(timer);

            yield return new WaitForSecondsRealtime(1);
            timer -= 1;
        }

        Evaluate();
        StopCoroutine(Timer());
    }


}
