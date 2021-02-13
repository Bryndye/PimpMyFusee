using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerShop : Singleton<ManagerShop>
{
    [SerializeField] private GameObject moduleInstance;
    [SerializeField] private Transform helpPlayer;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
        Save();
    }

    private void Update()
    {
        if (helpPlayer != null)
        {
            helpPlayer.position = Input.mousePosition;
        }
        ModuleDrag();
        UpdateScore();
        CheckMoney();
    }

    #region ModuleInWorld
    private void ModuleDrag()
    {
        if (moduleInstance != null)
        {
            MoveTarget();
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                moduleInstance.transform.Rotate(new Vector3(moduleInstance.transform.localEulerAngles.x, moduleInstance.transform.localEulerAngles.y, 45));
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                moduleInstance.transform.Rotate(new Vector3(moduleInstance.transform.localEulerAngles.x, moduleInstance.transform.localEulerAngles.y, -45));
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                moduleInstance.GetComponent<Module>().StartDrag(false);
                moduleInstance = null;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Destroy(moduleInstance);
            }
        }
        else
        {
            GetModuleInWorld();
        }
    }

    private void GetModuleInWorld()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<Module>())
                {
                    moduleInstance = hit.collider.gameObject;
                    Module moduleScript = moduleInstance.GetComponent<Module>();
                    moduleScript.StartDrag(true);
                }
                //Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
            }
        }
    }

    private void MoveTarget()
    {
        //moduleInstance.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //moduleInstance.transform.position = new Vector3(moduleInstance.transform.position.x, moduleInstance.transform.position.y, 0);

        moduleInstance.GetComponent<Rigidbody2D>().MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //moduleInstance.transform.position = new Vector3(moduleInstance.transform.position.x, moduleInstance.transform.position.y, 0);
    }
    #endregion

    public void InstantiateModule(GameObject prefab)
    {
        //Debug.Log(prefab);
        moduleInstance = Instantiate(Resources.Load<GameObject>("Modules/"+prefab.name));
    }

    #region Gestion Eco
    [Header("Espace")]
    public int EspacePris;
    public int EspaceMax = 100;

    public void AddEspace(int esp)
    {
        EspacePris += esp;
        if (EspacePris < 0)
        {
            EspacePris = 0;
        }
    }

    [Header("tune")]
    public int Gold;
    public int PriceUp = 100;
    [SerializeField] private Button bt_up;
    public void AddEspaceMax()
    {
        Gold -= PriceUp;
        EspaceMax += 10;
        PriceUp += 100;
        PlayerPrefs.SetInt("PriceUp", PriceUp);
        PlayerPrefs.SetInt("EspaceMax", EspaceMax);
    }
    private void CheckMoney()
    {
        if (bt_up != null)
        {
            bt_up.GetComponentInChildren<TextMeshProUGUI>().text = PriceUp.ToString();
            if (Gold >= PriceUp)
            {
                bt_up.interactable = true;
            }
            else
            {
                bt_up.interactable = false;
            }
        }
    }
    #endregion

    #region UI
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI gold_t;
    [SerializeField] private TextMeshProUGUI espace_t;
    private void UpdateScore()
    {
        if (gold_t != null && espace_t != null)
        {
            gold_t.text = Gold.ToString(); ;
            espace_t.text = EspacePris +"/"+ EspaceMax;
        }
    }
    #endregion

    private void Save()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            Gold = PlayerPrefs.GetInt("Gold");
            Debug.Log("setup gold");
        }
        else
        {
            PlayerPrefs.SetInt("Gold", Gold);
            Debug.Log("create key");
        }

        if (PlayerPrefs.HasKey("PriceUp"))
        {
            PriceUp = PlayerPrefs.GetInt("PriceUp");
            Debug.Log("setup PriceUp");
        }
        else
        {
            PlayerPrefs.SetInt("PriceUp", PriceUp);
            Debug.Log("create PriceUp");
        }
        
        if (PlayerPrefs.HasKey("EspaceMax"))
        {
            EspaceMax = PlayerPrefs.GetInt("EspaceMax");
            Debug.Log("setup EspaceMax");
        }
        else
        {
            PlayerPrefs.SetInt("EspaceMax", EspaceMax);
            Debug.Log("create EspaceMax");
        }
    }
}
