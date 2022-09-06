using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class CharacterHandler : MonoBehaviour
{
    public List<GameObject> characterList;
    public List<bool> characterBought;
    public int selected = 0;

    public GameObject player;

    public Button buyCoins;
    public Button buyGems;
    public Button select;


    public TextMeshProUGUI buyCoinsText;
    public TextMeshProUGUI buyGemsText;

    public Button back;

    // Start is called before the first frame update
    void Start()
    {
        _timer = timeToNextSave;

        path = Application.persistentDataPath + "/characters.json";

        Load();

        Save();

        selected = PlayerPrefs.GetInt("character", 0);

        changeCharacter();

        SetPrice(selected);
    }

    public float timeToNextSave = 10f;
    float _timer=10f;
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
        }
    }

    int index = 0;

    public void SetButtons(int i)
    {
        buyCoins.onClick.RemoveAllListeners();
        buyGems.onClick.RemoveAllListeners();
        select.onClick.RemoveAllListeners();


        buyCoins.onClick.AddListener(() => { Buy(i); });
        buyGems.onClick.AddListener(() => { BuyGems(i); });
        select.onClick.AddListener(() => { SelectChar(i); });
    }

    public void Prev()
    {
        index--;
        if (index < 0)
            index = characterList.Count-1;

        for (int i = 0; i < characterList.Count; i++)
        {
            if (i == index)
            {
                Destroy(player.GetComponent<GameHandler>().playerCharacter);

                GameObject character = Instantiate(characterList[index], player.transform);
                player.GetComponent<GameHandler>().playerCharacter = character;
                SetPrice(i);
                SetButtons(index);
            }
        }
    }

    public void Next()
    {
        index++;
        if (index >= characterList.Count)
            index = 0;

        for(int i = 0; i < characterList.Count; i++)
        {
            if (i == index)
            {

                Destroy(player.GetComponent<GameHandler>().playerCharacter);

                GameObject character = Instantiate(characterList[index], player.transform);
                player.GetComponent<GameHandler>().playerCharacter = character;

                SetPrice(i);
                SetButtons(index);
            }
        }
    }

    public void Buy(int i)
    {
        float coinPrice = 50f * Mathf.Pow(i, 2);

        GameHandler gh = player.GetComponent<GameHandler>();

        if (gh.money >= coinPrice)
        {
            gh.money -= coinPrice;
            gh.SetUI();
            SelectChar(i);
            CheckMission();

            AudioManager.instance.Play("Cash");
        }

    }

    public void BuyGems(int i)
    {
        float gemPrice = 4 * Mathf.Pow(i, 2);

        if (i >= 4)
        {
            gemPrice = 4 * Mathf.Pow(4, 2);
        }


        GameHandler gh = player.GetComponent<GameHandler>();

        if (gh.gems >= gemPrice)
        {
            gh.gems -= gemPrice;
            gh.SetUI();
            SelectChar(i);
            CheckMission();

            AudioManager.instance.Play("Cash");
        }

    }

    void CheckMission()
    {

        foreach (Mission mission in player.GetComponent<GameHandler>().missions.listOfMissions)
        {
            if (mission.missionToDo == missionToDo.BuyNewSkin)
            {
                mission.amount++;
                mission.UpdateStatus();
            }
        }
    }

    public void SelectChar(int i)
    {
        characterBought[i] = true;
        selected = i;
        PlayerPrefs.SetInt("character", selected);
        back.onClick.Invoke();
        //Out();
    }

    public void SetPrice(int i)
    {
        if (characterBought[i])
        {
            buyCoins.gameObject.SetActive(false);
            buyGems.gameObject.SetActive(false);
            select.gameObject.SetActive(true);
        }
        else
        {
            buyCoins.gameObject.SetActive(true);
            buyGems.gameObject.SetActive(true);
            select.gameObject.SetActive(false);
        }

        float coinPrice = 50f * Mathf.Pow(i, 2);
        buyCoinsText.text = coinPrice.ToString();

        float gemPrice = 4 * Mathf.Pow(i,2);
        if (gemPrice > 50)
        {
            gemPrice = 4 * Mathf.Pow(4, 2);
        }
        buyGemsText.text = gemPrice.ToString();


    }

    public void changeCharacter()
    {

        Destroy(player.GetComponent<GameHandler>().playerCharacter);

        GameObject character = Instantiate(characterList[selected], player.transform);
        player.GetComponent<GameHandler>().playerCharacter = character;
        index = selected;
        SetPrice(selected);
        SetButtons(selected);

    }


    string path;
    


    public void Save()
    {
        if (characterBought.Count > 0)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);

            bf.Serialize(file, characterBought);
            file.Close();


            Debug.Log("saved characters");
        }
        else
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                if(i==0)
                    characterBought.Add(true);
                else
                    characterBought.Add(false);
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);

            bf.Serialize(file, characterBought);
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
            characterBought = (List<bool>)bf.Deserialize(file);

            file.Close();
        }
    }



    public void In()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("CharacterIn");
        AudioManager.instance.Play("Click");
    }

    public void Out()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("CharacterOut");

        Destroy(player.GetComponent<GameHandler>().playerCharacter);

        GameObject character = Instantiate(characterList[selected], player.transform);
        player.GetComponent<GameHandler>().playerCharacter = character;

        SetPrice(selected);
    }

}
