using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    public WheelOfFortune _spinner;
    public bool once = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!_spinner.isStoped)
            return;

        if (!once)
        {
            Debug.Log(col.gameObject.name);
            //gameObject.GetComponent<Needle>().enabled = false;
            col.gameObject.GetComponent<SpinReward>().GiveReward();
            _spinner.canSpin = true;
            once = true;
        }
    }
}
