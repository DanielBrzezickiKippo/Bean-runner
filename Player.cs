using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float mainSpeed;
    public float speed=10f;
    public Camera screenCamera;

    public float distance=3.84f;

    public List<Boost> actualBoosts;
    public GameObject footPrint;


    public FailedHandler fh;

    public bool isAlive = true;



    // Start is called before the first frame update
    void Start()
    {
        playAnim("Run02");
        mainSpeed = speed;

        AudioManager.instance.Play("Yey");
        StartCoroutine(delaySoundtrack());
    }


    IEnumerator delaySoundtrack()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.Play("Soundtrack");
    }

    float maxSpeed = 50f;
    void Update()
    {
        distance = transform.position.z+3.84f;
        transform.Translate(Vector3.forward * Time.deltaTime * speed * (1 / Time.timeScale));
        Movement();

        if ((distance / 1000f) <= maxSpeed)
            speed = mainSpeed + (distance / 1000f);
        else
            speed = mainSpeed + maxSpeed;
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            slow = !slow;
            Time.timeScale = slow ? .5f : 1f;
        }*/

        footParticles();
    }

    float border = 5f;

    //public bool slow = false;
    void Movement()
    {

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = mousePos = Input.mousePosition;

            //if (screenCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Vector3.Distance(screenCamera.transform.position, transform.position))).x > -3.5f &&
            //    screenCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Vector3.Distance(screenCamera.transform.position, transform.position))).x < 3.5f)
            //{

            float x = screenCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Vector3.Distance(screenCamera.transform.position, transform.position))).x;
            float multiplier = 5f;

            if (x > border)
            {
                x = border;
                multiplier = 1.75f;
            }
            else if (x < -border)
            {
                x = -border;
                multiplier = 1.75f;
            }

                Vector3 targetPos = new Vector3(
                    x,
                    transform.position.y,
                    transform.position.z
                    );
            //}

            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed * multiplier);

        }

    }

    public void playAnim(string name)
    {

        gameObject.GetComponent<GameHandler>().playerCharacter.GetComponent<Animator>().Play(name);
    }

    public float timeToNextStep = 0.05f;
    float _time = 0;
    public void footParticles()
    {
        if (_time > 0)
        {
            _time -= Time.deltaTime;
        }
        else
        {
            Instantiate(GetComponent<GameHandler>().footPrintParticle, footPrint.transform.position, Quaternion.Euler(60f,0f,0f));
            //AudioManager.instance.Play("Foot");

            _time = timeToNextStep;
        }
    }


    public void fail()
    {
        AudioManager.instance.Play("Die");
        AudioManager.instance.Play("DieHit");

        AudioManager.instance.JustFade("Soundtrack");
        //fh.SetUI(Mathf.Floor(distance));
        StartCoroutine(delay());

        GetComponent<Player>().enabled = false;
        GetComponent<GameHandler>().spawner.SetActive(false);

        fh.score = distance;
        isAlive = false;

    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.8f);
        fh.SetUI(Mathf.Floor(distance));
        continued = true;
    }


    public bool continued = false;
    public void continueRun()
    {
        //Instantiate();
        fh.Out();
        forceClear();
        playAnim("Run02");
        GetComponent<GameHandler>().spawner.SetActive(true);

        isAlive = true;
    }




    public void forceClear()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject car in cars)
        {
            GameObject p =Instantiate(GetComponent<GameHandler>().carBoomParticle, car.transform.position, Quaternion.Euler(60f, 0f, 0f));
            switch (Random.Range(0, 4))
            {
                case 0:
                    AudioManager.instance.PlayInObject("CarHit", p);
                    break;
                case 1:
                    AudioManager.instance.PlayInObject("CarHit1", p);
                    break;
                case 2:
                    AudioManager.instance.PlayInObject("CarHit2", p);
                    break;
                case 3:
                    AudioManager.instance.PlayInObject("CarHit3", p);
                    break;

            }
            Destroy(car);
        }
    }
}
