using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public enum boost
    {
        slowMo,
        clearMap,
        freeSpin,
        gigaKubica,
        goldRain,
        magnet
    }

    public boost b;

    GameObject player;

    AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckSingleRunMission()
    {
        GameHandler gh = player.GetComponent<GameHandler>();
        gh.boosts++;

        foreach (Mission mission in gh.missions.listOfMissions)
        {
            if (mission.missionToDo == missionToDo.CollectBoosts)
            {
                if (gh.boosts >= mission.amountToDo)
                {
                    mission.amount = mission.amountToDo;
                    mission.UpdateStatus();
                }
            }
        }
    }

    void pick()
    {
        switch (b)
        {
            case boost.slowMo:
                //Sets time of slow motion +15secs
                player.GetComponent<GameHandler>().slowTimer += 15f;
                player.GetComponent<GameHandler>().slowTimerMax = player.GetComponent<GameHandler>().slowTimer;
                CheckSingleRunMission();
                break;
            case boost.clearMap:
                GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
                foreach(GameObject car in cars)
                {
                    //Spawn particles
                    GameObject p = Instantiate(player.GetComponent<GameHandler>().carBoomParticle, car.transform.position, Quaternion.Euler(60f, 0f, 0f));
                    foreach (Mission mission in player.GetComponent<GameHandler>().missions.listOfMissions)
                    {
                        if (mission.missionToDo == missionToDo.explodeVehicles)
                        {
                            mission.amount++;
                            mission.UpdateStatus();
                        }
                    }
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            am.PlayInObject("CarHit", p);
                            break;
                        case 1:
                            am.PlayInObject("CarHit1", p);
                            break;
                        case 2:
                            am.PlayInObject("CarHit2", p);
                            break;
                        case 3:
                            am.PlayInObject("CarHit3", p);
                            break;

                    }

                    Destroy(car);
                }
                CheckSingleRunMission();
                break;
            case boost.freeSpin:
                player.GetComponent<GameHandler>().wof.spins++;
                player.GetComponent<GameHandler>().wof.SaveSpins();
                CheckSingleRunMission();
                break;
            case boost.gigaKubica:
                player.GetComponent<GameHandler>().ciotkaBoostTimer += 15f;
                player.GetComponent<GameHandler>().ciotkaBoostTimerMax = player.GetComponent<GameHandler>().ciotkaBoostTimer;
                break;
            case boost.goldRain:
                player.GetComponent<GameHandler>().doubleCoinsTimer += 15f;
                player.GetComponent<GameHandler>().doubleCoinsTimerMax = player.GetComponent<GameHandler>().doubleCoinsTimer;
                CheckSingleRunMission();
                break;
            case boost.magnet:
                player.GetComponent<GameHandler>().magnetBoostTimer += 15f;
                player.GetComponent<GameHandler>().magnetBoostTimerMax = player.GetComponent<GameHandler>().magnetBoostTimer;
                CheckSingleRunMission();
                break;
        }

        foreach (Mission mission in player.GetComponent<GameHandler>().missions.listOfMissions)
        {
            if (mission.missionToDo == missionToDo.pickUpBoosts)
            {
                mission.amount++;
                mission.UpdateStatus();
            }
        }

        Destroy(this.gameObject);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag== "Player")
        {
            pick();
            am.Play("PowerUp");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
    }
}
