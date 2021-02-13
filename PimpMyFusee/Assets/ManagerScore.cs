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
        manager = GetComponent<Manager>();
        mShop = ManagerShop.Instance;
    }

    private void Update()
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
        }
        mShop.Gold += Mathf.RoundToInt(Meter / 10);
        PlayerPrefs.SetInt("Gold",mShop.Gold);
        Meter = 0;
        scoreMax_t.text = MeterMax.ToString() + " MAX";
    }
}
