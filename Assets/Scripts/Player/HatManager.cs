using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject hatInventoryCanvas; // Canvas del inventario de sombreros
    [SerializeField] private GameObject hatStoreCanvas; // Canvas de la tienda de sombreros

    [Header("UI Elements")]
    [SerializeField] private Button openStoreButton; // Botón para abrir la tienda desde el inventario
    [SerializeField] private Button openInventoryButton; // Botón para volver al inventario desde la tienda
    [SerializeField] private Button leftArrowButton; // Botón para moverse a la izquierda
    [SerializeField] private Button rightArrowButton; // Botón para moverse a la derecha
    [SerializeField] private Button buyButton; // Botón para comprar el sombrero
    [SerializeField] private Text itemNameText; // Texto que muestra el nombre del sombrero
    [SerializeField] private Text dataText; // Texto para mensajes (como "no tienes suficientes monedas")

    [Header("Lists")]
    [SerializeField] private List<GameObject> inventoryHats; // Lista de sombreros en el inventario
    [SerializeField] private List<GameObject> storeHats; // Lista de sombreros en la tienda

    [Header("Game Management")]
    [SerializeField] private GameManager gameManager; // Referencia al GameManager
    [SerializeField] private UIController uIController; // Referencia al UIController

    private int currentItemIndex = 0; // Índice del sombrero actual

    void Start()
    {
        // Inicialmente ambos Canvas están desactivados
        hatInventoryCanvas.SetActive(false);
        hatStoreCanvas.SetActive(false);

        // Asignar las funciones a los botones
        openStoreButton.onClick.AddListener(OpenStore);
        openInventoryButton.onClick.AddListener(OpenInventory);
        leftArrowButton.onClick.AddListener(SelectPreviousItem);
        rightArrowButton.onClick.AddListener(SelectNextItem);
        buyButton.onClick.AddListener(BuyHat);

        // Inicializar el texto
        dataText.text = "";
        UpdateItemName();
    }

    void Update()
    {
        // Alternar el Canvas del inventario con la tecla "X"
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (hatStoreCanvas.activeSelf) return; // No hacer nada si la tienda está activa
            ToggleCanvas(hatInventoryCanvas);
        }

        // Cerrar la tienda con "X" y reanudar el tiempo
        if (Input.GetKeyDown(KeyCode.X) && hatStoreCanvas.activeSelf)
        {
            hatStoreCanvas.SetActive(false);
            Time.timeScale = 1; // Reanudar el tiempo
        }
    }

    // Función para alternar entre abrir y cerrar un Canvas
    private void ToggleCanvas(GameObject canvas)
    {
        bool isActive = canvas.activeSelf;
        canvas.SetActive(!isActive);

        // Pausar o reanudar el tiempo
        Time.timeScale = isActive ? 1 : 0;
    }

    // Función para abrir la tienda desde el inventario
    private void OpenStore()
    {
        hatInventoryCanvas.SetActive(false);
        hatStoreCanvas.SetActive(true);
    }

    // Función para volver al inventario desde la tienda
    private void OpenInventory()
    {
        hatStoreCanvas.SetActive(false);
        hatInventoryCanvas.SetActive(true);
    }

    // Función para seleccionar el sombrero anterior
    private void SelectPreviousItem()
    {
        currentItemIndex--;
        if (currentItemIndex < 0)
        {
            currentItemIndex = GetCurrentList().Count - 1;
        }
        UpdateItemName();
    }

    // Función para seleccionar el siguiente sombrero
    private void SelectNextItem()
    {
        currentItemIndex++;
        if (currentItemIndex >= GetCurrentList().Count)
        {
            currentItemIndex = 0;
        }
        UpdateItemName();
    }

    // Función para comprar un sombrero
    private void BuyHat()
    {
        dataText.text = ""; // Inicializar mensaje

        if (storeHats.Count == 0)
        {
            dataText.text = "No hats available in the store";
            return;
        }

        int hatCost = 2; // Costo del sombrero
        if (uIController.coins < hatCost)
        {
            dataText.text = "You don't have enough money";
            return;
        }

        // Restar monedas al jugador
        gameManager.SubtractCoins(hatCost);

        // Mover el sombrero de la tienda al inventario
        GameObject selectedHat = storeHats[currentItemIndex];
        inventoryHats.Add(selectedHat);
        storeHats.RemoveAt(currentItemIndex);

        // Ajustar el índice y actualizar el texto
        currentItemIndex = Mathf.Clamp(currentItemIndex, 0, storeHats.Count - 1);
        UpdateItemName();
    }

    // Función para obtener la lista actual (inventario o tienda)
    private List<GameObject> GetCurrentList()
    {
        return hatStoreCanvas.activeSelf ? storeHats : inventoryHats;
    }

    // Función para actualizar el nombre del sombrero en el texto
    private void UpdateItemName()
    {
        List<GameObject> currentList = GetCurrentList();
        if (currentList.Count > 0)
        {
            itemNameText.text = currentList[currentItemIndex].name;
        }
        else
        {
            itemNameText.text = "No items available";
        }
    }
}

