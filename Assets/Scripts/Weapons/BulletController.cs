using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Bullet Settings")]
    public float speed;

    [HideInInspector]
    public Transform direction;
    [HideInInspector]
    public int port;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.velocity = this.transform.forward * speed * Time.deltaTime;

        Destroy(this.gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
