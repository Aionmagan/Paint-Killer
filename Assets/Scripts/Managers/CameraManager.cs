using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float[,] viewPort = new float[4, 4];

    void Start()
    {
        //player 1 camera viewport
        //top half screen
        viewPort[0, 0] = 0; viewPort[0, 1] = 0.5f;
        //top right screen
        viewPort[0, 2] = -0.5f; viewPort[0, 3] = 0.5f;

        //player 2 camera viewport
        //bottom half screen
        viewPort[1, 0] = 0; viewPort[1, 1] = -0.5f;
        //top left screen;
        viewPort[1, 2] = 0.5f; viewPort[1, 3] = 0.5f;

        //player 3 camera viewport
        //bottom right screen
        viewPort[2, 0] = -0.5f; viewPort[2, 1] = -0.5f;
        //bottom right screen
        viewPort[2, 2] = -0.5f; viewPort[2, 3] = -0.5f;

        //player 4 camera viewport
        //bottom left screen
        viewPort[3, 0] = 0.5f; viewPort[3, 1] = -0.5f;
        //bottom left screen;
        viewPort[3, 2] = 0.5f; viewPort[3, 3] = -0.5f;

        //players.Clear();
        StartCoroutine(CameraChange());
    } 

    /// <summary>
    /// this courutine is incharged of changing the camera location
    /// a.k.a camera spliting/splicing
    /// </summary>
    /// <returns></returns>
    IEnumerator CameraChange()
    {
        bool changeCamera = true;
        int viewPortSelect = 0;
        int playerLength = 0;
        int playerCount = 0;

        while (changeCamera)
        {
            playerCount = GameManager.Instance.players.Count;

            if (playerLength != playerCount && playerCount > 1)
            {
                //check if theres 2 or 4 players
                if (playerCount % 2 == 1) { viewPortSelect += 2; }

                for (int i = 0; i < playerCount; ++i)
                {
                    Camera[] cam = GameManager.Instance.players[i].GetComponentsInChildren<Camera>();

                    for(int j = 0; j < cam.Length; ++j)
                    {
                        //setting the view port in teh camera component collect in the getComponent line
                        cam[j].rect = new Rect(viewPort[i, viewPortSelect],
                                               viewPort[i, viewPortSelect + 1], 1f, 1f);
                    }
                    
                }
                
                playerLength = playerCount;
            }
            yield return null;

            //break from while loop and close the courutine 
            if(4 <= playerLength) { changeCamera = false; }
        }

        StopCoroutine(CameraChange());
    }
}
