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

    public void GetModule(int index)
    {
        Debug.Log(index);
        //Instantiate get index and module for resources
        moduleDrag = Instantiate(Resources.Load<GameObject>("Modules/ModuleTest"));
    }
}
