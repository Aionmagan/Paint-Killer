using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private AudioController _audioController;
    private int _port;
    private bool isDead = false;
    private int _maxHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
    }

    //can me replaced with Func<>
    public delegate Transform OnDeath(GameObject player);
    public event OnDeath death;

    [Header("Health Settings")]
    public int health = 20;

    private void Start()
    {
        _audioController = GetComponentInParent<AudioController>();
        _port            = (int)GetComponentInParent<PlayerPortSelect>().playerPort;
        _maxHealth       = health;
    }

    /// <summary>
    /// delegaes the death call to any subcribers
    /// adds a death value (by calling GamaManager by instance)
    /// </summary>
    void Die()
    {
        if(death != null)
        this.transform.position = death(this.gameObject).position;

        GameManager.Instance.AddDeath(_port);

        health = MaxHealth;
        isDead = false;
    }

    /// <summary>
    /// checks for collition 
    /// if anything did collide witht his game object (player) 
    /// if that object contains a BulletController component
    /// 
    /// coroutine calls are inefficient inefficient 
    /// and requires to stop them every time before using them again
    /// NEEDS OPTIMIZATION 
    /// 
    /// calles GamaManager by instance to add a kill to score 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        BulletController bullet = null;
        if((bullet = other.gameObject.GetComponent<BulletController>()) && !isDead)
        {
            if (health != 0) { --health; }
            _audioController.HitAudio();

            StopAllCoroutines();
            StartCoroutine(Regeneration());

            if (health <= 0)
            {
                isDead = true;
                health = 0;
                GameManager.Instance.AddKill(bullet.port);
                StopAllCoroutines();
                StartCoroutine(DeathTimer());
            }
        }
    }

    /// <summary>
    /// a timer that leave the player disabled and laying 
    /// on the floor before respawning him 
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathTimer()
    {
        yield return new WaitForSecondsRealtime(3);
        Die();
    }

    /// <summary>
    /// regeneration of health 
    /// only if player is not dead 
    /// </summary>
    /// <returns></returns>
    IEnumerator Regeneration()
    {
        bool regenerate = true;
        while(regenerate)
        {
            yield return new WaitForSeconds(1);

            if (health >= MaxHealth)
            {
                health = MaxHealth;
                regenerate = false;
            }
            else 
            {
                health++;
            }
            
        }
    }
}
