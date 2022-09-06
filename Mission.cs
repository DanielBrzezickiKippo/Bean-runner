using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mission : MonoBehaviour
{


    public MissionHandler mh;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI sliderText;
    public Slider slider;
    public Button claim;

    public GameObject notification;

    public int amount;
    public int amountToDo;
    [HideInInspector] public int reward;


    public bool claimed = false;
    public Image rewardImage;
    public TextMeshProUGUI rewardAmountText;

    public Sprite[] UI;

    public Image[] UI_Images;

    public missionToDo missionToDo;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        mh = GameObject.FindGameObjectWithTag("MissionHandler").GetComponent<MissionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetUI(int i)
    {

        index = i;

        claimed = mh.missions[i].claimed;

        missionToDo = mh.missions[i].whatToDo;
        titleText.text = mh.missions[i].title;
        descriptionText.text = mh.missions[i].description;

        amount = mh.missions[i].did;
        amountToDo = mh.missions[i].amountToDo;
        reward = mh.missions[i].rewardAmount;
        if (amount <= mh.missions[i].amountToDo)
            sliderText.text = amount + "/" + mh.missions[i].amountToDo;
        else
            sliderText.text = mh.missions[i].amountToDo + "/" + mh.missions[i].amountToDo;
        slider.maxValue = mh.missions[i].amountToDo;
        slider.value = mh.missions[i].did;

        rewardAmountText.text = mh.missions[i].rewardAmount.ToString();
        rewardImage.sprite = mh.typeOfRewards[mh.missions[i].typeOfReward];

        if (mh.missions[i].did >= mh.missions[i].amountToDo)
        {
            if (!claimed)
            {
                claim.gameObject.SetActive(true);
                mh.notification++;
                notification.SetActive(true);
            }
            else
            {
                claim.gameObject.SetActive(false);
                sliderText.text = "Claimed";
                notification.SetActive(false);
            }
            UI_Images[0].sprite = UI[0];
            UI_Images[1].sprite = UI[1];
        }
        else
        {
            claim.gameObject.SetActive(false);
            notification.SetActive(false);
            UI_Images[0].sprite = UI[2];
            UI_Images[1].sprite = UI[3];
        }
    }

    public void ShortCheckStatus()
    {
        if (amount >= mh.missions[index].amountToDo)
        {
            if (!claimed)
            {
                claim.gameObject.SetActive(true);
                //mh.notification++;
                //notification.SetActive(true);
            }
            UI_Images[0].sprite = UI[0];
            UI_Images[1].sprite = UI[1];
        }
    }

    public void UpdateStatus()
    {
        if(amount<= mh.missions[index].amountToDo)
            sliderText.text = amount + "/" + mh.missions[index].amountToDo;
        else
            sliderText.text = mh.missions[index].amountToDo + "/" + mh.missions[index].amountToDo;
        slider.maxValue = amountToDo;
        slider.value = amount;

        if (amount >= mh.missions[index].amountToDo)
        {
            if (!claimed)
            {
                claim.gameObject.SetActive(true);
                mh.notification++;
                notification.SetActive(true);
            }
            else
            {
                claim.gameObject.SetActive(false);
                sliderText.text = "Claimed";
                notification.SetActive(false);
            }
            UI_Images[0].sprite = UI[0];
            UI_Images[1].sprite = UI[1];
        }
        else
        {
            claim.gameObject.SetActive(false);
            notification.SetActive(false);
            UI_Images[0].sprite = UI[2];
            UI_Images[1].sprite = UI[3];
        }
    }

    public void Claim()
    {
        GameHandler gh = GameObject.FindGameObjectWithTag("Player").GetComponent<GameHandler>();
        switch (mh.missions[index].typeOfReward)
        {
            case 0:
                gh.GetMoney(reward);

                break;
            case 1:
                gh.GetGems(reward);
                break;
            case 2:
                gh.GetPower(reward);
                break;
        }
        claim.gameObject.SetActive(false);
        claimed = true;
        sliderText.text = "Claimed";

        notification.SetActive(false);
        if (mh.notification > 0)
            mh.notification--;

        gh.SetUI();

        mh.SetNotifications();
    }

}
