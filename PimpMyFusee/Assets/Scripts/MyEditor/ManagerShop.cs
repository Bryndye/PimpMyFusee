using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerShop : Singleton<ManagerShop>
{
    [SerializeField] private GameObject moduleInstance;
    [SerializeField] private Transform InputModuleInHand;
    [SerializeField] private Transform InputModuleDestroy;
    [SerializeField] private TextMeshProUGUI GoldBonus_t;

    private void Awake()                                                            // AWAKE
    {
        if (Instance != this)
            Destroy(this);
        Save();
    }

    private void Start()
    {
        CreateButtons();
    }

    private void Update()                                                       // UPDATE
    {
        if (!Manager.Instance.simulationStarted)
            ModuleDrag();
        UpdateScore();
        CheckMoney();

    }

    #region ModuleInWorld
    private void ModuleDrag()
    {
        if (moduleInstance != null)
        {
            if (InputModuleInHand != null)
            {
                InputModuleInHand.gameObject.SetActive(true);
                InputModuleInHand.position = Input.mousePosition;
            }
            if (InputModuleInHand != null)
            {
                InputModuleDestroy.gameObject.SetActive(false);
            }

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
        }
        else
        {
            GetModuleInWorld();
            if (InputModuleInHand != null)
            {
                InputModuleInHand.gameObject.SetActive(false);
            }
        }
    }

    private void GetModuleInWorld()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (InputModuleDestroy != null && !hit.collider.GetComponent<Mother01>() && hit.collider.GetComponent<Module>() != null)
            {
                InputModuleDestroy.gameObject.SetActive(true);
                InputModuleDestroy.position = Input.mousePosition;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (hit.collider.gameObject.GetComponent<Module>())
                {
                    moduleInstance = hit.collider.gameObject;
                    Module moduleScript = moduleInstance.GetComponent<Module>();
                    moduleScript.StartDrag(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Mother01 mom = hit.collider.GetComponent<Mother01>();
                if (mom == null && hit.collider.gameObject.GetComponent<Module>())
                {
                    Destroy(hit.collider.gameObject);
                }
            } 
        }
        else
        {
            if (InputModuleDestroy != null)
            {
                InputModuleDestroy.gameObject.SetActive(false);
            }
        }
    }

    private void MoveTarget()
    {
        //moduleInstance.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //moduleInstance.transform.position = new Vector3(moduleInstance.transform.position.x, moduleInstance.transform.position.y, 0);
        moduleInstance.GetComponent<Rigidbody2D>().MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    #endregion
    
    public void InstantiateModule(GameObject prefab)
    {
        Instantiate(Resources.Load<GameObject>("Modules/"+prefab.name));
        //l.transform.position = new Vector3(0,3,0);
    }

    [Header("MODULES BUTTONS")]
    [SerializeField] RectTransform buttonsParent = null;
    [SerializeField] GameObject buttonPrefab = null;
    [SerializeField] GameObject buyButtonPrefab = null;
    [SerializeField] public ModulesData modulesData = null;
    List<GameObject> buttonsList = new List<GameObject>();

    
    public void CreateButtons()
    {
        SaveModules();

        for (int i = 0; i < buttonsParent.childCount; i++)
            Destroy(buttonsParent.GetChild(i).gameObject);
        buttonsList.Clear();
        

        float horizontalSize = 0;

        if (modulesData != null && modulesData.modulesList.Count > 0)
            for (int i = 0; i < modulesData.modulesList.Count; i++)
            {
                Bt_Module buttonScript = null;
                if (modulesData.modulesList[i].locked)
                {
                    GameObject newButton;
                    buttonsList.Add(Instantiate(buyButtonPrefab, buttonsParent));
                    newButton = buttonsList[i];
                    buttonScript = buttonsList[i].GetComponent<Bt_Module>();
                    buttonScript.buyPrice = modulesData.modulesList[i].price;
                    buttonScript.coutModule_t.text = modulesData.modulesList[i].price.ToString();
                    buttonScript.moduleIndex = i;
                    buttonScript.isBuyButton = true;
                    buttonScript.GetComponent<Button>().onClick.AddListener(delegate { buttonScript.BuyModule(); });
                }
                else
                {
                    buttonsList.Add(Instantiate(buttonPrefab, buttonsParent));
                    buttonScript = buttonsList[i].GetComponent<Bt_Module>();
                    buttonScript.prefabModule = modulesData.modulesList[i].prefab;
                    buttonScript.coutModule_t.text = modulesData.modulesList[i].prefab.GetComponent<Module>().cost.ToString();
                    buttonScript.SetUpButton();
                }

                
                buttonScript.moduleIcon.sprite = modulesData.modulesList[i].icon;

                horizontalSize += buttonsList[i].GetComponent<RectTransform>().sizeDelta.x + buttonsParent.gameObject.GetComponent<HorizontalLayoutGroup>().spacing;
            }

        buttonsParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalSize);
    }
    

    public void BuyModule(int index)
    {
        Debug.Log(index);
        
        ModulesData.Module module = modulesData.modulesList[index];
        module.locked = false;
        modulesData.modulesList[index] = module;

        CreateButtons();
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
    [SerializeField] TextMeshProUGUI priceUpText = null;
    [SerializeField] GameObject goldUp = null;
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
            //bt_up.GetComponentInChildren<TextMeshProUGUI>().text = PriceUp.ToString();
            if (priceUpText != null)
                priceUpText.text = PriceUp.ToString(); ;

            if (Gold >= PriceUp)
                bt_up.interactable = true;
            else
                bt_up.interactable = false;
        }
    }

    public void AddGold(int gold, float hauteur)
    {
        Gold += gold;
        PlayerPrefs.SetInt("Gold",Gold);
        goldUp.GetComponent<GoldPlus>().amountText.text = gold.ToString();
        goldUp.SetActive(false);
        goldUp.SetActive(true);
        
        if (GoldBonus_t != null && hauteur > 0)
        {
            GoldBonus_t.text = "Objectif: " + hauteur+" reach!\nGold: " + gold;
            Invoke(nameof(DesaText), 3);   
        }
    }

    private void DesaText()
    {
        GoldBonus_t.text = "";
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

    #region PlayerPrefs
    private void Save()
    {
        if (PlayerPrefs.HasKey("Gold"))
            Gold = PlayerPrefs.GetInt("Gold");
        else
            PlayerPrefs.SetInt("Gold", Gold);


        if (PlayerPrefs.HasKey("PriceUp"))
            PriceUp = PlayerPrefs.GetInt("PriceUp");
        else
            PlayerPrefs.SetInt("PriceUp", PriceUp);

        
        if (PlayerPrefs.HasKey("EspaceMax"))
            EspaceMax = PlayerPrefs.GetInt("EspaceMax");
        else
            PlayerPrefs.SetInt("EspaceMax", EspaceMax);


        if (PlayerPrefs.HasKey("Modules") && PlayerPrefs.GetString("Modules").Length == modulesData.modulesList.Count)
        {
            string boughtModules = PlayerPrefs.GetString("Modules");
            List<ModulesData.Module> modulesList = modulesData.modulesList;


            for (int i = 0; i < modulesList.Count; i++)
            {
                ModulesData.Module module = modulesData.modulesList[i];
                if (boughtModules[i].Equals('0'))
                    module.locked = false;
                else
                    module.locked = true;
                modulesData.modulesList[i] = module;
            }
        }
        else
        {
            SaveModules();
        }
    }

    void SaveModules()
    {
        string boughtModules = "";
        for (int i = 0; i < modulesData.modulesList.Count; i++)
        {
            if (modulesData.modulesList[i].locked)
                boughtModules += "1";
            else
                boughtModules += "0";
        }
        PlayerPrefs.SetString("Modules", boughtModules);
        Debug.Log("Set modules save");
    }

    public void ResetSave()
    {
        PlayerPrefs.SetInt("EspaceMax", 100);
        PlayerPrefs.SetInt("PriceUp", 100);
        PlayerPrefs.SetInt("Gold", 100);
        PlayerPrefs.SetInt("MeterMax", 0);
        PlayerPrefs.SetString("Modules", "");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
