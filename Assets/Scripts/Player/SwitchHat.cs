using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHat : MonoBehaviour
{

    [SerializeField] private List<GameObject> hatList;

    private enum CurrentHat
    {
        mexican,
        straw,
        elegant
    }

    [SerializeField] private CurrentHat currentHat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
