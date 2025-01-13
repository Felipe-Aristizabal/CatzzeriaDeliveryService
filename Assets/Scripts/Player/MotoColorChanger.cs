using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotoColorChanger : MonoBehaviour
{
    public Material[] materials; // Lista de materiales disponibles

    [Header("Canvas")]
    [SerializeField] private GameObject[] objectsToChange; // Lista de objetos a los que se les cambiará el material
    [SerializeField] private GameObject colorSelectionCanvas; // El Canvas para seleccionar el color
    [SerializeField] private GameObject shopCanvas; // Canvas de la tienda
    [SerializeField] private GameObject hatInventoryCanvas; // Canvas del inventario de sombreros
    [SerializeField] private GameObject hatStoreCanvas; // Canvas de la tienda de sombreros

    [Header("Canvas")]
    [SerializeField] private Button openShopButton; // Botón para abrir la tienda desde el inventario
    [SerializeField] private Button openHatButton; 
    [SerializeField] private Text colorNameText; // Texto que mostrará el nombre del color
    [SerializeField] private Button leftArrowButton; // Botón para moverse a la izquierda
    [SerializeField] private Button rightArrowButton; // Botón para moverse a la derecha
    [SerializeField] private Button selectButton; // Botón para seleccionar el color

    private int currentMaterialIndex = 0; // Índice del material actual

    void Start()
    {
        // Inicialmente el Canvas está desactivado
        colorSelectionCanvas.gameObject.SetActive(false);

        // Asignar las funciones a los botones
        leftArrowButton.onClick.AddListener(SelectPreviousColor);
        rightArrowButton.onClick.AddListener(SelectNextColor);
        selectButton.onClick.AddListener(FixColor);
        openShopButton.onClick.AddListener(OpenShop);
        openHatButton.onClick.AddListener(OpenHatInven);

    }

    void Update()
    {
        // Activar o desactivar los Canvas al presionar "P"
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                // Si el tiempo está en 1, prender el Canvas de selección de color y pausar el tiempo
                colorSelectionCanvas.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0)
            {
                // Si el tiempo está en 0 y algún Canvas está activo, apagar todos los Canvas y reanudar el tiempo
                if (colorSelectionCanvas.activeSelf || shopCanvas.activeSelf || hatInventoryCanvas.activeSelf || hatStoreCanvas.activeSelf)
                {
                    colorSelectionCanvas.SetActive(false);
                    shopCanvas.SetActive(false);
                    hatInventoryCanvas.SetActive(false);
                    hatStoreCanvas.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }
    }

    // Función para mostrar/ocultar el Canvas de selección de color
    void ToggleColorSelectionCanvas()
    {
        bool isActive = colorSelectionCanvas.gameObject.activeSelf;
        colorSelectionCanvas.gameObject.SetActive(!isActive);

        // Actualizar el nombre del color si el canvas está activo
        if (!isActive)
        {
            UpdateColorName();
        }
    }
    void OpenShop()
    {
        colorSelectionCanvas.gameObject.SetActive(false); 
        shopCanvas.gameObject.SetActive(true); 
    }

    void OpenHatInven()
    {
        colorSelectionCanvas.gameObject.SetActive(false); 
        hatInventoryCanvas.gameObject.SetActive(true); 
    }

    // Función para seleccionar el color anterior en la lista
    void SelectPreviousColor()
    {
        currentMaterialIndex--;
        if (currentMaterialIndex < 0)
        {
            currentMaterialIndex = materials.Length - 1;
        }
        UpdateColorName();
        ApplyCurrentColor();
    }

    // Función para seleccionar el siguiente color en la lista
    void SelectNextColor()
    {
        currentMaterialIndex++;
        if (currentMaterialIndex >= materials.Length)
        {
            currentMaterialIndex = 0;
        }
        UpdateColorName();
        ApplyCurrentColor();
    }

    // Función para fijar el color seleccionado y desactivar el Canvas
    void FixColor()
    {
        ApplyCurrentColor();
        colorSelectionCanvas.gameObject.SetActive(false); // Desactivar el Canvas
        Time.timeScale = 1;
    }

    // Función para aplicar el color actual a los objetos seleccionados
    void ApplyCurrentColor()
    {
        Material selectedMaterial = materials[currentMaterialIndex];
        foreach (GameObject obj in objectsToChange)
        {
            if (obj.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.material = selectedMaterial;
            }
        }
    }

    // Función para actualizar el nombre del color en el texto del Canvas
    void UpdateColorName()
    {
        string materialName = materials[currentMaterialIndex].name;
        colorNameText.text = materialName; // Cambiar el texto del color seleccionado
    }
}
