using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ParkourDetected : MonoBehaviour
{
    [Header("Parkour Settings")]
    public LayerMask parkourLayer; 
    public Transform orbital; 
    public float detectionRange = 2f;
    public float offsetHeight = 0.5f;

    private RaycastHit _hit;
    private Transform _parkourPos;
    private Vector3 _offset;

    private PlayerMove _move;
    
    [Space]
    [Header("Gizmos Settings")]
    [HideInInspector]
    public bool showOnDrawGizmos = false;
    
    [Header("IK Settings")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    public TwoBoneIKConstraint left;
    public TwoBoneIKConstraint right;
    
    [Space]
    [Header("Move Options")]
    public float moveSpeed = 3f; 
    [Space]

    private bool isJumped = false;

    private void Start()
    {
        _move = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        DetectParkour();
    }

    void DetectParkour()
    {
        Vector3 direction = orbital.forward;

        if (Physics.Raycast(orbital.position, direction, out _hit, detectionRange, parkourLayer))
        {
            if(_hit.transform == null) return;
            
            _parkourPos = _hit.transform;
            
            Vector3 detectionPoint = GetClosestPointOnCube(_hit.transform.position);

            _offset = new Vector3(detectionPoint.x,
                                  _parkourPos.GetComponent<Collider>().bounds.extents.y * 2 + offsetHeight,
                                  detectionPoint.z);
            
            isJumped = true;
        }
        else
        {
            isJumped = false;
            _parkourPos = null;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJumped == true)
        {
            _move.SetMove();
            
            Vector3 xOffset = new Vector3(_offset.x, transform.position.y, _offset.z - 0.3f);

            left.weight = 1;
            right.weight = 1;
            
            StartCoroutine(MoveToPosition(xOffset, 0.5f));
            StartCoroutine(WaitForParkourAnimation());
        }
        
        leftHandTarget.position = _offset + transform.right * -0.2f;
        rightHandTarget.position = _offset + transform.right * 0.2f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if(showOnDrawGizmos == true) return;
        
        if (_parkourPos != null)
        {
            Vector3 parkourCenter = _parkourPos.position;

            Vector3 detectionPoint = GetClosestPointOnCube(parkourCenter);

            Gizmos.DrawLine(orbital.position, detectionPoint);

            Vector3 xOffset = new Vector3(detectionPoint.x, detectionPoint.y, detectionPoint.z - 0.3f);
            Gizmos.DrawSphere(xOffset, 0.2f);
            
            Vector3 offset = new Vector3(detectionPoint.x,
                                         _parkourPos.GetComponent<Collider>().bounds.extents.y * 2 + offsetHeight,
                                         detectionPoint.z);

            Gizmos.DrawSphere(offset, 0.2f);

        }
        else
        {
            Gizmos.DrawRay(orbital.position, orbital.forward * detectionRange);
        }
    }
    
    IEnumerator MoveToPosition(Vector3 targetPosition, float distance)
    {
        float positionThreshold = distance;

        while (Vector3.Distance(transform.position, targetPosition) > positionThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) <= positionThreshold + 0.2f)
            {
                break;
            }

            yield return null;
        }

        transform.position = targetPosition;
    }
    
    IEnumerator WaitForParkourAnimation()
    {
        float cubeHeight = _parkourPos.GetComponent<Collider>().bounds.extents.y * 2;
        float adjustedWaitTime = cubeHeight / moveSpeed; 
        
        Vector3 _offsetHeight = new Vector3(_offset.x, _offset.y + 0.5f, _offset.z);
        
        StartCoroutine(MoveToPosition(_offsetHeight, 0.001f));
        
        yield return new WaitForSeconds(adjustedWaitTime);

        transform.position = _offsetHeight;
        
        left.weight = 0;
        right.weight = 0;

        _move.SetMove();
    }

    Vector3 GetClosestPointOnCube(Vector3 cubeCenter)
    {
        Vector3 direction = orbital.position - cubeCenter;

        Vector3 extents = _parkourPos.GetComponent<Collider>().bounds.extents;

        Vector3 closestPoint = cubeCenter + new Vector3(
            Mathf.Clamp(direction.x, -extents.x, extents.x),
            Mathf.Clamp(direction.y, -extents.y, extents.y),
            Mathf.Clamp(direction.z, -extents.z, extents.z)
        );

        return closestPoint;
    }
}
