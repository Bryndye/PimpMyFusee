using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script handling the blaster
public class Blaster01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Transform firePoint = null;



    [Header("SETTINGS")]
    [SerializeField] float fireRate = 1f;
    bool blasterActivated = false;
    float lastShootStartTime = 0;








    private void Awake()                                                                    // AWAKE
    {
        GetReferences();
    }


    void FixedUpdate()                                                              // FIXEDUPDATE
    {
        if (isActiveAndEnabled && enabled)
        {
            if (blasterActivated)
            {
            }
        }
    }







    public void TriggerBlaster(bool on = false)
    {
        blasterActivated = on;
    }






    // Get references if not here
    void GetReferences()
    {
        if (firePoint == null)
            if (transform.GetChild(transform.childCount - 1) != null)
                firePoint = transform.GetChild(transform.childCount - 1);
    }





    // EDITOR
    private void OnDrawGizmosSelected()                                                                         // ON DRAW GIZMOS SELECTED
    {
        GetReferences();
    }
}
