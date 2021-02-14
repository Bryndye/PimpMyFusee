using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bt_Module : MonoBehaviour
{
    private ManagerShop ms;

    public GameObject prefabModule;

    public int EspacePris;
    public int moduleIndex = 0;
    public bool isBuyButton = false;
    public int buyPrice = 0;
    private Image icon;
    private Button bt;
    [SerializeField] public Image moduleIcon = null;
    [SerializeField] private Text nomModuel_t;
    [SerializeField] public Text coutModule_t;
    private void Awake()                                                                    // AWAKE
    {
        ms = ManagerShop.Instance;

        icon = GetComponent<Image>();
        bt = GetComponent<Button>();

        var g = Resources.Load<GameObject>("Modules/" + prefabModule.name);

        EspacePris = g.GetComponent<Module>().cost;
        coutModule_t.text = EspacePris.ToString();
        nomModuel_t.text = prefabModule.name;
        //icon.sprite = Resources.Load<Sprite>("Icon/"+prefabModule.name);

        //bt.onClick.AddListener( delegate { ms.InstantiateModule(prefabModule); });
    }
    
    public void SetUpButton()
    {
        bt.onClick.AddListener(delegate { ms.InstantiateModule(prefabModule); });
    }

    private void Update()                                                                       // UPDATE
    {
        if (enabled && isActiveAndEnabled)
        {
            if (isBuyButton)
            {
                if (ms.Gold >= buyPrice)
                    bt.interactable = true;
                else
                    bt.interactable = false;
            }
            else
            {
                if (ms.EspacePris + EspacePris <= ms.EspaceMax)
                    bt.interactable = true;
                else
                    bt.interactable = false;
            }
        } 
    }

    private void OnDrawGizmos()                                                                                 // ON DRAW GIZMOS
    {
        if (prefabModule != null)
        {
            gameObject.name = "Button " + prefabModule.name;
        }
        else
        {
            gameObject.name = "Button ";
        }
    }


    public void BuyModule()
    {
        ModulesData.Module module = ManagerShop.Instance.modulesData.modulesList[moduleIndex];
        module.locked = false;
        ManagerShop.Instance.modulesData.modulesList[moduleIndex] = module;

        ManagerShop.Instance.CreateButtons();
    }
}
