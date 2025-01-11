using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public CurrentHat currentHat; // !CALL THIS ENUM FOR SWITCH THE HAT
    [SerializeField] private List<GameObject> hatList; //list of hats (GameObjects)


    void FixedUpdate()
    {
        SwitchCurretHat();
    }

    private void SwitchCurretHat()
    {
        TurnOffHats(); // this method is called so that the hats do not overlap when they are turned on and off
        
        //depending on the enum status, one hat or another will be activated.
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

    //Turn Of all objects in the list 
    private void TurnOffHats()
    {
        foreach (var item in hatList)
        {
            item.SetActive(false);
        }
    }
}
