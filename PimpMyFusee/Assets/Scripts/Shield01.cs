using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Script for the reactor module
public class Shield01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Module connectedModuleScript = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;


    [Header("PARAMETERS")]
    bool shieldActivated = false;
    [SerializeField] float repelStrength = 100;

    [Header("FX")]
    [SerializeField] ParticleSystem repelFX01 = null;






    #region FUNCTIONS
    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
    }





    /// <summary>
    /// Turns the reactor propeling on or off
    /// </summary>
    /// <param name="state"></param>
    public void TriggerShield(bool state = false)
    {
        shieldActivated = state;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shieldActivated)
            if (collision.gameObject != this.gameObject)
                if (collision.gameObject.GetComponent<Rigidbody2D>())
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position) * repelStrength, ForceMode2D.Impulse);
                    if (repelFX01 != null)
                        repelFX01.Play();
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
