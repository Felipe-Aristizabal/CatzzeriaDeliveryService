using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHat : MonoBehaviour
{
    public enum CurrentHat
    {
        elegant,
        picnic,
        chinese,
        mexican,
        pirate,
        none
    }

    public CurrentHat currentHat;
    [SerializeField] private List<GameObject> hatList;


    void FixedUpdate()
    {
        SwitchCurretHat();
    }

    private void SwitchCurretHat()
    {
        TurnOffHats();  
        switch (currentHat)
        {
            case CurrentHat.elegant:
                hatList[0].SetActive(true);
                break;

            case CurrentHat.picnic:
                hatList[1].SetActive(true);
                break;

            case CurrentHat.chinese:
                hatList[2].SetActive(true);
                break;

            case CurrentHat.mexican:
                hatList[3].SetActive(true);
                break;

            case CurrentHat.pirate:
                hatList[4].SetActive(true);
                break;
            
            case CurrentHat.none:
                TurnOffHats(); 
                break;

            default:
                TurnOffHats();  
                break;
        }
    }

    private void TurnOffHats()
    {
        foreach (var item in hatList)
        {
            item.SetActive(false);
        }
    }
}
