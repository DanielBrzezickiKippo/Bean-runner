using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelOfFortune : MonoBehaviour
{
    public Sprite[] rewardsSprites;
    public Color[] colors;

    public float reducer;
    public float multiplier = 0;
    bool round1 = true;
    public bool isStoped = false;
    public Needle needle;

    public int spins = 0;
    public int freeSpinsMax=3;
    public int freeSpins = 0;
    public TextMeshProUGUI spinText;

    public TextMeshProUGUI spinAdText;

    public Button watchAd;


    public TextMeshProUGUI spinsRouletteText;
    public GameObject spinsAlarm;

    //GameManager gm;
    //AdMobVideo ads;
    AudioManager am;

    void Start()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        freeSpins = PlayerPrefs.GetInt("FreeSpin",0);

        spins = PlayerPrefs.GetInt("Spins", 2);
        //ads = GameObject.FindGameObjectWithTag("Ads").GetComponent<AdMobVideo>();
        //gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        reducer = Random.Range(0.01f, 0.5f);

        SetUI();
    }

    // Update is called once per frameQ
    void FixedUpdate()
    {



        if (multiplier > 0)
        {
            transform.Rotate(Vector3.forward, 1 * multiplier);
        }
        else
        {
            isStoped = true;
        }

        if (multiplier < 20 && !round1)
        {
            multiplier += 0.1f;
        }
        else
        {
            round1 = true;
        }

        if (round1 && multiplier > 0)
        {
            multiplier -= reducer;
        }

        if (spins > 0)
        {
            spinsRouletteText.gameObject.SetActive(true);
            spinsRouletteText.text = spins.ToString();
            spinsAlarm.SetActive(true);
        }
        else
        {
            spinsRouletteText.gameObject.SetActive(false);
            spinsAlarm.SetActive(false);
        }
    }



    public bool canSpin = true;

    public void SaveSpins()
    {
        PlayerPrefs.SetInt("Spins", spins);
    }

    public void Spin()
    {
        if (canSpin && spins>0)
        {
            am.Play("WheelOfFortune");
            spins--;

            PlayerPrefs.SetInt("Spins", spins);

            SetUI();

            canSpin = false;
            multiplier = 1;
            reducer = Random.Range(0.1f, 1f);
            round1 = false;
            isStoped = false;

            needle.once = false;
        }
    }

    public void SetUI()
    {
        spinText.text = "SPIN!\n" + spins;
        spinAdText.text = freeSpins+"/" + freeSpinsMax;
    }


    public void WatchAdAndSpin()
    {
        if (freeSpins < freeSpinsMax)
        {

            freeSpins++;
            spins++;
            SetUI();

            PlayerPrefs.SetInt("FreeSpin", freeSpins);

            StartCoroutine(delayed());

            if (freeSpins == freeSpinsMax)
            {
                watchAd.interactable = false;
            }
        }

        /*if (freeSpins >= freeSpinsMax)
        {
            watchAd.interactable = false;
        }*/

        //ads.UserChoseToWatchAd(0);
    }

    IEnumerator delayed()
    {
        yield return new WaitForSeconds(0.15f);
        GameObject player = GameObject.Find("/Player");
        player.GetComponent<GameHandler>().IncreaseDaily();
    }

    
    public void ResetSpins()
    {
        spins++;
        PlayerPrefs.SetInt("FreeSpin", 0);
        freeSpins = PlayerPrefs.GetInt("FreeSpin", 0);
    }

}
