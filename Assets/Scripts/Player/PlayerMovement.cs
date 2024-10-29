using System;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController controller = null;

    [SerializeField] private GameObject camera = null;

    [SerializeField] private GameObject feet = null;

    [SerializeField] private LayerMask groundMask = new LayerMask();

    [SerializeField] private float jumpHeight = 2f;

    [SerializeField] private float speed = 5;

    private float gravity = -9.81f;

    private float checkRadius = 0.4f;

    private Vector3 velocity;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isLocalPlayer == false)
        {
            camera.SetActive(false);
            return;
        }

        bool isGrounded = Physics.CheckSphere(feet.transform.position, checkRadius, groundMask);

        if (isGrounded && velocity.y <= 0f)
        {
            velocity.y = -2f;
        }
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * y;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            if(!isGrounded) return;

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
