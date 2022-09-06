using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelHandler : MonoBehaviour
{
    int level;
    public float progress;
    float toNextLevel;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI progressText;
    public Slider progressSlider;

    public Player player;

    public float shortDistance=0;

    public Animator animator;

    public RectTransform position;
    public GameObject _object;

    // Start is called before the first frame update
    void Start()
    {

        level = PlayerPrefs.GetInt("level", 1);
        progress = PlayerPrefs.GetFloat("levelProgress", 0f);
        toNextLevel = PlayerPrefs.GetFloat("toNextLevel", 10f);

        levelText.text = level.ToString();

        _object.transform.position = new Vector2(position.transform.position.x, position.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (progress + player.distance - shortDistance >= toNextLevel)
        {
            //show
            _object.transform.localPosition = new Vector2(0f, _object.transform.localPosition.y);
            /*_object.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            _object.gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            _object.gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);*/

            animator.Play("LevelUp");
            Debug.Log((progress + player.distance - shortDistance) + " >= " + toNextLevel);
            NextLevel();
            player.distance = 0;
            progress = 0;
        }

        progressText.text = Mathf.Floor(progress) + "/" + Mathf.Round(toNextLevel);
        progressSlider.value = progress / toNextLevel;


        //animator.GetComponent<RectTransform>().anchoredPosition = new Vector2(position.anchoredPosition.x, position.anchoredPosition.y);
        /*if (progress >= toNextLevel)
        {
            NextLevel();
            progress = 0;
        }*/
    }

    public void SaveProgress()
    {
        Debug.Log("short " + shortDistance);
        progress += (player.distance-shortDistance);
        Debug.Log("prog " + progress+" ");
        PlayerPrefs.SetFloat("levelProgress", progress);
        player.distance = 0;
        shortDistance = 0;
    }

    public void NextLevel()
    {
        shortDistance += Mathf.Floor(player.distance)-shortDistance;
        level++;
        PlayerPrefs.SetInt("level", level);

        PlayerPrefs.SetFloat("levelProgress", 0f);
        progress = 0f;

        toNextLevel = Mathf.Round(toNextLevel * 2.5f);
        PlayerPrefs.SetFloat("toNextLevel", toNextLevel);


        levelText.text = level.ToString();

        foreach (Mission mission in player.GetComponent<GameHandler>().missions.listOfMissions)
        {
            if (mission.missionToDo == missionToDo.LevelUp)
            {
                mission.amount++;
                mission.UpdateStatus();
            }
        }

        AudioManager.instance.Play("LevelUp");

    }



}
