using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraImageScaler : MonoBehaviour
{
    private Image _image;
    private RectTransform _rectTransform;
    private Health _health;
    private Camera _camera;

    private Vector3 _lastRectTransform;

    [Header("Image Assignment")]
    public Sprite[] images;

    // Start is called before the first frame update
    void Start()
    {
        //making sure we are getting the RectTransform component from image child and not canvas child
        _rectTransform = this.transform.GetChild(0).GetChild(0).GetComponentInChildren<RectTransform>();

        //making sure we are getting the Image component from image child not canvas child
        _image = this.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>();

        //making sure we are getting component from the player which is a child of the player container
        _health = this.transform.parent.transform.parent.GetComponentInChildren<Health>();

        _camera = GetComponent<Camera>();

        _lastRectTransform = _rectTransform.localScale;
    }

    private void Update()
    {

        if (_health.health != _health.MaxHealth)
        {
            /*setting an offset because
             * array index starts at 0 
             * health starts a 5 
             * _images only have 4-0 indexes 
            */
            int offset = 1;
            if (_health.health < 1) { offset = 0; }
            
            _image.sprite = images[_health.health - offset];
        }
        else
        {
            //putting transparent sprite
            _image.sprite = images[4];  
        }

        //scales the hit effect to camera viewport 
        //NEEDS OPTIMIZATION
        if (_camera.rect.x != 0) 
        {
            _rectTransform.localScale = new Vector3(_lastRectTransform.x/2, 
                                                    _rectTransform.localScale.y,
                                                    _rectTransform.localScale.z); 
        }

        //scales the hit effect to camera viewport 
        //NEEDS OPTIMIZATION
        if (_camera.rect.y != 0)
        {
            _rectTransform.localScale = new Vector3(_rectTransform.localScale.x,
                                                    _lastRectTransform.y / 2,
                                                    _rectTransform.localScale.z);
        }
        
    }
}
