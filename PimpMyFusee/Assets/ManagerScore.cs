using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerScore : Singleton<ManagerScore>
{
    private ManagerShop mShop;
    private Manager manager;

    [SerializeField] private Transform motherModule;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI score_t;
    [SerializeField] private TextMeshProUGUI scoreMax_t;
    public int Meter;
    public int MeterMax;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
        if (PlayerPrefs.HasKey("MeterMax"))
        {
            MeterMax = PlayerPrefs.GetInt("MeterMax");
            //Debug.Log("setup MeterMax");
        }
        else
        {
            PlayerPrefs.SetInt("MeterMax", MeterMax);
            //Debug.Log("create key MeterMax");
        }
        scoreMax_t.text = MeterMax + " MAX";
        manager = GetComponent<Manager>();
        mShop = ManagerShop.Instance;
    }

    private void Update()                                                           // UPDATE
    {
        if (manager.simulationStarted)
        {
            if (motherModule.position.y > Meter)
            {
                Meter = Mathf.RoundToInt(motherModule.position.y);
            }
        }
        score_t.text = Meter + "m";

    }

    public void GetTheScore()
    {
        if (Meter > MeterMax)
        {
            MeterMax = Meter;
            PlayerPrefs.SetInt("MeterMax", MeterMax);
        }
        mShop.Gold += Mathf.RoundToInt(Meter / 10);
        PlayerPrefs.SetInt("Gold",mShop.Gold);
        Meter = 0;
        scoreMax_t.text = MeterMax.ToString() + " MAX";
    }
}
