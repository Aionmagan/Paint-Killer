using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;

public class AnimationController : MonoBehaviour
{
    private Animation _animation;
    private AudioController _audioController;
    private PlayerController _player;
    private Health _healh;
    private int _port;

    [Space]
    [Header("Animation clips")]
    public AnimationClip idle;
    public AnimationClip run;
    public AnimationClip jump;
    public AnimationClip death;

    [Space]
    [Header("Animation Settings")]
    public float animationSpeed = 80f; 

    // Start is called before the first frame update
    private void Start()
    {
        _audioController = GetComponentInParent<AudioController>();
        _player          = GetComponent<PlayerController>();
        _port            = (int)GetComponentInParent<PlayerPortSelect>().playerPort;
        _healh           = GetComponent<Health>();
        _animation       = GetComponent<Animation>();
        _animation.Stop();
    }

    private void Update()
    {
        if (_healh.health <= 0) 
        {
            Death();
            return; 
        }
        if (_player.OnGround())
        {
            if (Controller.MainAxesIsActive(_port))
            {
                Run();
            }
            else
            {
                Idle();
            }
        }
        else
        {
            Jump();
        }
    }

    /// <summary>
    /// animation for idle 
    /// using legacy animations 
    /// to develop fast 
    /// </summary>
    private void Idle()
    {
        _animation.wrapMode = WrapMode.Loop;
        _animation[run.name].speed = 2f; 
        _animation.CrossFade(idle.name);
    }

    /// <summary>
    /// animation for run 
    /// using legacy animations 
    /// to develop fast 
    /// </summary>
    private void Run()
    {
        float dir = (Controller.MainVertical(_port) != 0 ? Controller.MainVertical(_port) : 1);

        _audioController.FootAudio();

        _animation.wrapMode = WrapMode.Loop;
        _animation[run.name].speed = dir * (_player.moveSpeed / animationSpeed);
        _animation.CrossFade(run.name);
    }

    /// <summary>
    /// animation for jump 
    /// using legacy animations 
    /// to develop fast 
    /// </summary>
    private void Jump()
    {
        _animation.wrapMode = WrapMode.ClampForever;
        _animation.CrossFade(jump.name);
    }

    /// <summary>
    /// animation for death 
    /// using legacy animations 
    /// to develop fast 
    /// </summary>
    private void Death()
    {
        _animation.wrapMode = WrapMode.ClampForever;
        _animation[death.name].speed = 2;
        _animation.CrossFade(death.name);
    }

}
