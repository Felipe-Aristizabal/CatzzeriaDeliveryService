using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatStore : MonoBehaviour
{
    [Header("Store UI Elements")]
    [SerializeField] private GameObject hatStoreCanvas; // Canvas de la tienda de sombreros
    [SerializeField] private GameObject hatInventoryCanvas; // Canvas del inventario de sombreros
    [SerializeField] private GameObject shopColorCanvas;
    [SerializeField] private Button openInventoryButton; // Botón para volver al inventario desde la tienda
    [SerializeField] private Button openColorStoreButton;
    [SerializeField] private Button buyButton; // Botón para comprar el sombrero
    [SerializeField] private Button leftArrowButton; // Botón para moverse a la izquierda
    [SerializeField] private Button rightArrowButton; // Botón para moverse a la derecha
    [SerializeField] private Text hatNameText; // Texto que muestra el nombre del sombrero
    [SerializeField] private Text dataText; // Texto para mensajes (como "no tienes suficientes monedas")

    [Header("Store Data")]
    [SerializeField] private List<GameObject> storeHats; // Lista de sombreros disponibles en la tienda

    [Header("Inventory Reference")]
    [SerializeField] private HatIventory hatIventory;

    [Header("Game Management")]
    [SerializeField] private GameManager gameManager; // Referencia al GameManager
    [SerializeField] private UIController uIController; // Referencia al UIController

    private int currentHatIndex = 0;

    void Start()
    {
        // Inicialmente el Canvas de la tienda está desactivado
        hatStoreCanvas.SetActive(false);

        // Asignar las funciones a los botones
        leftArrowButton.onClick.AddListener(SelectPreviousHat);
        rightArrowButton.onClick.AddListener(SelectNextHat);
        buyButton.onClick.AddListener(BuyHat);
        openInventoryButton.onClick.AddListener(OpenInventory);
        openColorStoreButton.onClick.AddListener(OpenColorStore);

        // Inicializar el texto
        dataText.text = "";
        UpdateHatName();
    }

    // Función para abrir el inventario desde la tienda
    void OpenInventory()
    {
        hatStoreCanvas.SetActive(false); // Apagar el Canvas de la tienda
        hatInventoryCanvas.SetActive(true); // Encender el Canvas del inventario
    }
    void OpenColorStore()
    {
        hatStoreCanvas.SetActive(false); // Apagar el Canvas de la tienda
        shopColorCanvas.SetActive(true); // Encender el Canvas del inventario
    }
    // Función para seleccionar el sombrero anterior
    void SelectPreviousHat()
    {
        currentHatIndex--;
        if (currentHatIndex < 0)
        {
            currentHatIndex = storeHats.Count - 1; // Volver al final si se pasa del principio
        }
        UpdateHatName();
    }

    // Función para seleccionar el siguiente sombrero
    void SelectNextHat()
    {
        currentHatIndex++;
        if (currentHatIndex >= storeHats.Count)
        {
            currentHatIndex = 0; // Volver al principio si se pasa del final
        }
        UpdateHatName();
    }

    void BuyHat()
    {
        // Inicializar el texto "Data" como vacío
        dataText.text = "";

        if (storeHats.Count == 0)
        {
            dataText.text = "No hats available in the store";
            return;
        }

        int hatCost = 13; // Costo del sombrero (puedes personalizar esto según el sombrero)
        if (uIController.coins < hatCost)
        {
            dataText.text = "You don't have enough money";
            return;
        }

        GameObject selectedHat = storeHats[currentHatIndex];

        // Restar monedas usando el método del GameManager
        gameManager.SubtractCoins(hatCost);

        // Mover el sombrero a la lista de inventario en HatInventory
        hatIventory.AddHatToInventory(selectedHat);

        // Eliminar el sombrero de la tienda
        storeHats.RemoveAt(currentHatIndex);

        // Ajustar el índice y actualizar el texto
        currentHatIndex = Mathf.Clamp(currentHatIndex, 0, storeHats.Count - 1);
        UpdateHatName();
    }
    void UpdateHatName()
    {
        if (storeHats.Count > 0)
        {
            hatNameText.text = storeHats[currentHatIndex].name;
        }
        else
        {
            hatNameText.text = "No hats available";
            buyButton.gameObject.SetActive(false);
            leftArrowButton.gameObject.SetActive(false);
            rightArrowButton.gameObject.SetActive(false);
        }
    }
}
