using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSystem : MonoBehaviour
{
    [SerializeField] private float force = 12f;

    private Transform from;
    private Transform to;

    private Rigidbody _rigidbody;

    private bool isStart;

    public void SetPlay(Transform from, Transform to)
    {
        this.from = from;
        this.to = to;
        isStart = true;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.transform.position = from.position;
        _rigidbody.transform.LookAt(to);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isStart)
        {
            _rigidbody.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime);
        }        
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.layer == Globals.LAYER_PLAYER || collision.gameObject.layer == 3) 
            && collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            //rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime);
            rb.AddForce((to.position - transform.position).normalized * 10, ForceMode.Force);
        }
    }
}
