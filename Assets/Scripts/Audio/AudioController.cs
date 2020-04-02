using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource[] _audioSource;
    private bool[] _feetPlaySound = new bool[2] { false, false };
    private float _lastPitch;

    [Header("Object Assignment")]
    public Transform[] feets = new Transform[2];

    [Space]
    [Header("Object Setting")]
    public LayerMask layermask;
    public float distanceToGround;
    
    [Space]
    [Header("Audio Assignment")]
    public AudioClip foot;
    public AudioClip shoot;
    public AudioClip hit;

    // Start is called before the first frame update
    private void Start()
    {
        //3 audio sources to be able to play more then 1 audio per player 
        _audioSource = GetComponents<AudioSource>();

        _lastPitch   = _audioSource[0].pitch;
        _audioSource[0].volume = 0.5f;
    }

    /// <summary>
    /// raycast downward from each feets and checks for ground 
    /// if it detects something it plays it once
    /// can also use a courutine with a simple equation like so 
    ///  secondsToWait = 1 / speedOfplayer; 
    /// </summary>
    public void  FootAudio()
    {
        for (int i = 0; i < feets.Length; ++i)
        {
            if (Physics.Raycast(feets[i].transform.position, Vector3.down, distanceToGround, layermask))
            {
                Debug.DrawRay(feets[i].transform.position, Vector3.down * distanceToGround, Color.green);

                if (_audioSource[0].isPlaying || _feetPlaySound[i]) { return; }
                _audioSource[0].pitch = Random.Range(_lastPitch - 0.1f, _lastPitch + 0.1f);
                _audioSource[0].PlayOneShot(this.foot);
                _feetPlaySound[i] = true;
            }
            else
            {
                _feetPlaySound[i] = false;
            }
        }
    }

    /// <summary>
    /// pretty self explanatory 
    /// it's being called by WeaponController.cs
    /// </summary>
    public void ShootAudio()
    {
        _audioSource[1].PlayOneShot(shoot);
    }

    /// <summary>
    /// pretty self explanatory 
    /// it's being call by Health.cs
    /// </summary>
    public void HitAudio()
    {
        _audioSource[2].PlayOneShot(hit);
    }
}
