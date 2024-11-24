using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    
    public float gravityMultiplier = 2f;

    public bool isparkour = false;

    public bool freeze,activeGrapple = false;

    private bool _enableMovementOnNextTouch;
    
    private Rigidbody _rb;
    
    private CapsuleCollider _collider;

    private Vector3 _direciton, _velocityToSet;

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
        if (freeze)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }
        
        if (activeGrapple == true) return;
        
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        _direciton = new Vector3(x,0,y);
        
        _animator.SetFloat("Speed",_direciton.magnitude);
    }

    private void FixedUpdate()
    {
        if (freeze == true) return;
        if (activeGrapple == true) return;

        Vector3 currentVelocity = _rb.linearVelocity;

        currentVelocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        
        Vector3 cameraForward = Camera.main.transform.forward;
        
        cameraForward.y = 0;
        
        Vector3 moveDirection = cameraForward * _direciton.z + Camera.main.transform.right * _direciton.x;

        Vector3 movement = moveDirection * speed * Time.deltaTime;
        _rb.linearVelocity = new Vector3(movement.x, currentVelocity.y, movement.z);
    }

    public void SetMove()
    {
        isparkour = !isparkour;

        _rb.isKinematic = isparkour;

        _collider.enabled = !isparkour;
    }

    public void JumpToPosition(Vector3 targetPos, float trajectoryHeight)
    {
        activeGrapple = true;
        
        _velocityToSet = CalculateJumpVelocity(transform.position,targetPos,trajectoryHeight);
        
        Invoke(nameof(SetVelocity),0.1f);
        
        Invoke(nameof(ResetRestrictions),5f);
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_enableMovementOnNextTouch)
        {
            _enableMovementOnNextTouch = false;
            ResetRestrictions();
            
            GetComponent<Grappling>().StopGrapple();
        }
    }

    void SetVelocity()
    {
        _enableMovementOnNextTouch = true;
        
        _rb.linearVelocity = _velocityToSet;
    }

    Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocity = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
                                               + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocity;
    }
}
