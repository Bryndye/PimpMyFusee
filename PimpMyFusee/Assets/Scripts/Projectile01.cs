using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile01 : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] public Module moduleShooter = null;
    [SerializeField] Rigidbody2D rigidbody2d = null;


    [Header("PARAMETERS")]
    [SerializeField] float speed = 5;



    private void Awake()                                                            // AWAKE
    {
        GetReferences();
    }

    private void Start()                                                        // START
    {
        Invoke("StartProjectile", 0.05f);
    }

    void StartProjectile()
    {
        if (rigidbody2d != null && moduleShooter != null)
            rigidbody2d.velocity = moduleShooter.rigidbody2d.velocity + rigidbody2d.velocity + (Vector2)transform.up * speed;
    }

    // Get references if not set
    void GetReferences()
    {
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();

        if (moduleShooter == null)
            if (GetComponent<Module>())
                moduleShooter = GetComponent<Module>();
    }


    // EDITOR
    private void OnDrawGizmosSelected()                                                                 // ON DRAW GIZMOS SELECTED
    {
        GetReferences();
    }
}
