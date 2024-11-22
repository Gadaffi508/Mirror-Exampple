using System;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private PlayerMove _pm;

    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;
    
    public float maxGrappleDistance;
    public float graplleDelayTime;
    public float overShootYAxis;

    private Vector3 _grapplePoint,_currentGrapplePoint;

    public float grapplingCd;
    private float _grapplingCdTimer;

    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool _grappling = false;
    
    private void Start()
    {
        _pm = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(grappleKey))
            StartGrapple();

        if (_grapplingCdTimer > 0)
            _grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(_grappling)
            DrawRope(gunTip.position,_grapplePoint);
    }

    void StartGrapple()
    {
        if (_grapplingCdTimer > 0) return;

        _grappling = true;

        _pm.freeze = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position,cam.forward,out hit,maxGrappleDistance,whatIsGrappleable))
        {
            _grapplePoint = hit.point;
            
            Invoke(nameof(ExecuteGrapple),graplleDelayTime);
        }
        else
        {
            _grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            
            Invoke(nameof(StopGrapple),graplleDelayTime);
        }

        lr.enabled = true;
    }

    void DrawRope(Vector3 startPos, Vector3 grapplePoint)
    {
        startPos = Vector3.Lerp(startPos, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0,startPos);
        lr.SetPosition(1,grapplePoint);
    }

    void ExecuteGrapple()
    {
        _pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplepointRelativeYPos = _grapplePoint.y - lowestPoint.y;
        float highHestPointOnArc = grapplepointRelativeYPos + overShootYAxis;

        if (grapplepointRelativeYPos < 0) highHestPointOnArc = overShootYAxis;
        
        _pm.JumpToPosition(_grapplePoint,highHestPointOnArc);
        
        Invoke(nameof(StopGrapple),1f);
    }

    public void StopGrapple()
    {
        _pm.freeze = false;
        
        _grappling = false;

        _grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }
}
