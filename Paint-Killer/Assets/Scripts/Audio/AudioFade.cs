using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource1;
    [SerializeField]
    private AudioSource _audioSource2;
    
    public AudioClip[] audioSamples;
    public float FadeInSpeed;
    public float crossFadeSpeed;
    public float FadeOutSpeed;

    [Range(0, 1)]
    public float volume = 1;
    [Range (1, 200)]
    public int bpm = 180;

    public int secondsToChange;
    public int lastTimer = 300;

    private int clipSelect = 0;

    //public bool bpm_access = false; //for debuging
    private bool outro = false;//, intro = false;
    private bool crossFade_reverse = false;

    public int timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.timerCheck += CheckTimer;
        GameManager.Instance.timerStart += TimerStarted;
    }

    // Update is called once per frame
    private void CheckTimer(int timer)
    {
        this.timer = timer;
        //ChangeSamples();
    }

    private void TimerStarted()
    {
        //ChangeSamples();
        StartCoroutine(Intro());
        //StartCoroutine(BPM());
    }

    //private void ChangeSamples()
    //{
    //    if (clipSelect >= audioSamples.Length) { return; }

    //    //if (!bpm_access) { return; }
    //    //if (intro)

    //    //switch (lastTimer)
    //    //{
    //    //    case 300:
    //    //        //FadeIntro();
    //    //        break;
    //    //    default:

    //    //        if (timer < sec_change)
    //    //        {
    //    //            FadeOuttro();
    //    //        }
    //    //        else
    //    //        {
    //    //            CrossFade();
    //    //        }
    //    //        break;
    //    //}

    //    CrossFade();
    //    StartCoroutine(Fade());    
    //}

    private void FadeIntro()
    {
        _audioSource1.volume = 0;
        _audioSource1.clip = audioSamples[clipSelect];
        _audioSource1.Play();
        clipSelect++;
    }

    private void FadeOuttro()
    {
       outro = true;
    }

    //access datatype name
    private void CrossFade()
    {
        if (_audioSource1.clip != audioSamples[clipSelect - 1])
        {
            _audioSource1.volume = 0;
            _audioSource1.clip = audioSamples[clipSelect];
            _audioSource1.timeSamples = _audioSource2.timeSamples;
            _audioSource1.Play();
            crossFade_reverse = true;
        }
        else
        {
            _audioSource2.volume = 0;
            _audioSource2.clip = audioSamples[clipSelect];
            _audioSource2.timeSamples = _audioSource1.timeSamples;
            _audioSource2.Play();
            crossFade_reverse = false;
        }
        clipSelect++;
    }

    IEnumerator Intro()
    {
        FadeIntro();

        while(true)
        {
            _audioSource1.volume += FadeInSpeed;

            if (_audioSource1.volume >= volume)
            {
                //intro = false;
                break;
            }
            yield return null;
        }

        StartCoroutine(BPM());
    }
    IEnumerator Fade()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            //if (intro)
            //{

            //    _audioSource1.volume += FadeInSpeed;

            //    if (_audioSource1.volume >= 1.0f) 
            //    { 
            //        intro = false; 
            //        break; 
            //    }
            //}
            if (outro)
            {
                _audioSource1.volume -= FadeOutSpeed;
                _audioSource2.volume -= FadeOutSpeed;

                if (_audioSource1.volume <= 0) { break; }
            }
            else
            {
                if (crossFade_reverse)
                {
                    _audioSource1.volume += crossFadeSpeed;
                    _audioSource2.volume -= crossFadeSpeed;
                }
                else
                {
                    _audioSource1.volume -= crossFadeSpeed;
                    _audioSource2.volume += crossFadeSpeed;
                }

                if (_audioSource2.volume >= volume || _audioSource1.volume >= volume) { break; }
            }
        }
    }

    IEnumerator BPM()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1 / (bpm / 60));

            if (timer < secondsToChange)
            {
                FadeOuttro();
                StartCoroutine(Fade());
                break;
            }

            if (timer < lastTimer - secondsToChange) 
            {
                if (clipSelect >= audioSamples.Length) { continue; }
                CrossFade();
                StartCoroutine(Fade());
                lastTimer = timer;
            }
            //yield return new WaitForSecondsRealtime(1 / (bpm / 60));
        }
    }
}
