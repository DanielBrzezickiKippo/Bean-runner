using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCreator : MonoBehaviour
{
    public GameObject gamePrefab;
    public GameObject[] powerUps;

    public float lastSpawned =0;

    public bool spawn = true;

    public float timeToNextSpawn = 0.2f;
    float _spawnTimer=0;

    float _timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        lastSpawned = transform.position.z;
        //_spawnTimer = timeToNextSpawn;
        _timer = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn)
        {
            if (_spawnTimer > 0)
            {
                _spawnTimer -= Time.deltaTime * (1 / Time.timeScale);
            }
            else
            {
                Spawn();
                _spawnTimer = timeToNextSpawn;
            }
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime * (1 / Time.timeScale);
        }
        else
        {
            if (spawn)
                spawn = false;
            else
                spawn = true;
            _timer = Random.Range(5f,10f);
        }


    }

    int lastDir = 0;

    public void Spawn()
    {
        int dir=0;
        if (lastDir==0)
            dir = Random.Range(-1, 2);
        else if(lastDir==1)
            dir = Random.Range(0, 2);
        else if (lastDir == 1)
            dir = Random.Range(-1, 1);

        lastDir = dir;
        

        if(Random.Range(0,100)<2f)
            Instantiate(powerUps[Random.Range(0,powerUps.Length)], new Vector3(2f * dir, transform.position.y, transform.position.z), Quaternion.Euler(-30f, 0f, 0f));
        else
            Instantiate(gamePrefab, new Vector3(2f * dir, transform.position.y, transform.position.z), Quaternion.Euler(-30f,0f,0f));
        spawn = true;
    }
}
