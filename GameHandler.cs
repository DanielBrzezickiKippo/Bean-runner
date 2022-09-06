using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public GameObject playerCharacter;
    public GameObject mainMenu;
    public GameObject topPanel;
    public GameObject topRightPanel;

    public Camera cam;

    public GameObject[] carHitParticles;
    public GameObject footPrintParticle;
    public GameObject carBoomParticle;

    public GameObject spawner;
    public GameObject tapToPlayButton;
    public float money = 0;
    public float gems = 0;
    public float power = 0;

    public MissionHandler missions;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI powerText;

    public TextMeshProUGUI usernameText;

    public GameObject content;
    public GameObject[] boostPrefabs;

    public PowerUpUIHandler ciotkaBoostUI;
    public PowerUpUIHandler doubleCoinsBoostUI;
    public PowerUpUIHandler slowMoBoostUI;
    public PowerUpUIHandler magnetBoostUI;

    [HideInInspector] public float slowTimer=0;
    [HideInInspector] public float doubleCoinsTimer=0;
    [HideInInspector] public bool doubleCoins = false;
    [HideInInspector] public float ciotkaBoostTimer = 0;
    [HideInInspector] public bool ciotkaBoost = false;
    public WheelOfFortune wof;

    public RevenueParticles rp;

    [HideInInspector] public float magnetBoostTimer = 0;


    [HideInInspector] public float slowTimerMax = 0;
    [HideInInspector] public float doubleCoinsTimerMax = 0;
    [HideInInspector] public float ciotkaBoostTimerMax = 0;
    [HideInInspector] public float magnetBoostTimerMax = 0;


    AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        money = PlayerPrefs.GetFloat("_money", 0f);
        gems = PlayerPrefs.GetFloat("_gems", 0f);
        power = PlayerPrefs.GetFloat("_power", 0f);

        SetUI();
    }


    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            if (Input.touchCount > 1)
            {
                Vector2 pos1 = Input.touches[0].position;
                Vector2 pos2 = Input.touches[1].position;

                    Debug.Log(Vector2.Distance(pos1, pos2));

            }
        }*/


        if (gameObject.GetComponent<Player>().enabled)
        {
            if (IsDoubleTap()|| Input.GetKeyDown(KeyCode.Space))
            {
                if (power >= 3)
                {
                    if (ciotkaBoostTimer <= 0f)
                    {
                        power -= 3;
                        SetUI();
                        ciotkaBoostTimer += 10f;

                        foreach (Mission mission in missions.listOfMissions)
                        {
                            if (mission.missionToDo == missionToDo.DoubleClickPowerUp)
                            {
                                mission.amount++;
                                mission.UpdateStatus();
                            }
                        }

                    }

                }
            }
        }


        if (slowTimer > 0)
        {
            Time.timeScale = .5f;
            slowTimer -= Time.deltaTime * (1 / Time.timeScale);

            if (slowMoBoostUI == null)
            {
                GameObject obj = Instantiate(boostPrefabs[0], content.transform);
                slowMoBoostUI = obj.GetComponent<PowerUpUIHandler>();
            }
            else
            {
                slowMoBoostUI.timer.text = Mathf.Round(slowTimer) + "s";
                slowMoBoostUI.rs.SliderValue = (slowTimer / slowTimerMax)*100f;
                slowMoBoostUI.rs.UpdateUI();
            }

        }
        else
        {
            Time.timeScale = 1f;
            if (slowMoBoostUI != null)
                Destroy(slowMoBoostUI.gameObject);
           
        }

        if (doubleCoinsTimer > 0)
        {
            doubleCoins = true;
            doubleCoinsTimer -= Time.deltaTime * (1 / Time.timeScale);


            if (doubleCoinsBoostUI == null)
            {
                GameObject obj = Instantiate(boostPrefabs[1], content.transform);
                doubleCoinsBoostUI = obj.GetComponent<PowerUpUIHandler>();
            }
            else
            {
                doubleCoinsBoostUI.timer.text = Mathf.Round(doubleCoinsTimer) + "s";

                //doubleCoinsBoostUI.rs.currentValue = (doubleCoinsTimer / doubleCoinsTimerMax);
                doubleCoinsBoostUI.rs.SliderValue = (doubleCoinsTimer / doubleCoinsTimerMax)*100f;
                doubleCoinsBoostUI.rs.UpdateUI();
            }

        }
        else
        {
            doubleCoins = false;
            if (doubleCoinsBoostUI != null)
                Destroy(doubleCoinsBoostUI.gameObject);
        }

        if (ciotkaBoostTimer > 0)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            ciotkaBoostTimer -= Time.deltaTime * (1 / Time.timeScale);
            ciotkaBoost = true;


            if (ciotkaBoostUI == null)
            {
                GameObject obj = Instantiate(boostPrefabs[2], content.transform);
                ciotkaBoostUI = obj.GetComponent<PowerUpUIHandler>();
            }
            else
            {
                ciotkaBoostUI.timer.text = Mathf.Round(ciotkaBoostTimer) + "s";
                //ciotkaBoostUI.rs.currentValue = ciotkaBoostTimer / ciotkaBoostTimerMax;
                ciotkaBoostUI.rs.SliderValue = (ciotkaBoostTimer / ciotkaBoostTimerMax)*100f;
                ciotkaBoostUI.rs.UpdateUI();
            }
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            ciotkaBoost = false;

            if(ciotkaBoostUI!=null)
                Destroy(ciotkaBoostUI.gameObject);
        }

        if (magnetBoostTimer > 0)
        {
            magnetBoostTimer -= Time.deltaTime * (1 / Time.timeScale);


            if (magnetBoostUI == null)
            {
                GameObject obj = Instantiate(boostPrefabs[3], content.transform);
                magnetBoostUI = obj.GetComponent<PowerUpUIHandler>();
            }
            else
            {
                magnetBoostUI.timer.text = Mathf.Round(magnetBoostTimer) + "s";
                magnetBoostUI.rs.SliderValue = (magnetBoostTimer / magnetBoostTimerMax)*100f;
                magnetBoostUI.rs.UpdateUI();
            }
        }
        else
        {
            if (magnetBoostUI != null)
                Destroy(magnetBoostUI.gameObject);
        }
    }

    public void Res()
    {
        slowTimer = 0;
        doubleCoinsTimer = 0;
        doubleCoins = false;
        ciotkaBoostTimer = 0;
        //spins = 0;
    }

    public void StartGame()
    {
        tapToPlayButton.SetActive(false);
        Res();
        gameObject.GetComponent<Player>().enabled = true;
        spawner.SetActive(true);

        cam.GetComponent<Animator>().enabled = false;
        In();

    }

    public void SetUI()
    {
        coinsText.text = money.ToString();
        gemsText.text = gems.ToString();
        powerText.text = power.ToString();

        usernameText.text = PlayerPrefs.GetString("username", "");

        PlayerPrefs.SetFloat("_money", money);
        PlayerPrefs.SetFloat("_gems", gems);
        PlayerPrefs.SetFloat("_power", power);
    }

    public void In()
    {
        mainMenu.GetComponent<Animator>().Play("start");
        topPanel.GetComponent<Animator>().Play("topOut");
        topRightPanel.GetComponent<Animator>().Play("topRightOut");
    }

    public void Out()
    {
        mainMenu.GetComponent<Animator>().Play("play");
        topPanel.GetComponent<Animator>().Play("topIn");
    }


    public void IncreaseDaily()
    {
        foreach (Mission mission in missions.listOfMissions)
        {
            if (mission.missionToDo == missionToDo.clickDailySpins)
            {

                mission.amount++;
                if (mission.amount >= 3)
                    mission.ShortCheckStatus();
                else
                    mission.UpdateStatus();
            }
        }
    }
    /*Vector2 lastClick = Vector2.zero;
    public bool IsNear()
    {
        float d = Vector2.Distance(lastClick, Input.mousePosition);
        Debug.Log("distance: " + d);
        if (d < 100f)
        {
            lastClick = Input.mousePosition;
            return true;
        }
        lastClick = Input.mousePosition;

        return false;
    }*/

    Vector2 lastClick = Vector2.zero;
    public bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 0.9f;
        float VariancePosition = 1f;

        float d = Vector2.Distance(lastClick, Input.mousePosition);

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;


            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && d<150f)
            {
                lastClick = Input.mousePosition;
                result = true;
            }
        }
        lastClick = Input.mousePosition;
        return result;
    }


    public void GetGems(int amount)
    {
        am.Play("Gems",true,2f);
        rp.SpawnGems();
        gems += amount;
        SetUI();

    }

    public void GetMoney(int amount)
    {
        am.Play("Money",true,2f);
        rp.Spawn();
        money += amount;
        SetUI();

    }

    public void GetPower(int amount)
    {
        am.Play("Powers",true,2f);
        rp.SpawnPower();
        power += amount;
        SetUI();
    }

    [HideInInspector] public int boosts = 0;

    /*public void CheckUI(int numOfBoost, Boost.boost checking)
    {
        PowerUpUIHandler[] boosts = GameObject.FindObjectsOfType<PowerUpUIHandler>();

        if (boosts.Length > 0)
        {
            foreach (PowerUpUIHandler boost in boosts) {
                if(boost.b == checking)
                {
                    boost.timer = 
                    
                }
            }
        }
        else
        {
            Instantiate(boostPrefabs[numOfBoost], content.transform);
        }

    }*/


    


    /*void LateUpdate()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            tapCount += 1;
            StartCoroutine(Countdown());
        }

        if (tapCount == 2)
        {
            tapCount = 0;
            StopCoroutine(Countdown());

            ciotkaBoostTimer += 15f;
            //// DO STUFF!
        }

    }
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(0.3f);
        tapCount = 0;
    }*/
}
