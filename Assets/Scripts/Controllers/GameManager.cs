using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIController uIController;
    
    [Header("Menu Items")]
    [SerializeField] private GameObject catPizzaEnv;
    [SerializeField] private GameObject catMenu;
    [SerializeField] private GameObject catMoto;
    [SerializeField] private GameObject catPizzaBox;

    [SerializeField] private GameObject catAnim;
    
    [Header("Game Items")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject minimapCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mainCanva;

    [SerializeField] private Rigidbody rb;

    [Header("Feedbacks")]
    public int countPizza = 0;

    public GameObject endFeedback;
    public Animator deliveryFeedback;

    // Variables para guardar la posición inicial del jugador
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    

    private void Start()
    {
        // Guardar la posición y rotación inicial del jugador
        _initialPosition = player.transform.position;
        _initialRotation = player.transform.rotation;

        // rb = player.GetComponent<Rigidbody>(); // Si quieres habilitar esto más tarde
    }

    private void FixedUpdate() 
    {
        if (countPizza >= 3)
        {
            endFeedback.SetActive(true);
        }
    }

    // Method for update the coins TXT
    public void AddCoins(int amount)
    {
        uIController.coins += amount; // Sumar el valor pasado como parámetro
        uIController.UpdateCoinText(); // Actualizar el texto
        countPizza++;
    }

    public void SwitchCatPizza(bool value)
    {
        StartCoroutine((PlayAnims()));
    }

    public void OpenSpecificGameObject(GameObject gameObjectToOpen)
    {
        gameObjectToOpen.SetActive(true);
    }

    IEnumerator PlayAnims()
    {
        catMenu.GetComponent<Animator>().SetBool("isPlaying", true);

        yield return new WaitForSeconds(1f);
        catMenu.SetActive(false);
        catPizzaBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        catMoto.GetComponent<Animator>().SetBool("isPlaying", true);
        
        yield return new WaitForSeconds(2f);
        mainCamera.SetActive(true);
        player.SetActive(true);
        
        Debug.Log(_initialPosition);
        mainCanva.SetActive(true);
        minimapCamera.SetActive(true);
        
        catPizzaEnv.SetActive(false);
    }

    public IEnumerator RestartScene()
    {
        
        catAnim.SetActive(true);
        
        
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // Mover el jugador a la posición inicial guardada
        player.transform.position = _initialPosition;
        player.transform.rotation = _initialRotation;

        catPizzaEnv.SetActive(true);
        mainCamera.SetActive(false);
        
        
        catPizzaEnv.SetActive(true);
        catMenu.SetActive(true);
        catPizzaBox.SetActive(false);
        
        mainCanva.SetActive(false);
        minimapCamera.SetActive(false);
        yield return new WaitForSeconds(2.8f);
        catAnim.SetActive(false);
        
        
        rb.constraints = RigidbodyConstraints.None;
        
        player.GetComponent<PlayerPositionState>().isRestart = false;
        
        player.SetActive(false);
        
        
        catPizzaBox.GetComponent<Animator>().Rebind();
        catMenu.GetComponent<Animator>().Rebind();
        catMoto.GetComponent<Animator>().Rebind();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
