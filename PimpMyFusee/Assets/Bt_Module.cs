using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bt_Module : MonoBehaviour
{
    private ManagerShop ms;

    [SerializeField] private GameObject prefabModule;

    private Image icon;
    private Button bt;
    private Text text;

    private void Awake()
    {
        ms = ManagerShop.Instance;

        icon = GetComponent<Image>();
        bt = GetComponent<Button>();
        text = GetComponentInChildren<Text>();

        text.text = prefabModule.name;
        //icon.sprite = Resources.Load<Sprite>("Sprite/"+prefabModule.name);

        bt.onClick.AddListener( delegate { ms.GetModule(prefabModule); });
    }
}
