using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject hatInventoryCanvas; 
    [SerializeField] private GameObject hatStoreCanvas; 

    [Header("UI Elements")]
    [SerializeField] private Button openStoreButton; 
    [SerializeField] private Button openInventoryButton; 
    [SerializeField] private Button leftArrowButton; 
    [SerializeField] private Button rightArrowButton; 
    [SerializeField] private Button buyButton; 
    [SerializeField] private Text itemNameText; 
    [SerializeField] private Text dataText; 

    [Header("Lists")]
    [SerializeField] private List<GameObject> inventoryHats; 
    [SerializeField] private List<GameObject> storeHats; 

    [Header("Game Management")]
    [SerializeField] private GameManager gameManager; 
    [SerializeField] private UIController uIController; 

    private int currentItemIndex = 0; 

    void Start()
    {
        hatInventoryCanvas.SetActive(false);
        hatStoreCanvas.SetActive(false);

        // buttons actions
        openStoreButton.onClick.AddListener(OpenStore);
        openInventoryButton.onClick.AddListener(OpenInventory);
        leftArrowButton.onClick.AddListener(SelectPreviousItem);
        rightArrowButton.onClick.AddListener(SelectNextItem);
        buyButton.onClick.AddListener(BuyHat);

        // Teeeeext
        dataText.text = "";
        UpdateItemName();
    }

    void Update()
    {
        // Open store
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (hatStoreCanvas.activeSelf) return; 
            ToggleCanvas(hatInventoryCanvas);
        }

        // Close store
        if (Input.GetKeyDown(KeyCode.X) && hatStoreCanvas.activeSelf)
        {
            hatStoreCanvas.SetActive(false);
            Time.timeScale = 1; 
        }
    }

    
    private void ToggleCanvas(GameObject canvas)
    {
        bool isActive = canvas.activeSelf;
        canvas.SetActive(!isActive);

        // Pausar o reanudar el tiempo
        Time.timeScale = isActive ? 1 : 0;
    }

    private void OpenStore()
    {
        hatInventoryCanvas.SetActive(false);
        hatStoreCanvas.SetActive(true);
    }

    private void OpenInventory()
    {
        hatStoreCanvas.SetActive(false);
        hatInventoryCanvas.SetActive(true);
    }

    private void SelectPreviousItem()
    {
        currentItemIndex--;
        if (currentItemIndex < 0)
        {
            currentItemIndex = GetCurrentList().Count - 1;
        }
        UpdateItemName();
    }

    private void SelectNextItem()
    {
        currentItemIndex++;
        if (currentItemIndex >= GetCurrentList().Count)
        {
            currentItemIndex = 0;
        }
        UpdateItemName();
    }

    // Buy Hats
    private void BuyHat()
    {
        dataText.text = ""; 

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

        gameManager.SubtractCoins(hatCost);

        GameObject selectedHat = storeHats[currentItemIndex];
        inventoryHats.Add(selectedHat);
        storeHats.RemoveAt(currentItemIndex);

        currentItemIndex = Mathf.Clamp(currentItemIndex, 0, storeHats.Count - 1);
        UpdateItemName();
    }

    private List<GameObject> GetCurrentList()
    {
        return hatStoreCanvas.activeSelf ? storeHats : inventoryHats;
    }

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

