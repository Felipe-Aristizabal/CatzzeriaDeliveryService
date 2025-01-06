using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotoShop : MonoBehaviour
{
    public Material[] shopMaterials; // Lista de materiales disponibles en la tienda
    [SerializeField] private MotoColorChanger motoColorChanger; // Referencia al script MotoColorChanger
    [SerializeField] private GameManager gameManager; // Referencia al GameManager
    [SerializeField] private UIController uIController; //Referencia al UIController
    [SerializeField] private Text dataText; //Referencia al data Text
    [SerializeField] private GameObject shopCanvas; // Canvas de la tienda
    [SerializeField] private Text materialNameText; // Texto que mostrará el nombre del material
    [SerializeField] private Button buyButton; // Botón para comprar el material
    [SerializeField] private Button leftArrowButton; // Botón para moverse a la izquierda
    [SerializeField] private Button rightArrowButton; // Botón para moverse a la derecha

    private int currentMaterialIndex = 0; // Índice del material actual en la tienda

    void Start()
    {
        // Inicialmente el Canvas de la tienda está desactivado
        shopCanvas.gameObject.SetActive(false);

        // Asignar las funciones a los botones
        leftArrowButton.onClick.AddListener(SelectPreviousMaterial);
        rightArrowButton.onClick.AddListener(SelectNextMaterial);
        buyButton.onClick.AddListener(BuyMaterial);

        dataText.text = "";
    }

    void Update()
    {
        // Mostrar/ocultar la tienda al presionar "T"
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleShopCanvas();
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1; // Reanudar el tiempo
            }
            else
            {
                Time.timeScale = 0; // Detener el tiempo
            }
        }
    }

    // Función para mostrar/ocultar el Canvas de la tienda
    void ToggleShopCanvas()
    {
        bool isActive = shopCanvas.gameObject.activeSelf;
        shopCanvas.gameObject.SetActive(!isActive);

        // Actualizar el nombre del material si la tienda está activa
        if (!isActive)
        {
            UpdateMaterialName();
        }
    }

    // Función para seleccionar el material anterior en la lista
    void SelectPreviousMaterial()
    {
        currentMaterialIndex--;
        if (currentMaterialIndex < 0)
        {
            currentMaterialIndex = shopMaterials.Length - 1;
        }
        UpdateMaterialName();
    }

    // Función para seleccionar el siguiente material en la lista
    void SelectNextMaterial()
    {
        currentMaterialIndex++;
        if (currentMaterialIndex >= shopMaterials.Length)
        {
            currentMaterialIndex = 0;
        }
        UpdateMaterialName();
    }

    // Función para comprar el material seleccionado
    void BuyMaterial()
    {
        // Inicializar el texto "Data" como vacío
        dataText.text = "";

        if (shopMaterials.Length == 0)
        {
            //Debug.LogWarning("No hay más materiales");
            return;
        }

        int materialCost = 10; 
        if (uIController.coins < materialCost)
        {
            dataText.text = "You don't have enough money";
            return;
        }

        Material selectedMaterial = shopMaterials[currentMaterialIndex];

        // Restar monedas usando el método del GameManager
        gameManager.SubtractCoins(materialCost);

        // Agregar el material al array de MotoColorChanger
        AddMaterialToMotoColorChanger(selectedMaterial);

        // Eliminar el material del array de la tienda
        RemoveMaterialFromShop(currentMaterialIndex);

        // Actualizar el índice y el texto
        currentMaterialIndex = Mathf.Clamp(currentMaterialIndex, 0, shopMaterials.Length - 1);
        UpdateMaterialName();
    }

    // Función para agregar un material al array de MotoColorChanger
    void AddMaterialToMotoColorChanger(Material material)
    {
        Material[] updatedMaterials = new Material[motoColorChanger.materials.Length + 1];
        motoColorChanger.materials.CopyTo(updatedMaterials, 0);
        updatedMaterials[updatedMaterials.Length - 1] = material;
        motoColorChanger.materials = updatedMaterials;
    }

    // Función para eliminar un material del array de la tienda
    void RemoveMaterialFromShop(int index)
    {
        Material[] updatedMaterials = new Material[shopMaterials.Length - 1];
        int j = 0;
        for (int i = 0; i < shopMaterials.Length; i++)
        {
            if (i != index)
            {
                updatedMaterials[j] = shopMaterials[i];
                j++;
            }
        }
        shopMaterials = updatedMaterials;
    }

    // Función para actualizar el nombre del material en el texto del Canvas
    void UpdateMaterialName()
    {
        if (shopMaterials.Length > 0)
        {
            string materialName = shopMaterials[currentMaterialIndex].name;
            materialNameText.text = materialName;
        }
        else
        {
            buyButton.gameObject.SetActive(false);
            leftArrowButton.gameObject.SetActive(false);
            rightArrowButton.gameObject.SetActive(false);
            materialNameText.text = "No hay más materiales";
        }
    }
}
 
