using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public int did;
    public int amountToDo;
    public int rewardAmount;
    public int typeOfReward;
    public missionToDo whatToDo;

    public bool claimed=false;
}

[System.Serializable]
public enum missionToDo
{
    earnCoins,
    pickUpBoosts,
    explodeVehicles,
    runMeters,
    clickDailySpins,
    CollectBoosts,
    LevelUp,
    BuyNewSkin,
    CollectCoinsWithMagnet,
    DoubleClickPowerUp
}

[System.Serializable]
public class MissionData
{
    public int amount;
    public bool claimed = false;
}

public class MissionHandler : MonoBehaviour
{

    public List<Sprite> typeOfRewards;
    public List<Quest> missions;

    public GameObject missionPrefab;
    public GameObject content;

    public List<Mission> listOfMissions;
    List<MissionData> missionsData = new List<MissionData>();

    public float timeToNextSave = 10f;
    float _timer;

    public string path;

    public int notification=0;
    public GameObject notificationObject;
    public TextMeshProUGUI notificationText;


    private void Awake()
    {
        _timer = timeToNextSave;

        path = Application.persistentDataPath + "/quests.json";

        Load();

        Create();

        SetNotifications();

    }

    // Start is called before the first frame update
    void Start()
    {
        Save();

    }

    public void ResetMissions()
    {
        for (int i = 0; i < missions.Count; i++)
        {
            listOfMissions[i].amount = 0;
            listOfMissions[i].claimed = false;
            missions[i].did = listOfMissions[i].amount;
            missions[i].claimed = listOfMissions[i].claimed;
            missionsData[i].amount = listOfMissions[i].amount;
            missionsData[i].claimed = listOfMissions[i].claimed;
        }

        Create();

        notification = 0;
        SetNotifications();

    }

    public void Save()
    {

        if (missionsData.Count > 0)
        {
            for (int i = 0; i < missions.Count; i++)
            {
                missionsData[i].amount = listOfMissions[i].amount;
                missionsData[i].claimed = listOfMissions[i].claimed;
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);

            bf.Serialize(file, missionsData);
            file.Close();


            Debug.Log("saved quests");
        }
        else
        {
            for (int i = 0; i < missions.Count; i++)
            {
                MissionData md = new MissionData();
                md.amount = 0;
                md.claimed = false;

                missionsData.Add(md);
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);

            bf.Serialize(file, missionsData);
            file.Close();


            Debug.Log("saved quests");
        }


    }

    public void Load()
    {
        if (File.Exists(path))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            missionsData = (List<MissionData>)bf.Deserialize(file);

            file.Close();


            for (int i = 0; i < missionsData.Count; i++)
            {
                missions[i].did = missionsData[i].amount;
                missions[i].claimed = missionsData[i].claimed;

            }
        }
    }

    public void Create()
    {
        foreach(Mission m in listOfMissions)
        {
            Destroy(m.gameObject);
        }
        listOfMissions.Clear();

        for (int i = 0; i < missions.Count; i++)
        {
            GameObject mission = Instantiate(missionPrefab, content.transform);
            mission.GetComponent<Mission>().mh = this.gameObject.GetComponent<MissionHandler>();
            mission.GetComponent<Mission>().SetUI(i);
            listOfMissions.Add(mission.GetComponent<Mission>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = timeToNextSave;
            Save();
            SetNotifications();

        }
    }


    public void SetNotifications()
    {
        if (notification > 0)
        {
            notificationObject.SetActive(true);
            notificationText.text = notification.ToString();
        }
        else
        {
            notificationObject.SetActive(false);
        }
    }


    public void In()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("MissionsIn");
        AudioManager.instance.Play("Click");
    }

    public void Out()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("MissionsOut");
    }
}
