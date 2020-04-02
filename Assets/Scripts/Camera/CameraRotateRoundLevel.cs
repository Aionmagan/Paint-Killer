using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateRoundLevel : MonoBehaviour
{
    public GameObject level;
    public float rotateionSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        RespawnManager.Intance.playerSpawned += SpawnedPlayers;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        this.transform.RotateAround(level.transform.position, Vector3.up, rotateionSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        RespawnManager.Intance.playerSpawned -= SpawnedPlayers;
    }

    private void SpawnedPlayers()
    {
        Destroy(this.gameObject);
    }
}
