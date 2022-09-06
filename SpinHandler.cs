using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void In()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("SpinIn");
        AudioManager.instance.Play("Click");

    }

    public void Out()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("SpinOut");
    }

}
