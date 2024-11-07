using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    
    public float gravityMultiplier = 2f; 
    
    private Rigidbody _rb;

    private Animator _animator;

    private Vector3 _direciton;

    private float x, y;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        _direciton = new Vector3(x,0,y);
        
        _animator.SetFloat("Speed",_direciton.magnitude);
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = _rb.linearVelocity;

        currentVelocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

        Vector3 movement = _direciton * speed * Time.deltaTime;
        _rb.linearVelocity = new Vector3(movement.x, currentVelocity.y, movement.z);
    }
}
