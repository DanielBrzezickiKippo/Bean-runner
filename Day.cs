using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class Day : MonoBehaviour
{
    #region Singleton class: WorldTimeAPI

    public static Day Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


    }

    #endregion

    public struct TimeData
    {
        //public string client_ip;
        //...
        public string datetime;
        //..
    }


    const string API_URL = "http://worldtimeapi.org/api/ip";

    private DateTime _currentDateTime = DateTime.Now;

    void Start()
    {
    
        StartCoroutine(GetRealDateTimeFromAPI());

    }

    public IEnumerator GetRealDateTimeFromAPI()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
        //Debug.Log("getting real datetime...");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            //error
            //Debug.Log("Error: " + webRequest.error);

        }
        else
        {
            TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
            _currentDateTime = ParseDateTime(timeData.datetime);

            DateTime savedDate;

            if (PlayerPrefs.GetString("LAST_DATE", "0") == "0")
            {
                PlayerPrefs.SetString("LAST_DATE", timeData.datetime);
                savedDate = ParseDateTime(PlayerPrefs.GetString("LAST_DATE", timeData.datetime));
                StartCoroutine(SaveRealDateTimeFromAPI());
            }

            savedDate = ParseDateTime(PlayerPrefs.GetString("LAST_DATE", timeData.datetime));

            if ((_currentDateTime.Day != savedDate.Day && _currentDateTime.Month == savedDate.Month) ||
                (_currentDateTime.Day == savedDate.Day && _currentDateTime.Month != savedDate.Month)
                )
            {
                StartCoroutine(SaveRealDateTimeFromAPI());
            }
        }
    }

    public IEnumerator SaveRealDateTimeFromAPI()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
        Debug.Log("getting real datetime...");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            //error
            Debug.Log("Error: " + webRequest.error);

        }
        else
        {
            //success
            Debug.Log("succesfuly saved");
            TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);

            _currentDateTime = ParseDateTime(timeData.datetime);
            PlayerPrefs.SetString("LAST_DATE", timeData.datetime);

            ResetMissions();
            ResetSpins();

        }
    }


    void ResetMissions()
    {
        MissionHandler mh = GameObject.FindGameObjectWithTag("MissionHandler").GetComponent<MissionHandler>();
        mh.ResetMissions();
    }

    void ResetSpins()
    {
        WheelOfFortune wof = GameObject.FindGameObjectWithTag("Wof").GetComponent<WheelOfFortune>();
        wof.ResetSpins();
    }

    DateTime ParseDateTime(string datetime)
    {
        //match 0000-00-00
        string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;

        //match 00:00:00
        string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }

}
