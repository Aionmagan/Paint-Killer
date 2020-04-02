using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;

public class RespawnManager : MonoBehaviour
{
    #region Singleton
    private static RespawnManager instance = null;
    public static RespawnManager Intance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    private CameraManager camManager;

    //can be replaced with Action<>
    public delegate void RespawnSysCall();
    public event RespawnSysCall playerSpawned;

    [Header("Respawn assignment")]
    public Transform[] spawnPoints = new Transform[4];
    public GameObject[] players = new GameObject[4];

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        camManager = GetComponent<CameraManager>();
        StartCoroutine(StartButtonAllControllers());    
    }

    /// <summary>
    /// function to spawn player object 
    /// this is called by different parts of the code 
    /// </summary>
    private void SpawnPlayer(in int player)
    {
        //player is sub by '1' because index starts at '0' and player return that starts at 1 
        GameObject obj = Instantiate(players[player-1], spawnPoints[player-1].position, spawnPoints[player-1].rotation);
        //camManager.AddPlayer(obj);
        GameManager.Instance.AddPlayer(obj);
        obj.GetComponentInChildren<Health>().death += RespawnPlayer;

        //if (playerSpawned != null) { playerSpawned(); }
        playerSpawned?.Invoke();
    }

    /// <summary>
    /// this function looks for a spawnpoint that is farthest
    /// of the param player and return it's transform 
    /// it's being called from Health.cs(delegates)
    /// </summary>
    public Transform RespawnPlayer(GameObject player)
    {
        int index = 0;
        Transform spawn = null;
        GameObject[] players = new GameObject[3];
        GameObject otherPlayer;

        //get player objects and compare with player to spawn
        for(int i = 1; i < this.players.Length; ++i)
        {
            otherPlayer = GameObject.FindGameObjectWithTag($"Player {i.ToString()}");
            
            //check if player exist and it's not the same tag 
            //as param player
            if (otherPlayer == null) { continue; }
            if (!otherPlayer.CompareTag(player.tag))
            {
                players[index] = otherPlayer;
                index++;
                Debug.Log(otherPlayer.tag);
            }
            
        }

        //if there's no players in players[] then send random spawnPoint
        if (index == 0) return spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

        float currentDistance = 0;
        float lastDistance = 50f;
        int[] indexSkip = {-1, -1, -1, -1}; 
        //compare player distance to spawnpoints
        for(int i = 0; i < index; ++i)
        {
            if (players[i] == null) { continue; }

            for (int j = 0; j < spawnPoints.Length; ++j)
            {
                if (indexSkip[j] == j) { continue; }

                //getting farthest spawnpoints from all enemies to spawn player in
                currentDistance = Vector3.Distance(players[i].transform.position, spawnPoints[j].position);

                if (lastDistance > currentDistance)
                {
                    indexSkip[j] = j;
                    //spawn = spawnPoints[j];
                    //lastDistance = currentDistance;
                }
                else
                {
                    spawn = spawnPoints[j];
                }
            }
        }

        return spawn;
    }

    /// <summary>
    /// UNUSED IGNORE
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Transform ResetPlayer(in int player)
    {
        return spawnPoints[player];
    }

    /// <summary>
    /// checks if all 4 controller has pressed start and if so
    /// it will spawn each player in order of which controller
    /// spressed start first 
    /// </summary>
    IEnumerator StartButtonAllControllers()
    {
        bool checkControllers = true;
        bool[] startsPressed = { true, true, true, true };
        while (checkControllers)
        {
            for(int i = 1; i < 5; ++i)
            {
                if(Controller.Action7(Button.DOWN, i))
                {
                    if(startsPressed[i-1])
                    {
                        //bool array is to prevent more then 1 spawn of a single player 
                        startsPressed[i-1] = false;
                        SpawnPlayer(i);
                    }
                }
            }

            //check if every controller pressed start
            //if so make checkControlelrs false;
            int checkStartPressedIndex = 0;
            for (int i = 0; i < startsPressed.Length; ++i)
            {
                if(!startsPressed[i])
                {
                    checkStartPressedIndex++;
                }
            }

            if (checkStartPressedIndex == startsPressed.Length) { checkControllers = false; }

            yield return null;
        }
    }
}
