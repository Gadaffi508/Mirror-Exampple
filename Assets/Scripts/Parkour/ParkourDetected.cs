using System;
using UnityEngine;

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
    
    [Space]
    [Header("Gizmos Settings")]
    [HideInInspector]
    public bool showOnDrawGizmos = false;

    private bool isJumped = false;

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

            _offset = new Vector3(detectionPoint.x,_parkourPos.GetComponent<Collider>().bounds.extents.y * 2 + offsetHeight,detectionPoint.z);
            
            isJumped = true;
        }
        else
        {
            isJumped = false;
            
            _parkourPos = null;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJumped == true)
        {
            Vector3 _offsetHeight = new Vector3(_offset.x,_offset.y + 1,_offset.z);
            
            transform.position = _offsetHeight;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if(showOnDrawGizmos == true) return;
        
        if (_parkourPos != null)
        {
            Vector3 parkourCenter = _parkourPos.position;

            Vector3 detectionPoint = GetClosestPointOnCube(parkourCenter);

            Gizmos.DrawLine(orbital.position, detectionPoint);
            
            Vector3 offset = new Vector3(detectionPoint.x,_parkourPos.GetComponent<Collider>().bounds.extents.y * 2 + offsetHeight,detectionPoint.z);

            Gizmos.DrawSphere(offset, 0.2f);

        }
        else
        {
            Gizmos.DrawRay(orbital.position, orbital.forward * detectionRange);
        }
    }

    private Vector3 GetClosestPointOnCube(Vector3 cubeCenter)
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
