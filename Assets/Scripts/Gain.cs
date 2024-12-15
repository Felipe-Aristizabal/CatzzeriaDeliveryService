using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class Gain : MonoBehaviour
{
    [SerializeField] private GameManager gameManager; // Referencia al script CoinManager
    public int pointsToAdd = 10; // Valor de puntos a sumar cuando el jugador entre

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.AddCoins(pointsToAdd);
            gameManager.deliveryFeedback.SetTrigger("Delivered");
            Destroy(gameObject.transform.parent.GetChild(2).gameObject);
            Destroy(this.gameObject);
        }
    }
}
