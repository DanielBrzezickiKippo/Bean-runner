using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Canvas canvas;

    public GameObject target;

    public int what = 0;

    public Vector2 newVector;
    public float variance;
    public float speed;

    bool was = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();

        Vector2 currPos = gameObject.transform.localPosition;
        newVector = new Vector2(currPos.x - Random.Range(-variance, variance), currPos.y - Random.Range(-variance, variance));
        if (what ==0)
            target = GetComponentInParent<RevenueParticles>().target;
        else if(what ==1)
            target = GetComponentInParent<RevenueParticles>().target2;
        else
            target = GetComponentInParent<RevenueParticles>().target3;

        speed = Random.Range(0.03f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!was)
        {
            gameObject.transform.localPosition = Vector2.Lerp(gameObject.transform.localPosition, newVector, speed);
            if (Vector2.Distance(gameObject.transform.localPosition, newVector) < 5f)
                was = true;
        }
        else
            Follow(target);
    }

    public void Follow(GameObject target)
    {
        Debug.Log("follow     " + target.transform.position.x + " " + target.transform.position.y);

        Vector3 pos = target.transform.position;
        gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, pos, speed);

        if (Vector2.Distance(gameObject.transform.position, pos) < 1f)
        {
            gameObject.transform.localScale = Vector2.Lerp(gameObject.transform.localScale, new Vector2(0f, 0f), speed * 5);
            if (Vector2.Distance(gameObject.transform.localScale, new Vector2(0f, 0f)) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        //gameObject.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);

    }
}
