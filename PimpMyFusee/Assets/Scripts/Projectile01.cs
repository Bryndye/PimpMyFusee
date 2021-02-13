using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Module connectedModuleScript = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;


    [Header("PARAMETERS")]
    [SerializeField] float speed = 5;



    private void Awake()                                                            // AWAKE
    {
        GetReferences();
    }

    private void Start()                                                        // START
    {
        if (rigidbody2d != null)
            rigidbody2d.velocity = transform.up * speed;
    }


    // Get references if not set
    void GetReferences()
    {
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();

        if (connectedModuleScript == null)
            if (GetComponent<Module>())
                connectedModuleScript = GetComponent<Module>();
    }


    // EDITOR
    private void OnDrawGizmosSelected()                                                                 // ON DRAW GIZMOS SELECTED
    {
        GetReferences();
    }
}
