using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FailedHandler : MonoBehaviour
{
    public Button gemContinue;
    public Button adContinue;

    public SceneLoader sl;

    public TextMeshProUGUI meterText;
    public TextMeshProUGUI secondText;
    public Slider timerSlider;

    float _timer;
    float timeToSkip = 5.5f;

    bool once = true;
    bool continued = false;

    public Player player;
    public LeaderboardController lc;


    public float score;
    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetFloat("score", 0);

        Debug.Log("score!!!!!!!!!!!!!!!!!!  "+score);

        //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        {
            if (t)
            {
                timerSlider.value = _timer / timeToSkip * 100f;
                secondText.text = Mathf.Round(_timer) + "s";
                _timer -= Time.deltaTime * (1 / Time.timeScale);
            }
        }
        else
        {
            if (!once && !player.isAlive)
            {
                Fail();
                once = true;
            }
        }
    }


    public void SetUI(float meters)
    {
        if (player.continued)
        {
            adContinue.interactable = false;
            gemContinue.interactable = false;
        }

        once = false;
        In();
        meterText.text = meters.ToString();
        SetTimer();
    }

    public void SetTimer()
    {
        _timer = timeToSkip;

    }

    public void Fail()
    {
        player.GetComponent<LevelHandler>().SaveProgress();

        
        if (PlayerPrefs.GetFloat("score") < score) {
            PlayerPrefs.SetFloat("score", score);
            lc.SubmitScore((int)score);

            Debug.Log("New best record!");
        }


        Out();
        sl.ReloadLevel();
    }

    bool t = false;
    
    public void In()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("FailedIn");
        t = true;
    }

    public void Out()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("FailedOut");
        t = false;
    }

    public void restart()
    {
        AudioManager.instance.Play("Soundtrack");

        once = false;
        timeToSkip = 2f;
        //Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.enabled = true;
        player.continueRun();


        

    }

    public void payGems()
    {
        if (player.GetComponent<GameHandler>().gems >= 3)
        {
            player.GetComponent<GameHandler>().gems -= 3;

            restart();

        }



    }


}
