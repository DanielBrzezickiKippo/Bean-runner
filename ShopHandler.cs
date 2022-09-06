using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    GameHandler gh;

    public Button[] powerButtons;
    public Button[] coinsButtons;


    // Start is called before the first frame update
    void Start()
    {
        gh = GameObject.FindGameObjectWithTag("Player").GetComponent<GameHandler>();
        SetButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetButtons()
    {
        powerButtons[0].onClick.RemoveAllListeners();
        powerButtons[0].onClick.AddListener(() => { BuyPower(12f, 100f); });
        powerButtons[1].onClick.RemoveAllListeners();
        powerButtons[1].onClick.AddListener(() => { BuyPower(60f, 1000f); });


        coinsButtons[0].onClick.RemoveAllListeners();
        coinsButtons[0].onClick.AddListener(() => { BuyCoins(12f, 1500f); });

        coinsButtons[1].onClick.RemoveAllListeners();
        coinsButtons[1].onClick.AddListener(() => { BuyCoins(48f, 4000f); });

        coinsButtons[2].onClick.RemoveAllListeners();
        coinsButtons[2].onClick.AddListener(() => { BuyCoins(120f, 12000f); });

        coinsButtons[3].onClick.RemoveAllListeners();
        coinsButtons[3].onClick.AddListener(() => { BuyCoins(240f, 25000f); });

        coinsButtons[4].onClick.RemoveAllListeners();
        coinsButtons[4].onClick.AddListener(() => { BuyCoins(490f, 60000f); });
    }

    public void BuySpecialOffer(float amountOfGems, float amountOfCoins)
    {
        gh.gems += amountOfGems;
        gh.money += amountOfCoins;
    }

    public void BuyGems(float amountOfGems)
    {
        gh.gems += amountOfGems;
    }

    public void BuyPower(float gemCost, float earnPower)
    {
        if (gh.gems >= gemCost)
        {
            gh.gems -= gemCost;
            gh.power += earnPower;
        }
    }

    public void BuyCoins(float gemCost, float earnCoins)
    {
        if (gh.gems >= gemCost)
        {
            gh.gems -= gemCost;
            gh.money += earnCoins;
        }
    }

    public void In()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("ShopIn");
        AudioManager.instance.Play("Click");
    }

    public void Out()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("ShopOut");
    }
}
