using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{


    public GameObject player;
    public Vector3 v;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeepDistance();
    }

    public void KeepDistance()
    {
        transform.position = new Vector3(
            transform.position.x,
            player.transform.position.y +v.y,
            player.transform.position.z + v.z
            );
    }

}
