using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    private int countPizza = 0;

    [SerializeField] private GameObject endFeedback;
    [SerializeField] private Animator deliveryFeedback;

    private void FixedUpdate() 
    {
        if (countPizza >= 3)
        {
            endFeedback.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("CustomerBuilding"))
        {
            countPizza++;
            deliveryFeedback.SetTrigger("Delivered");
        }
    }
}
