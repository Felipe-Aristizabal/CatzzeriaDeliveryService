using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 0.1f;
    public float maxSpeed = 20f;
    public float maxReverseSpeed = 7f;
    public float turnSpeed = 50f;
    public float brakePower = 0.2f;
    public float tiltAngle = 15f;

    private float currentSpeed = 0f;
    private bool isBraking = false;

    [Header("References")]
    private Rigidbody rb;
    private MotocycleSound audioManager; 
    [SerializeField] private PlayerPositionState _playerPosition;

    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioManager = GetComponent<MotocycleSound>();
    }

    void Update()
    {
        // Only process inputs and rotation if we are not restarting
        if (!_playerPosition.isRestart)
        {
            float moveInput = Input.GetAxis("Vertical");
            float turnInput = Input.GetAxis("Horizontal");

            bool isCurrentlyBraking = false;

            // -- ACCELERATION/BRAKE LOGIC
            if (moveInput > 0)
            {
                // If going backward and pressing forward, first move towards 0
                if (currentSpeed < 0)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, brakePower * Time.deltaTime);
                    isCurrentlyBraking = true;
                }
                else
                {
                    currentSpeed += acceleration * Time.deltaTime;
                    currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
                }
            }
            else if (moveInput < 0)
            {
                // If going forward and pressing backward, first move towards 0
                if (currentSpeed > 0)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, brakePower * Time.deltaTime);
                    isCurrentlyBraking = true;
                }
                else
                {
                    currentSpeed -= acceleration * Time.deltaTime;
                    currentSpeed = Mathf.Clamp(currentSpeed, -maxReverseSpeed, 0f);
                }
            }
            else
            {
                // Without input, smoothly move speed towards 0
                if (currentSpeed != 0)
                {
                    isCurrentlyBraking = true;
                }
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
            }

            // -- TURN AND TILT LOGIC (only if there's a noticeable speed)
            float turn = 0f;
            float tilt = 0f;
            if (Mathf.Abs(currentSpeed) > 0.1f)
            {
                turn = turnInput * turnSpeed * Time.deltaTime;
                tilt = -turnInput * tiltAngle;
            }

            // Apply rotation on Y-axis in Update for smooth turning
            transform.Rotate(0, turn, 0);

            // Apply tilt on Z-axis (while preserving current X and Y rotation)
            transform.localRotation = Quaternion.Euler(
                transform.localRotation.eulerAngles.x,
                transform.localRotation.eulerAngles.y,
                tilt
            );

            // Handle brake sound
            if (isCurrentlyBraking && !isBraking)
            {
                audioManager.PlayBrakeSound();
                isBraking = true;
            }
            else if (!isCurrentlyBraking && isBraking)
            {
                audioManager.StopBraking();
                isBraking = false;
            }
        }
    }

    void FixedUpdate()
    {
        // In FixedUpdate, we handle physics-related movement
        if (!_playerPosition.isRestart)
        {
            // Build horizontal velocity based on the motorcycle's forward direction
            Vector3 newHorizontalVelocity = transform.forward * currentSpeed;

            // Preserve the current y-velocity for proper gravity behavior
            newHorizontalVelocity.y = rb.velocity.y;

            // Assign the new velocity to the rigidbody
            rb.velocity = newHorizontalVelocity;
        }
    }   
}
