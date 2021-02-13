using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script handling the blaster
public class Blaster01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Transform firePoint = null;
    [SerializeField] Module connectedModuleScript = null;


    [SerializeField] GameObject projectilePrefab = null;


    [Header("SETTINGS")]
    [SerializeField] float fireRate = 1f;
    bool blasterActivated = false;
    float lastShootStartTime = 0;







    #region FUNCTIONS
    private void Awake()                                                                    // AWAKE
    {
        GetReferences();
    }


    void FixedUpdate()                                                              // FIXEDUPDATE
    {
        if (isActiveAndEnabled && enabled)
        {
            if (blasterActivated)
                if (lastShootStartTime + fireRate < Time.time)
                    Shoot();
        }
    }
    






    public void TriggerBlaster(bool on = false)
    {
        blasterActivated = on;
        if (on)
            Shoot();
    }

    void Shoot()
    {
        Projectile01 lastShotProjectile = null;

        if (projectilePrefab != null)
            lastShotProjectile = Instantiate(projectilePrefab, firePoint.position, transform.rotation).GetComponent<Projectile01>();
        
        lastShotProjectile.moduleShooter = connectedModuleScript;
        lastShootStartTime = Time.time;
    }









    // Get references if not here
    void GetReferences()
    {
        if (firePoint == null)
            if (transform.GetChild(transform.childCount - 1) != null)
                firePoint = transform.GetChild(transform.childCount - 1);

        if (connectedModuleScript == null)
            if (GetComponent<Module>())
                connectedModuleScript = GetComponent<Module>();
    }





    // EDITOR
    private void OnDrawGizmosSelected()                                                                         // ON DRAW GIZMOS SELECTED
    {
        GetReferences();
    }
    #endregion
}
