using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    #region Singleton
    private static PoolSystem _instance;
    public static PoolSystem Instance
    {
        get { return _instance; }
    }
    #endregion
    public GameObject blue_bullet;
    public GameObject green_bullet;
    public GameObject red_bullet;
    public GameObject yellow_bullet;

    private int listLength = 0;

    List<GameObject>[] _bullets = new List<GameObject>[4];

    public enum Player
    {
        BLUE,
        GREEN, 
        RED,
        YELLOW
    };

    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    private void Start()
    {
        for(int i = 0; i < 4; ++i)
        {
            _bullets[i] = new List<GameObject>();
        }
    }

    public GameObject  bullets(Player playerColor)
    {
        listLength = _bullets[(int)playerColor].Count;

        if (listLength > 0)
        {
            for (int i = 0; i < listLength; ++i)
            {
                if (!_bullets[(int)playerColor][i].activeInHierarchy)
                {
                    _bullets[(int)playerColor][i].SetActive(true);
                    return _bullets[(int)playerColor][i];
                }
            }
        }
        GameObject bullet = null; 

        switch((int)playerColor)
        {
            case 0:
                bullet = Instantiate(blue_bullet);
                break;
            case 1:
                bullet = Instantiate(green_bullet);
                break;
            case 2:
                bullet = Instantiate(red_bullet);
                break;
            case 3:
                bullet = Instantiate(yellow_bullet);
                break;
        }

        _bullets[(int)playerColor].Add(bullet);

        return bullet;

    }
}
