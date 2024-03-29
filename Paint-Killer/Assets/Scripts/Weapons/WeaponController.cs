﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;
//using UnityEditor.Experimental.GraphView;

public class WeaponController : MonoBehaviour
{
    /// <summary>
    /// select shoot type 
    /// not used at the moment and should be left on 
    /// AUTO
    /// </summary>
    public enum ShootSelect
    {
        AUTO, 
        BURST, 
        SINGLE
    }

    [Header ("Shooting Mode")]
    public ShootSelect shootSelect;

    [Header("Object Assignment")]
    public Transform shootPointer;

    [Header("Weapon Setting")]
    public float randomRadiusX;
    public float randomRadiusY;
    public float shootSpeed;
    public float demage;

    //controller port being used 
    private int _port;
    private Health _health;
    private AudioController _audioController;
    Vector3 bulletSpread;

    // Start is called before the first frame update
    private void Start()
    {
        _health = this.transform.parent.transform.parent.GetComponentInChildren<Health>();
        _audioController = GetComponentInParent<AudioController>();
        _port = (int)GetComponentInParent<PlayerPortSelect>().playerPort;

        //if burst and single shot are added
        //then remove this
        shootSelect = ShootSelect.AUTO;
       
        switch (shootSelect)
        {
            case ShootSelect.AUTO:
                //AutomaticShooting();
                StartCoroutine(Automatic());
                break;
            case ShootSelect.BURST:
                //BurstShooting();
                break;
            case ShootSelect.SINGLE:
                //SingleShooting();
                break;
        }
    }

    /// <summary>
    /// add function for future updates if any
    /// private void AutomaticShooting()
    /// </summary>
   
    /// <summary>
    /// add function for future updates if any
    /// private void BurstShooting()
    /// </summary>
   
    /// <summary>
    /// add function for future updates if any
    /// private void SingleShooting()
    /// </summary>

    /// <summary>
    /// function that spawns the bullet into the game 
    /// very inefficient but not going to optimized
    /// not needed at the specific instance
    /// NEED OPTIMIZATION: add a pool system to manage bullets
    /// </summary>
    private void Shoot()
    {
        if (_health.health <= 0) { return; }

        _audioController.ShootAudio();

        bulletSpread = Vector3.zero;

        //GameObject bul = Instantiate(bullet, shootPointer.position, shootPointer.rotation);
        GameObject bul = null;
        
        switch(_port-1)
        {
            case 0:
                bul = PoolSystem.Instance.bullets(PoolSystem.Player.BLUE);
                break;
            case 1:
                bul = PoolSystem.Instance.bullets(PoolSystem.Player.GREEN);
                break;
            case 2:
                bul = PoolSystem.Instance.bullets(PoolSystem.Player.RED);
                break;
            case 3:
                bul = PoolSystem.Instance.bullets(PoolSystem.Player.YELLOW);
                break;
        }

        bul.transform.position = shootPointer.position;
        bul.transform.localRotation = shootPointer.rotation;

        if (!Controller.LeftTrigger(_port))
        {
            bulletSpread = new Vector3(Random.Range(-randomRadiusX, randomRadiusX),
                                         Random.Range(-randomRadiusX, randomRadiusX), 0);

            //for bullet spread (bulletSpread)
            //creates the spread but only if you are not 
            //in aim mode
            bul.transform.Rotate(bulletSpread);
        }

        BulletController bulCo = bul.GetComponent<BulletController>();
        //bul.GetComponent<BulletController>().port = _port;
        bulCo.port = _port;
        bulCo.Force();
    }

    /// <summary>
    /// automatic courutine 
    /// for shooting automatically 
    /// has a delay but barely noticeable 
    /// </summary>
    /// <returns></returns>
    IEnumerator Automatic()
    {
        while(true)
        {
            while(Controller.RightTrigger(_port))
            {
                Shoot();
                yield return new WaitForSeconds(shootSpeed);
            }

            yield return null;
        }
    }
}
