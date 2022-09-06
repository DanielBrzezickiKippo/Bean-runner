using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenueParticles : MonoBehaviour
{

    Canvas canvas;
    public GameObject prefab;
    public GameObject prefabGem;
    public GameObject prefabPower;

    public GameObject target;
    public GameObject target2;
    public GameObject target3;
    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn()
    {
        Vector2 movePos = Input.mousePosition;

        /*RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera,
            out movePos);*/

        //transform.position = canvas.transform.TransformPoint(movePos);

        for (int i = 0; i < 10; i++)
        {
            GameObject o = Instantiate(prefab, gameObject.transform);

            //Vector3 finalPos = canvas.transform.TransformPoint(movePos);

            o.transform.position = new Vector3(Input.mousePosition.x + Random.Range(-50, 50), Input.mousePosition.y);//canvas.transform.TransformPoint(movePos);

        }
    }

    public void SpawnGems()
    {
        Vector2 movePos = Input.mousePosition;


        for (int i = 0; i < 10; i++)
        {
            GameObject o = Instantiate(prefabGem, gameObject.transform);


            o.transform.position = new Vector3(Input.mousePosition.x + Random.Range(-50, 50), Input.mousePosition.y);//canvas.transform.TransformPoint(movePos);

        }
    }


    public void SpawnPower()
    {
        Vector2 movePos = Input.mousePosition;



        //transform.position = canvas.transform.TransformPoint(movePos);

        for (int i = 0; i < 10; i++)
        {
            GameObject o = Instantiate(prefabPower, gameObject.transform);

            //Vector3 finalPos = canvas.transform.TransformPoint(movePos);

            o.transform.position = new Vector3(Input.mousePosition.x + Random.Range(-50, 50), Input.mousePosition.y);//canvas.transform.TransformPoint(movePos);

        }
    }
}
