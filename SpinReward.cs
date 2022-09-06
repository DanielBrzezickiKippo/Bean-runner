using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinReward : MonoBehaviour
{
    public enum Reward
    {
        gems,
        power,
        coins,
        reroll
    }

    public Reward reward;
    public float amount;
    public WheelOfFortune wof;
    public GameHandler gh;
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveReward()
    {
        ps.textureSheetAnimation.SetSprite(0, transform.GetChild(0).GetComponent<Image>().sprite);
        switch (reward)
        {
            case Reward.gems:
                gh.GetGems((int)amount);
                break;
            case Reward.coins:
                gh.GetMoney((int)amount);
                break;
            case Reward.power:
                gh.GetPower((int)amount);
                break;
            case Reward.reroll:
                wof.spins += (int)amount;
                wof.SetUI();
                break;
        }
        ps.Play();
    }
}

