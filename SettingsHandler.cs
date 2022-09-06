using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    public Animator animator;
    public Slider soundSlider;
    public Slider musicSlider;

    public Image[] icons;

    public Sprite[] iconSprites;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager am = AudioManager.instance;
        am.volume = PlayerPrefs.GetFloat("soundVolume", 1f);
        am.soundTrackVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        soundSlider.value = 0.5f * PlayerPrefs.GetFloat("soundVolume", 1f);
        musicSlider.value = 0.5f * PlayerPrefs.GetFloat("musicVolume", 1f);

        SetUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void In()
    {
        animator.Play("SettingsIn");
    }


    public void Out()
    {
        animator.Play("SettingsOut");
    }

    public void ChangeSoundVolume()
    {
        AudioManager.instance.volume = soundSlider.value/0.5f;
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value / 0.5f);

        SetUI();
    }

    public void ChangeMusicVolume()
    {
        AudioManager.instance.soundTrackVolume = musicSlider.value / 0.5f;
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value / 0.5f);
        SetUI();
    }


    void SetUI()
    {
        if (soundSlider.value / 0.5f <= 0)
            icons[0].sprite = iconSprites[0];
        else
            icons[0].sprite = iconSprites[1];

        if (musicSlider.value / 0.5f <= 0)
            icons[1].sprite = iconSprites[2];
        else
            icons[1].sprite = iconSprites[3];
    }

}
