using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine.Networking;
using System;

public class LeaderboardController : MonoBehaviour
{

    int ID = 442;
    int MaxScores = 25;
    public GameObject[] scoresObjects;
    public GameObject youObject;
    int currentID;

    public Sprite[] stages;

    public GameHandler gh;

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.GetString("UserID", "") == "")
        {
            int random1 = UnityEngine.Random.Range(0, 10000);
            int random2 = UnityEngine.Random.Range(0, 10000);
            int random3 = UnityEngine.Random.Range(0, 10000);

            PlayerPrefs.SetString("UserID", random1.ToString() + "-" + random2.ToString() + "-"+random3.ToString());
        }

        LootLockerSDKManager.StartSession(PlayerPrefs.GetString("UserID"), (response) =>
         {
             if (response.success)
             {
                 currentID = response.player_id;
                 Debug.Log("Success");
             }
             else
             {
                 Debug.Log("Failed");
             }
         });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowScores()
    {
        AudioManager.instance.Play("Click");
        /*LootLockerSDKManager.GetPlayerInfo((onComplete) =>
        {
            onComplete
        });*/

        LootLockerSDKManager.GetMemberRank(ID.ToString(), currentID, (onComplete) => {
            youObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
            youObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "none";
            youObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = 0 + "m";
            if (onComplete.success)
            {
                if (onComplete.rank == 1)
                {
                    youObject.transform.GetChild(5).gameObject.SetActive(true);
                    youObject.transform.GetChild(5).GetComponent<Image>().sprite = stages[0];
                }
                else if (onComplete.rank == 2)
                {
                    youObject.transform.GetChild(5).gameObject.SetActive(true);
                    youObject.transform.GetChild(5).GetComponent<Image>().sprite = stages[1];
                }
                else if (onComplete.rank == 3)
                {
                    youObject.transform.GetChild(5).gameObject.SetActive(true);
                    youObject.transform.GetChild(5).GetComponent<Image>().sprite = stages[2];
                }
                else
                {
                    youObject.transform.GetChild(5).gameObject.SetActive(false);
                }

                youObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = onComplete.rank.ToString();
                if(onComplete.player!=null)
                    youObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = onComplete.player.name;
                else
                    youObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "none";
                youObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = onComplete.score.ToString() + "m";
            }


        });

        LootLockerSDKManager.GetScoreList(ID, MaxScores, (response)=>{
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = scores.Length; i < MaxScores; i++)
                {
                    if (i >= 3)
                        scoresObjects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                    scoresObjects[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "none";
                    scoresObjects[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = (0).ToString() + "m";
                }

                for (int i = 0; i < scores.Length; i++)
                {
                    if(i>=3)
                        scoresObjects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                    scoresObjects[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = scores[i].player.name;
                    scoresObjects[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = scores[i].score.ToString() + "m";

                }

                /*if (scores.Length > MaxScores)
                {
                    for(int i = scores.Length; i < MaxScores; i++)
                    {
                        if (i >= 3)
                            scoresObjects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                        scoresObjects[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "none";
                        scoresObjects[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = (0).ToString() + "m";
                    }
                }*/

                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Failed");
            }
        });
    }

    public void SetName(string n)
    {
        LootLockerSDKManager.SetPlayerName(n, (onComplete) => {
            if (onComplete.success)
            {
                PlayerPrefs.SetString("username", n);
                gh.SetUI();
            }

        });
    }


    private string getUrlStringFormatted(float finalScore)
    {
        var userName = Social.localUser.userName; // Google play user name
        var userId = Social.localUser.id; // Google play user id
        //var password = "************"; // Password to access API
        var packageName = "com.Kippo.WatchOut"; // Package of this game

        return "api.gameap.app/scores/add?" +
               $"username={userName}&" +
               $"userID={userId}&" +
               $"gameName={packageName}&" +
               $"newScore={(int)finalScore}";
    }

    IEnumerator updateScoreGaMeap(float finalScore)
    {
        var url = getUrlStringFormatted(finalScore);
        using (UnityWebRequest req = UnityWebRequest.Get(String.Format(url)))
        {
            req.SetRequestHeader("x-api-key", "uZ20q5qvrV9oosmU9FyJq93Kdv5Oygik2KYG6Uol");
            //print("SENDING REQUEST TO GAMEAP...");
            yield return req.Send();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            //print("DONE - SCORE UPDATE ON GAMEP");
            string response = Encoding.Default.GetString(result);
            //print(response);
        }
    }

    public void SubmitScore(int score)
    {
        StartCoroutine(updateScoreGaMeap(score));

        LootLockerSDKManager.SubmitScore(currentID.ToString(), score, ID, (response) =>
          {
              if (response.success)
              {
                  Debug.Log("Success");
              }
              else
              {
                  Debug.Log("Failed");
              }
          });



    }

}
