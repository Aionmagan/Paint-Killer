using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;

public class CameraController : MonoBehaviour
{
    [Header("Object assignment")]
    public Transform gunZoomPoint;
    public Transform hipPoint;
    public Transform pointToFollow;
    public Transform armsPointer;
    public GameObject arms;
    public new GameObject camera;

    [Space]
    [Header("Camera Settings")]
    public float xAxisRotation;
    public float yAxisRotation;
    public float zoomLerpSpeed;
    //public float armsLerpSpeed;
    public Vector3 pointToFollowOffset;
    public float yAxisLimitUp;
    public float yAxisLimitDown;

    [HideInInspector]
    public bool AimMode = false;
    [HideInInspector]
    public Transform playerPointer;

    private int _port;
    private float _xAxis;
    private float _yAxis;
    private Vector3 _position;
    private Health _health;

    // Start is called before the first frame update
    private void Start()
    {
        _health = this.transform.parent.GetComponentInChildren<Health>();
        _port = (int)GetComponentInParent<PlayerPortSelect>().playerPort;
        playerPointer = new GameObject().transform;
        playerPointer.name = "Player pointer " + _port.ToString();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        CameraRotation();
        CameraZoom();
        ArmsLerp();
    }

    /// <summary>
    /// rotates camera with clamp on X rotation
    /// aimMode is also set here
    /// </summary>
    private void CameraRotation()
    {
        playerPointer.position = this.transform.position;
        //playerPointer.eulerAngles = Vector3.up * this.transform.eulerAngles.y;

        this.transform.position = pointToFollow.position + pointToFollowOffset;

        //return if health is 0 (to prevent movement from player)
        if (_health.health <= 0) { return; }

        AimMode = Controller.LeftTrigger(_port);

        _xAxis += Controller.SecondHorizontal(_port) * xAxisRotation * Time.deltaTime;
        _yAxis += Controller.SecondVertical(_port) * yAxisRotation * Time.deltaTime;
        _yAxis = Mathf.Clamp(_yAxis, yAxisLimitUp, yAxisLimitDown);

        this.transform.eulerAngles = new Vector3(_yAxis, _xAxis, this.transform.eulerAngles.z);
    }

    /// <summary>
    /// camera moves to hip or to gun iron sight
    /// </summary>
    private void CameraZoom()
    {
        if (AimMode)
        {
            _position = Vector3.Lerp(camera.transform.position, 
                                                     gunZoomPoint.position, 
                                                     zoomLerpSpeed * Time.deltaTime);
            _position.y = gunZoomPoint.position.y;
            camera.transform.position = _position;
        }
        else
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, 
                                                     hipPoint.position, 
                                                     zoomLerpSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// moves arm slightly slower then camera 
    /// to create a non-static image of the arms and gun
    /// </summary>
    private void ArmsLerp()
    {
        arms.transform.position = armsPointer.position;
        //arms.transform.rotation = Quaternion.Lerp(arms.transform.rotation, 
                                                  //armsPointer.rotation, 
                                                  //armsLerpSpeed * Time.deltaTime);
        arms.transform.rotation = armsPointer.rotation;
    }
}
