using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Script for the reactor module
public class Shield01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Module connectedModuleScript = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;
    [SerializeField] Transform chargesParent = null;
    [SerializeField] GameObject chargeObjectRef = null;


    [Header("PARAMETERS")]
    bool shieldActivated = false;
    [SerializeField] float repelStrength = 100;
    [SerializeField] int maxCharges = 3;
    [HideInInspector] public int charges = 3;
    [SerializeField] float repelCooldown = 0.3f;
    bool canRepel = true;
    float repelCoolDownStartTime = 0;


    [Header("FX")]
    [SerializeField] ParticleSystem repelFX01 = null;






    #region FUNCTIONS
    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
    }

    private void FixedUpdate()                                                                  // FIXED UPDATE
    {
        if (enabled && isActiveAndEnabled)
            if (shieldActivated)
            {
                if (!canRepel && repelCoolDownStartTime + repelCooldown < Time.time)
                    canRepel = true;
            }
    }






    /// <summary>
    /// Turns the reactor propeling on or off
    /// </summary>
    /// <param name="state"></param>
    public void TriggerShield(bool state = false)
    {
        shieldActivated = state;
        if (state)
            charges = maxCharges;

        
        canRepel = true;

        connectedModuleScript.graphicsObject.SetActive(true);
        connectedModuleScript.collider2d.enabled = true;
        connectedModuleScript.triggerCollider2d.enabled = true;

        DisplayCharges();
    }




    void DisplayCharges()
    {
        // Clear charges
        for (int i = 0; i < chargesParent.childCount; i++)
            if (chargesParent.GetChild(i).gameObject.activeInHierarchy)
                Destroy(chargesParent.GetChild(i).gameObject);

        for (int i = 0; i < maxCharges; i++)
        {
            GameObject newCharge = Instantiate(chargeObjectRef, chargesParent);
            newCharge.SetActive(true);
            newCharge.transform.localScale = chargeObjectRef.transform.localScale;
            newCharge.transform.position = chargeObjectRef.transform.position + new Vector3(0.7f * i, 0, 0);
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shieldActivated)
            if (collision.gameObject != this.gameObject && !collision.gameObject.GetComponent<Module>())
                if (collision.gameObject.GetComponent<Rigidbody2D>())
                {
                    if (canRepel && charges > 0)
                    {
                        collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position) * repelStrength, ForceMode2D.Impulse);
                        if (repelFX01 != null)
                            repelFX01.Play();

                        charges--;
                        Destroy(chargesParent.GetChild(chargesParent.childCount - 1).gameObject);
                        canRepel = false;
                        repelCoolDownStartTime = Time.time;

                        if (charges <= 0)
                        {
                            // Disable shield
                            connectedModuleScript.graphicsObject.SetActive(false);
                            connectedModuleScript.collider2d.enabled = false;
                            connectedModuleScript.triggerCollider2d.enabled = false;
                        }
                    }
                }
    }





    // EDITOR
    private void GetComponentsIfNotReferenced()
    {
        // GET COMPONENTS IS NOT REFERENCED
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();

        if (connectedModuleScript == null)
            if (GetComponent<Module>())
                connectedModuleScript = GetComponent<Module>();
    }


    private void OnDrawGizmosSelected()                                                         // ON DRAW GIZMOS SELECTED
    {
        GetComponentsIfNotReferenced();
    }
    #endregion
}
