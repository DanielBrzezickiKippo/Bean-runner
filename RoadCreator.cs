using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadCreator : MonoBehaviour
{
    public Vector3 tileV;
    public Vector3 tileUrbanV;
    public GameObject roadPrefab;

    [SerializeField] public List<GameObject> roadTiles;
    public GameObject player;

    public List<GameObject> buildingPrefabs;
    [SerializeField] public List<GameObject> buildingTiles;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            CreateStart();
        }
        for (int i = 0; i < 10; i++)
        {
            Create();
            CreateUrban();
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (player.transform.position.z- (2*tileV.z) > roadTiles[0].transform.position.z)
        {
            Create();
            GameObject toDel = roadTiles[0];
            roadTiles.RemoveAt(0);

            Destroy(toDel);
        }

        if (player.transform.position.z - (2 * tileUrbanV.z) > buildingTiles[0].transform.position.z)
        {
            CreateUrban();
            GameObject toDelLeft = buildingTiles[0];
            GameObject toDelRight = buildingTiles[1];
            buildingTiles.RemoveAt(0);
            buildingTiles.RemoveAt(0);

            Destroy(toDelLeft);
            Destroy(toDelRight);
        }

    }

    int count = 0;
    public void CreateStart()
    {
        Vector3 lastPos = roadTiles[0].transform.localPosition;
        if (count != 0)
        {

            GameObject roadTile = Instantiate(roadPrefab, roadsHandlerObject.transform);


            roadTile.transform.localPosition = new Vector3(
                tileV.x,
                -2f,//lastPos.y - count * tileV.y,
                lastPos.z - count * tileV.z
                );
        }

        GameObject leftUrbanTile = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Count)], roadsHandlerObject.transform);
        GameObject rightUrbanTile = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Count)], roadsHandlerObject.transform);

        leftUrbanTile.transform.eulerAngles = new Vector3(0f, 90f, -30f);
        rightUrbanTile.transform.eulerAngles = new Vector3(0f, -90f, 30f);

        if (buildingTiles.Count > 0)
            lastPos = buildingTiles[0].transform.localPosition;
        else
            lastPos = new Vector3(0f, -14.29f, -19.24f);

        leftUrbanTile.transform.localPosition = new Vector3(
            -12f,
            -2.03f,
            lastPos.z - count * tileUrbanV.z
            );

        rightUrbanTile.transform.localPosition = new Vector3(
            12f,
            -2.03f,
            lastPos.z - count * tileUrbanV.z
        );


        count++;
    }

    public GameObject roadsHandlerObject;

    public void Create()
    {
        GameObject roadTile = Instantiate(roadPrefab, roadsHandlerObject.transform);

        //roadTile.transform.eulerAngles = new Vector3(-30f, 0f, 0f);

        Vector3 lastPos = roadTiles[roadTiles.Count-1].transform.localPosition;
        roadTile.transform.localPosition = new Vector3(
            tileV.x,
            -2f,//lastPos.y + tileV.y,
            lastPos.z + tileV.z
            );


        roadTiles.Add(roadTile);

    }

    public void CreateUrban()
    {
        //SceneManager.LoadSceneAsync(scenesPrefabs[Random.Range(0, scenesPrefabs.Length)],LoadSceneMode.Additive) ;
        GameObject leftUrbanTile = Instantiate(buildingPrefabs[Random.Range(0,buildingPrefabs.Count)], roadsHandlerObject.transform);
        GameObject rightUrbanTile = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Count)], roadsHandlerObject.transform);


        leftUrbanTile.transform.eulerAngles = new Vector3(0f, 90f, -30f);
        rightUrbanTile.transform.eulerAngles = new Vector3(0f, -90f, 30f);

        Vector3 lastPos;
        if (buildingTiles.Count>0)
            lastPos = buildingTiles[buildingTiles.Count - 1].transform.localPosition;
        else
            lastPos = new Vector3(0f, -14.29f, -19.24f);

        leftUrbanTile.transform.localPosition = new Vector3(
            -12f,
            -2.03f,
            lastPos.z + tileUrbanV.z
            );

        rightUrbanTile.transform.localPosition = new Vector3(
            12f,
            -2.03f,
            lastPos.z + tileUrbanV.z
        );

        buildingTiles.Add(leftUrbanTile);
        buildingTiles.Add(rightUrbanTile); 

        //roadTiles.Add(roadTile);

    }

}
