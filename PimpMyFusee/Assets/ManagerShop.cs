using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerShop : MonoBehaviour
{
    [SerializeField] private GameObject moduleDrag;

    private void Update()
    {
        if (moduleDrag != null)
        {
            MoveTarget();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (moduleDrag != null)
            {
                moduleDrag = null;
            }
            else
            {

            }
        }
    }

    private void MoveTarget()
    {
        moduleDrag.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moduleDrag.transform.position = new Vector3(moduleDrag.transform.position.x, moduleDrag.transform.position.y, 0);
    }

    public void GetModule(int index)
    {
        Debug.Log(index);
        //Instantiate get index and module for resources
        moduleDrag = Instantiate(Resources.Load<GameObject>("Modules/ModuleTest"));
    }
}
