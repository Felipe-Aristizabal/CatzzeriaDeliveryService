using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatIventory : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject hatInventoryCanvas; // Canvas del inventario de sombreros
    [SerializeField] private GameObject colorSelectionCanvas;
    [SerializeField] private GameObject hatStoreCanvas;

    [Header("UI Elements")]
    [SerializeField] private Button openShopHatButton; 
    [SerializeField] private Button openColorInvenButton;
    [SerializeField] private Button leftArrowButton; // Botón para moverse a la izquierda
    [SerializeField] private Button rightArrowButton; // Botón para moverse a la derecha
    [SerializeField] private Button selectButton; // Botón para seleccionar un sombrero
    [SerializeField] private Text hatNameText; // Texto para mostrar el nombre del sombrero

    [Header("Inventory Data")]
    [SerializeField] private List<GameObject> hats; // Lista de sombreros en el inventario

    private int currentHatIndex = 0; // Índice del sombrero actual
    [SerializeField] private SwitchHat switchHat;

    void Start()
    {
        // Inicialmente el Canvas está desactivado
        hatInventoryCanvas.SetActive(false);

        // Asignar funciones a los botones
        leftArrowButton.onClick.AddListener(SelectPreviousHat);
        rightArrowButton.onClick.AddListener(SelectNextHat);
        selectButton.onClick.AddListener(FixHat);
        openShopHatButton.onClick.AddListener(OpenHatShop);
        openColorInvenButton.onClick.AddListener(OpenColorInven);


        // Actualizar el nombre del sombrero actual al iniciar
        UpdateHatName();
    }
    void OpenHatShop()
    {
        hatInventoryCanvas.gameObject.SetActive(false); 
        hatStoreCanvas.gameObject.SetActive(true); 
    }

    void OpenColorInven()
    {
        hatInventoryCanvas.gameObject.SetActive(false); 
        colorSelectionCanvas.gameObject.SetActive(true); 
    }

    public void AddHatToInventory(GameObject hat)
    {
        hats.Add(hat);
        UpdateHatName();
    }

    // Función para seleccionar el sombrero anterior
    void SelectPreviousHat()
    {
        currentHatIndex--;
        if (currentHatIndex < 0)
        {
            currentHatIndex = hats.Count - 1; // Volver al final si se pasa del principio
        }
        UpdateHatName();
    }

    // Función para seleccionar el siguiente sombrero
    void SelectNextHat()
    {
        currentHatIndex++;
        if (currentHatIndex >= hats.Count)
        {
            currentHatIndex = 0; // Volver al principio si se pasa del final
        }
        UpdateHatName();
    }

    // Función para seleccionar el sombrero (vacía por ahora)
    void SelectHat()
    {
        Debug.Log($"Selected hat: {hats[currentHatIndex].name}");
        switch (hats[currentHatIndex].name)
        {
            case "Elegant":
            switchHat.currentHat = SwitchHat.CurrentHat.elegant;
            break;
            case "Picnic":
            switchHat.currentHat = SwitchHat.CurrentHat.picnic;
            break;
            case "Chinese":
            switchHat.currentHat = SwitchHat.CurrentHat.chinese;
            break;
            case "Mexican":
            switchHat.currentHat = SwitchHat.CurrentHat.mexican;
            break;
            case "Pirate":
            switchHat.currentHat = SwitchHat.CurrentHat.pirate;
            break;
            default:

                switchHat.currentHat = SwitchHat.CurrentHat.none;
                break;
        }
    }

    void FixHat()
    {
        SelectHat();
        hatInventoryCanvas.gameObject.SetActive(false); // Desactivar el Canvas
        Time.timeScale = 1;
    }

    // Función para actualizar el texto con el nombre del sombrero actual
    void UpdateHatName()
    {
        if (hats.Count > 0)
        {
            hatNameText.text = hats[currentHatIndex].name; // Mostrar el nombre del sombrero actual
        }
        else
        {
            hatNameText.text = "No hats available"; // Texto por defecto si no hay sombreros
        }
    }
}
