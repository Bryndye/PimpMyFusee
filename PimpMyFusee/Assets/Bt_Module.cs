using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bt_Module : MonoBehaviour
{
    private ManagerShop ms;

    [SerializeField] private GameObject prefabModule;

    public int EspacePris;

    private Image icon;
    private Button bt;
    private Text text;

    private void Awake()
    {
        ms = ManagerShop.Instance;

        icon = GetComponent<Image>();
        bt = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        var g = Instantiate(Resources.Load<GameObject>("Modules/" + prefabModule.name));
        //EspacePris = g.GetComponent<Module>().cost;

        text.text = prefabModule.name;
        //icon.sprite = Resources.Load<Sprite>("Sprite/"+prefabModule.name);

        bt.onClick.AddListener( delegate { ms.GetModule(prefabModule); });
    }

    private void Update()
    {
        if (ms.EspacePris + EspacePris <= ms.EspaceMax)
        {
            bt.interactable = true;
        }
        else
        {
            bt.interactable = false;
        }
    }
}
