using UnityEngine;

public class CarManager : MonoBehaviour
{
    public RoadCreator rc;
    public GameObject player;

    AudioManager am;



    // Start is called before the first frame update
    void Start()
    {
        rc = GameObject.FindGameObjectWithTag("RoadCreator").GetComponent<RoadCreator>();
        player = GameObject.FindGameObjectWithTag("Player");

        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        switch (Random.Range(0, 5))
        {
            case 0:
                am.PlayInObject("CarEngine", gameObject);
                break;
            case 1:
                am.PlayInObject("CarEngine1", gameObject);
                break;
            case 2:
                am.PlayInObject("CarEngine2", gameObject);
                break;
            case 3:
                am.PlayInObject("CarEngine3", gameObject);
                break;
            case 4:
                am.PlayInObject("CarEngine4", gameObject);
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }
    bool once = false;



    public void Check()
    {
        if (player.transform.position.z - rc.tileV.z*2 > transform.position.z)
        {
            
            Destroy(this.gameObject);
        }

        if (Vector3.Distance(player.transform.position, transform.position) < 3.5f)
        {
            //Debug.Log("flyby!!!!");
            if (!once)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        am.PlayInObject("CarFlyby1", gameObject);
                        break;
                    case 1:
                        am.PlayInObject("CarFlyby2", gameObject);
                        break;
                    case 2:
                        am.PlayInObject("CarFlyby3", gameObject);
                        break;

                }
                once = true;
            }
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player.GetComponent<GameHandler>().ciotkaBoost == false)
            {
                if (player.GetComponent<Player>().isAlive)
                {
                    player.GetComponent<Player>().fail();
                    player.GetComponent<GameHandler>().playerCharacter.GetComponent<Animator>().Play("Die01");
                }
                //Player died animation
            }
            else
            {
                GameObject p =Instantiate(player.GetComponent<GameHandler>().carBoomParticle, transform.position, Quaternion.Euler(60f, 0f, 0f));
                switch (Random.Range(0, 4))
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
                Destroy(this.gameObject);
            }
        }

        if (Random.Range(0, 10) > 3)
        {
            if (collision.gameObject.tag == "Road")
            {
                GameObject[] particles = player.GetComponent<GameHandler>().carHitParticles;
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                GameObject particle = Instantiate(particles[Random.Range(0, particles.Length)], pos, rot);
                particle.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                switch (Random.Range(0, 4))
                {
                    case 0:
                        am.PlayInObject("CarGround1", gameObject);
                        break;
                    case 1:
                        am.PlayInObject("CarGround2", gameObject);
                        break;
                    case 2:
                        am.PlayInObject("CarGround3", gameObject);
                        break;
                    case 3:
                        am.PlayInObject("CarGround4", gameObject);
                        break;
                }
            }
        }
    }
}
