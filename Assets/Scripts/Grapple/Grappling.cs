using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform gunTip;
    [SerializeField] private LayerMask grappleableLayers;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private TwoBoneIKConstraint ikConstraint;

    [Header("Settings")]
    [SerializeField] private float maxGrappleDistance = 30f;
    [SerializeField] private float grappleDelayTime = 0.2f;
    [SerializeField] private float overshootYAxis = 5f;
    [SerializeField] private float grappleCooldown = 2f;

    private float grappleCooldownTimer;
    private Vector3 grapplePoint;
    private bool isGrapplingActive = false;
    private bool isGrappling = false;

    public KeyCode grappleKey = KeyCode.Mouse1;
    public KeyCode toggleGrappleKey = KeyCode.Alpha1;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        ikConstraint.weight = 0f;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        HandleGrappleInput();
        HandleCooldown();
    }

    private void HandleGrappleInput()
    {
        if (Input.GetKeyDown(grappleKey) && isGrapplingActive)
        {
            StartGrapple();
        }

        if (Input.GetKeyDown(toggleGrappleKey) && !isGrappling && !isGrapplingActive)
        {
            StartCoroutine(ActivateGrapple());
        }
        else if (Input.GetKeyDown(toggleGrappleKey) && isGrapplingActive && !isGrappling)
        {
            StartCoroutine(DeactivateGrapple());
        }
    }

    private void HandleCooldown()
    {
        if (grappleCooldownTimer > 0)
        {
            grappleCooldownTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
        }
    }

    private IEnumerator ActivateGrapple()
    {
        yield return SmoothTransition(0f, 1f);
        isGrapplingActive = true;
    }

    private IEnumerator DeactivateGrapple()
    {
        isGrapplingActive = false;
        yield return SmoothTransition(1f, 0f);
    }

    private IEnumerator SmoothTransition(float startWeight, float endWeight)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            ikConstraint.weight = Mathf.Lerp(startWeight, endWeight, t);
            yield return null;
        }
    }

    private void StartGrapple()
    {
        if (grappleCooldownTimer > 0) return;

        isGrappling = true;
        playerMove.freeze = true;

        if (TryGetGrapplePoint(out var hit))
        {
            grapplePoint = hit.point;
            StartCoroutine(ExecuteGrappleWithDelay(grappleDelayTime + 1));
        }
        else
        {
            grapplePoint = cameraTransform.position + cameraTransform.forward * maxGrappleDistance;
            StartCoroutine(StopGrappleWithDelay(grappleDelayTime + 1));
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, gunTip.position);
        StartCoroutine(DrawRope());
    }

    private bool TryGetGrapplePoint(out RaycastHit hit)
    {
        return Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxGrappleDistance, grappleableLayers);
    }

    private IEnumerator DrawRope()
    {
        Vector3 startPosition = gunTip.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            Vector3 segmentPosition = Vector3.Lerp(startPosition, grapplePoint, t);
            lineRenderer.SetPosition(1, segmentPosition);
            yield return null;
        }
    }

    private IEnumerator ExecuteGrappleWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ExecuteGrapple();
    }

    private void ExecuteGrapple()
    {
        playerMove.freeze = false;

        float yOffset = Mathf.Max(grapplePoint.y - (transform.position.y - 1f), overshootYAxis);
        playerMove.JumpToPosition(grapplePoint, yOffset);
    }

    private IEnumerator StopGrappleWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopGrapple();
    }

    public void StopGrapple()
    {
        playerMove.freeze = false;
        isGrappling = false;
        grappleCooldownTimer = grappleCooldown;
        lineRenderer.enabled = false;
    }
}
