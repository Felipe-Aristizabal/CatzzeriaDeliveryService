using UnityEngine;
using UnityEngine.UI;

public class ButtonMover : MonoBehaviour
{
    public float moveDistance = 50f; // Distancia a mover el bot�n
    private Vector3 originalPosition;
    private bool isMovedDown = false;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Obtiene el RectTransform del mismo bot�n
        originalPosition = rectTransform.anchoredPosition; // Guarda la posici�n original

        // A�ade el listener al bot�n
        GetComponent<Button>().onClick.AddListener(ToggleButtonPosition);
    }

    void ToggleButtonPosition()
    {
        if (isMovedDown)
        {
            // Regresar a la posici�n original
            rectTransform.anchoredPosition = originalPosition;
        }
        else
        {
            // Mover hacia abajo
            rectTransform.anchoredPosition = originalPosition - new Vector3(0, moveDistance, 0);
        }

        // Cambia el estado
        isMovedDown = !isMovedDown;
    }
}
