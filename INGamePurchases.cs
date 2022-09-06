using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class INGamePurchases : MonoBehaviour
{
    public enum purchase
    {
        money,
        power
    }


    public GameHandler gh;
    public int price;
    public int amountToGive;
    public purchase p = purchase.money;
   
    public void Buy()
    {
        if (gh.gems >= price)
        {
            gh.gems -= price;
            if (p == purchase.money)
                gh.GetMoney(amountToGive);// gh.money += amountToGive;
            else if (p == purchase.power)
                gh.GetPower(amountToGive);// gh.power += amountToGive;
            gh.SetUI();
        }
    }
}
