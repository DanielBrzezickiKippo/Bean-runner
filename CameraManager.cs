using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public GameObject tapToPlay;

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    void Follow()
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y+0.7f, player.transform.position.z - 17f);
    }

    public void SwitchToCharacter()
    {
        GetComponent<Animator>().Play("CameraToCharacter");
        tapToPlay.SetActive(false);
        StartCoroutine(delay(false,1.25f));
    }

    public void SwitchToPlay()
    {
        GetComponent<Animator>().Play("CameraToPlay");
            StartCoroutine(delay(true,1.25f));
    }

    IEnumerator delay(bool set, float t)
    {
        yield return new WaitForSeconds(t);
            tapToPlay.SetActive(set);
    }


}
