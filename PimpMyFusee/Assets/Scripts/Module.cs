using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Main script for the modules
public class Module : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody2d = null;
    bool activated = false;








    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
    }

    private void Start()                                                             // START
    {
        // When the module appears on screen it is not activated yet
        TriggerModule(false);
    }








    // MODULE
    public void TriggerModule(bool state = true)
    {
        rigidbody2d.isKinematic = !state;
    }












    private void GetComponentsIfNotReferenced()
    {
        // GET COMPONENTS IS NOT REFERENCED
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // EDITOR
    private void OnDrawGizmosSelected()
    {
        GetComponentsIfNotReferenced();
    }
}
