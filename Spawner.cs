using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Player player;
    public Vector3 v;
    public bool spawning = true;
    public GameObject[] toSpawn;
    public float nextSpawn = 2f;
    float spawnTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + v.x,
            player.transform.position.y + v.y,
            player.transform.position.z + v.z
            );


        if (spawning) {
            if (spawnTime < 0f)
            {
                Spawn();
                spawnTime = nextSpawn;
            }
            else
                spawnTime -= Time.deltaTime;
        }
    }

    //public Vector3 v;

    public void Spawn()
    {
        GameObject obj = Instantiate(toSpawn[Random.Range(0,toSpawn.Length)], transform.position, Quaternion.identity);
        //obj.GetComponent<Rigidbody>().AddExplosionForce(50f, transform.position,20f);

        Vector3 v = new Vector3(Random.Range(-2f, 2f), Random.Range(1f, 3f), Random.Range(-8f, -14f));

        obj.GetComponent<Rigidbody>().AddForce(v, ForceMode.Impulse);
        obj.GetComponent<Rigidbody>().AddTorque(transform.up * 15f * Random.Range(-5f,5f));
        obj.GetComponent<Rigidbody>().AddTorque(transform.right * 15f * Random.Range(-5f, 5f));
    }


}
