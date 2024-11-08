using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    
    public float gravityMultiplier = 2f;

    public bool isparkour = false;
    
    private Rigidbody _rb;
    
    private CapsuleCollider _collider;

    private Vector3 _direciton;

    private float x, y;

    internal Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        _collider = GetComponent<CapsuleCollider>();

        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isparkour == true) return;
        
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        _direciton = new Vector3(x,0,y);
        
        _animator.SetFloat("Speed",_direciton.magnitude);
    }

    private void FixedUpdate()
    {
        if (isparkour == true) return;

        Vector3 currentVelocity = _rb.linearVelocity;

        currentVelocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

        Vector3 movement = _direciton * speed * Time.deltaTime;
        _rb.linearVelocity = new Vector3(movement.x, currentVelocity.y, movement.z);
    }

    public void SetMove()
    {
        isparkour = !isparkour;

        _rb.isKinematic = isparkour;

        _collider.enabled = !isparkour;
    }
}
