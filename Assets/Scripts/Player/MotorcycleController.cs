using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleController : MonoBehaviour
{
    public float acceleration = 0.1f;
    public float maxSpeed = 20f;
    public float maxReverseSpeed = 7f;
    public float turnSpeed = 50f;
    public float brakePower = 0.2f;
    public float tiltAngle = 15f;

    private float currentSpeed = 0f;
    private Rigidbody rb;
    private MotocycleSound audioManager; // Referencia al script MotorcycleAudio
    private bool isBraking = false;

    [SerializeField] private PlayerPositionState _playerPosition;
    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtener el componente RigidBody
        audioManager = GetComponent<MotocycleSound>(); // Obtener referencia al script de audio
    }

    void Update()
    {
        if (!_playerPosition.isRestart)
        {
            float moveInput = Input.GetAxis("Vertical");
            float turnInput = Input.GetAxis("Horizontal");
            float turn = 0f;
            float tilt = 0f;

            // Detectar si la moto está frenando
            bool isCurrentlyBraking = false;

            // Lógica de frenado y aceleración
            if (moveInput > 0)
            {
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
                if (currentSpeed != 0)
                {
                    isCurrentlyBraking = true;
                }

                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
            }

            // Solo permitir la rotación si la moto está en movimiento
            if (Mathf.Abs(currentSpeed) > 0.1f)
            {
                turn = turnInput * turnSpeed * Time.deltaTime;
                tilt = -turnInput * tiltAngle;
            }

            // Aplicar el movimiento
            Vector3 moveDirection = transform.forward * currentSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDirection);

            // Aplicar la rotación solo si hay velocidad
            transform.Rotate(0, turn, 0);

            // Aplicar la inclinación
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,
                transform.localRotation.eulerAngles.y, tilt);

            // Reproducir el sonido de freno si está frenando
            if (isCurrentlyBraking && !isBraking)
            {
                audioManager.PlayBrakeSound(); // Notificar al sistema de sonido
                isBraking = true;
            }
            else if (!isCurrentlyBraking && isBraking)
            {
                audioManager.StopBraking(); // Detener el estado de frenado
                isBraking = false;
            }
        }else{
            //Debug.Log("im parching");
            }
    }
}

