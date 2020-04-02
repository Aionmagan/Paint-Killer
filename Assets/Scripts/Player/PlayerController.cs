using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private CameraController _camController;
    private Health _health;
    private Vector3 _movement = Vector3.zero;
    private RaycastHit _hitInfo;
    private float _lastSpeed;
    private int _port;

    [Header ("Player Settings")]
    public float moveSpeed;
    public float jumpForce = 7;
    public float onGroundRayDistance = 2f;
    public LayerMask layermask;
    public new GameObject camera;
    
    // Start is called before the first frame update
    private void Start()
    {
        _health = GetComponent<Health>();
        _port = (int)GetComponentInParent<PlayerPortSelect>().playerPort;
        _rigidbody = GetComponent<Rigidbody>();
        _lastSpeed = moveSpeed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //return if health is 0 (to prevent movement from player)
        if (_health.health <= 0) { return; }
        
        if(!_camController)
        {
            SetCamera();
            return;
        }

        PlayerMovment();
    }

    private void LateUpdate()
    {
        //return if health is 0 (to prevent movement from player)
        if (_health.health <= 0) { return; }

        RotateToCameraDirection();
    }

    /// <summary>
    /// raycast for gound check
    /// this fucntion also papulates the _hitinfo variable
    /// </summary>
    public bool OnGround()
    {
        if(Physics.Raycast(this.transform.position, Vector3.down, out _hitInfo, onGroundRayDistance, layermask))
        {
            Debug.DrawLine(this.transform.position, Vector3.down * onGroundRayDistance, Color.magenta);
            return true;
        }
        return false;
    }

    /// <summary>
    /// player movement, velocity change (function call)
    /// player jump
    /// </summary>
    private void PlayerMovment()
    {
        ChangeVelocity(_camController.AimMode);

        if (OnGround())
        {
            //check for slope 
            OnSlope();

            //try using vector movement instead of rigidvelocity later on
            var lastYvel = _rigidbody.velocity.y;
            _movement = Controller.MainAxes(_port) * moveSpeed * 3 * Time.deltaTime;
            _movement = _camController.playerPointer.transform.TransformDirection(_movement);

            if (Controller.Action0(Button.DOWN, _port))
            {
                lastYvel = jumpForce;
            }

            _rigidbody.velocity = new Vector3(_movement.x, lastYvel, 
                                              _movement.z);
        }
    }

    /// <summary>
    /// adjust forwards for slope
    /// (being called on PlayerMomvent())
    /// </summary>
    private void OnSlope()
    {
        //if (_hitInfo.normal == Vector3.up) { return; }

        _camController.playerPointer.forward = Vector3.Cross(_hitInfo.normal, -transform.right);
        //_rigidbody.velocity += Vector3.down * 10;
        //var forwardSlop = Vector3.Cross(transform.right, _hitInfo.normal);
        //_camController.playerPointer.eulerAngles = forwardSlop;
    }

    /// <summary>
    /// velocity change for run walk and aim 
    /// </summary>
    private void ChangeVelocity(in bool change)
    {
        if (Controller.Action4(Button.HOLD, _port) && !_camController.AimMode && Controller.MainVertical(_port) > 0.9f)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, _lastSpeed * 1.5f, 2 * Time.deltaTime);
        }else 
        { 
            if (change)
            {
                moveSpeed = (_lastSpeed / 2.3f);
            }
            else
            {
                moveSpeed = _lastSpeed;
            }
        }
    }

    /// <summary>
    /// to rotate the player in the direction of the camera 
    /// (being called by late update)
    /// </summary>
    private void RotateToCameraDirection()
    {
        if (_camController)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, _camController.transform.eulerAngles.y, this.transform.eulerAngles.z);
            //bone.rotation = Quaternion.Euler(bone.transform.eulerAngles.x, bone.transform.eulerAngles.y, -camObject.transform.eulerAngles.x);
        }
    }

    /// <summary>
    /// set camera component 
    /// not using tags because more then one camera exist in this game
    /// </summary>
    public void SetCamera()
    {
        _camController = camera.GetComponent<CameraController>();
    }
}
