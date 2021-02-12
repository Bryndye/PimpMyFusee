using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerShop : Singleton<ManagerShop>
{
    [SerializeField] private GameObject moduleDrag;

    public int EspacePris;
    public int EspaceMax;

    private void Update()
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
    }

    private void MoveTarget()
    {
        moduleDrag.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moduleDrag.transform.position = new Vector3(moduleDrag.transform.position.x, moduleDrag.transform.position.y, 0);
    }

    public void GetModule(GameObject prefab)
    {
        //Debug.Log(prefab);
        moduleDrag = Instantiate(Resources.Load<GameObject>("Modules/"+prefab.name));
    }

    public void AddEspace(int esp)
    {
        EspacePris += esp;
        if (EspacePris < 0)
        {
            EspacePris = 0;
        }
    }
}
