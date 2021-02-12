using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerShop : Singleton<ManagerShop>
{
    [SerializeField] private GameObject moduleDrag;
    [SerializeField] private Transform helpPlayer;


    private void Update()
    {
        if (helpPlayer != null)
        {
            helpPlayer.position = Input.mousePosition;
        }
        ModuleDrag();
        UpdateScore();
    }

    #region ModuleInWorld
    private void ModuleDrag()
    {
        if (moduleDrag != null)
        {
            MoveTarget();
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                moduleDrag.transform.Rotate(new Vector3(moduleDrag.transform.localEulerAngles.x, moduleDrag.transform.localEulerAngles.y, 45));
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                moduleDrag = null;
            }
        }
        else
        {
            GetModuleInWorld();
        }
    }

    private void GetModuleInWorld()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                moduleDrag = hit.collider.gameObject;
                //Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
            }
        }
    }

    private void MoveTarget()
    {
        moduleDrag.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moduleDrag.transform.position = new Vector3(moduleDrag.transform.position.x, moduleDrag.transform.position.y, 0);
    }
    #endregion

    public void InstantiateModule(GameObject prefab)
    {
        //Debug.Log(prefab);
        moduleDrag = Instantiate(Resources.Load<GameObject>("Modules/"+prefab.name));
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

    public void AddEspaceMax()
    {
        EspaceMax += 10;
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
}
