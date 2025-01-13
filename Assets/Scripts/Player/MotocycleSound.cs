using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotocycleSound : MonoBehaviour
{
    // ---------------------- Sonido del motor ----------------------
    public AudioSource motorAudioSource; // AudioSource para el sonido del motor
    public AudioClip motorSound; // Sonido del motor
    public float minPitch = 0.8f; // Pitch mínimo para sonido lento
    public float maxPitch = 2.0f; // Pitch máximo para sonido rápido
    private MotorcycleController motoMovement; // Referencia al script de movimiento

    // ---------------------- Sonido de frenado ----------------------
    public AudioSource brakeAudioSource; // AudioSource para el freno
    public AudioClip brakeSound; // Sonido de frenado
    private bool isBraking = false; // Estado de frenado

    // ---------------------- Radio de la moto ----------------------
    public AudioSource radioAudioSource; // AudioSource para la radio
    public AudioClip[] songs; // Lista de canciones (10 canciones)
    private int currentSongIndex = 0; // Índice de la canción actual
    public float fadeDuration = 1.0f; // Duración del fade-out y fade-in

    // ---------------------- Slider de volumen ----------------------
    public Slider volumeSlider;

    void Start()
    {
        // Configuración del motor
        motoMovement = GetComponent<MotorcycleController>(); // Obtener el componente de movimiento de la moto
        motorAudioSource.clip = motorSound;
        motorAudioSource.loop = true; // Hacer que el sonido del motor sea un loop
        motorAudioSource.Play(); // Reproducir el sonido del motor

        // Configuración del volumen inicial en la mitad (0.5)
        float initialVolume = 0.5f;

        motorAudioSource.volume = initialVolume;
        brakeAudioSource.volume = initialVolume;
        radioAudioSource.volume = initialVolume;

        // Configuración de la radio
        if (songs.Length > 0)
        {
            radioAudioSource.clip = songs[currentSongIndex];
            radioAudioSource.Play();
        }

        // Configuración inicial del slider
        if (volumeSlider != null)
        {
            volumeSlider.value = initialVolume; 
            volumeSlider.onValueChanged.AddListener(SetVolume); // Escucha cambios en el slider
        }
    }

    void Update()
    {
        // ------------------- Sonido del motor -------------------
        // Cambiar el pitch del sonido del motor según la velocidad actual de la moto
        float speed = Mathf.Abs(motoMovement.CurrentSpeed); // Tomar la velocidad absoluta desde la propiedad
        float pitch = Mathf.Lerp(minPitch, maxPitch, speed / motoMovement.maxSpeed); // Interpolar el pitch según la velocidad
        motorAudioSource.pitch = pitch; // Aplicar el nuevo pitch

        // ------------------- Sonido de freno -------------------
        // Lógica de frenado: el script MotorcycleController notificará si la moto está frenando
        if (!radioAudioSource.isPlaying)
        {
            StartCoroutine(FadeOutAndNextSong(fadeDuration)); // Iniciar el fade-out cuando la canción termine
        }

        // Cambiar de canción cuando el jugador presiona la tecla 'R'
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(FadeOutAndNextSong(fadeDuration)); // Iniciar el fade-out al presionar 'R'
        }
        
        // Actualiza el slider con el volumen actual
        if (volumeSlider != null)
        {
            volumeSlider.value = motorAudioSource.volume;
        }
    }

    // Función para reproducir el sonido de freno
    public void PlayBrakeSound()
    {
        if (!isBraking)
        {
            brakeAudioSource.PlayOneShot(brakeSound); // Reproducir el sonido de frenado
            isBraking = true;
        }
    }

    // Función para dejar de frenar
    public void StopBraking()
    {
        isBraking = false;
    }

    // ------------------- Funciones de la radio -------------------
    void PlayNextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % songs.Length;
        radioAudioSource.clip = songs[currentSongIndex];
        radioAudioSource.Play();
    }

    IEnumerator FadeOutAndNextSong(float fadeDuration)
    {
        while (radioAudioSource.volume > 0)
        {
            radioAudioSource.volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        PlayNextSong();

        radioAudioSource.volume = 0f;

        while (radioAudioSource.volume < 0.2f)
        {
            radioAudioSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
    // ---------------------- Control del Volumen ----------------------
    public void SetVolume(float volume)
    {
        motorAudioSource.volume = volume;
        brakeAudioSource.volume = volume;
        radioAudioSource.volume = volume;
    }
}
