using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortSelect : MonoBehaviour
{
    /// <summary>
    /// this is used to determined 
    /// which player prefab belong to 
    /// which controller 
    /// this is only used by Inputmanager 
    /// indirectly 
    /// </summary>
    public enum PlayerPort 
    {
        player_1 = 1, 
        player_2 = 2, 
        player_3 = 3,
        player_4 = 4
    };

    public PlayerPort playerPort = PlayerPort.player_1;
}
