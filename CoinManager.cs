using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public RoadCreator rc;
    public GameObject player;



    AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        rc = GameObject.FindGameObjectWithTag("RoadCreator").GetComponent<RoadCreator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    public void Check()
    {
        if (player.GetComponent<GameHandler>().magnetBoostTimer > 0)
        {
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position)<5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, player.GetComponent<Player>().speed*1.5f * Time.deltaTime);
            }
        }

        if (player.transform.position.z - 2*rc.tileV.z > transform.position.z)
        {
            Destroy(this.gameObject);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameHandler gh = player.GetComponent<GameHandler>();

            am.Play("Coin");

            if (gh.magnetBoostTimer > 0)
            {
                foreach (Mission mission in gh.missions.listOfMissions)
                {
                    if (mission.missionToDo == missionToDo.CollectCoinsWithMagnet)
                    {
                        mission.amount++;
                        mission.UpdateStatus();
                    }
                }
            }

            if (gh.doubleCoins == false)
            {
                gh.money++;
                foreach(Mission mission in gh.missions.listOfMissions)
                {
                    if (mission.missionToDo == missionToDo.earnCoins)
                    {
                        mission.amount++;
                        mission.UpdateStatus();
                    }
                }
            }
            else
            {
                gh.money+=2;
                foreach (Mission mission in gh.missions.listOfMissions)
                {
                    if (mission.missionToDo == missionToDo.earnCoins)
                    {
                        mission.amount+=2;
                        mission.UpdateStatus();
                    }
                }
            }
            gh.SetUI();
        }
    }
}
