using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public RoadCreator rc;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
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
        if (player.transform.position.z - rc.tileV.z > transform.position.z)
        {
            Destroy(this.gameObject);
        }
    }
}
