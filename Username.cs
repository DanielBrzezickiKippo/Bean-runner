using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Username : MonoBehaviour
{
    string username;

    public GameObject usernamePopUp;
    public TMP_InputField input;
    public Button confirm;

    public Sprite[] confirmButtons;

    public LeaderboardController lc;


    private void Awake()
    {
        if (PlayerPrefs.GetString("username", "") == "")
        {
            usernamePopUp.SetActive(true);

        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (input.text.Length > 2)
        {
            if (!confirm.interactable)
            {
                confirm.interactable = true;
                confirm.GetComponent<Image>().sprite = confirmButtons[0];
            }
        }
        else
        {
            if (confirm.interactable)
            {
                confirm.interactable = false;
                confirm.GetComponent<Image>().sprite = confirmButtons[1];
            }
        }

    }

    public void Confirm()
    {
        if (input.text.Length > 2)
        {
            lc.SetName(input.text);
            usernamePopUp.SetActive(false);
        }
    }

}
