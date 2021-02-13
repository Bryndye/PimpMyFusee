using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Main script for the modules
public class Module : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Rigidbody2D rigidbody2d = null;
    [SerializeField] Collider2D collider2d = null;
    [SerializeField] FixedJoint2D fixedJoint = null;


    [Header("PARAMETERS")]
    [SerializeField] public int cost = 20;
    [SerializeField] float jointBreakForce = 8000f;
    Module connectedModule = null;
    bool activated = false;
    Vector2 startPosition = new Vector2();
    float startRotation = 0;
    Vector2 baseVelocity = new Vector2(0, 0);
    bool dragging = false;
    bool endDrag = false;
    bool connected = false;


    [Header("SUB MODULES")]
    [SerializeField] Reactor01 reactor01 = null;




    [Header("EDITOR")]
    [SerializeField] GameObject editorObjects = null;









    #region FUNCTIONS
    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
        DisableEditorInfo();


        // Get base values
        startPosition = transform.position;
        startRotation = transform.eulerAngles.z;
    }

    private void Start()                                                             // START
    {
        // When the module appears on screen it is not activated yet
        TriggerModule(false);

        // COST
        if (ManagerShop.Instance != null)
            ManagerShop.Instance.AddEspace(cost);
    }


    private void OnDestroy()                                                        // ON DESTROY
    {
        // COST
        if (ManagerShop.Instance != null)
            ManagerShop.Instance.AddEspace(-cost);
    }








    // MODULE
    /// <summary>
    /// Triggers all of the module's functions to start the simulation
    /// </summary>
    /// <param name="state">Should the module be turned on or off</param>
    public void TriggerModule(bool state = true)
    {
        activated = state;
        rigidbody2d.isKinematic = !state;

        // Sub modules
        if (reactor01 != null)
            reactor01.TriggerReactor(state);



        // Position
        if (state)
        {
            startRotation = transform.eulerAngles.z;
            startPosition = transform.position;
        } 
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startRotation);
            transform.position = startPosition;
            rigidbody2d.velocity = baseVelocity;
            rigidbody2d.angularVelocity = 0;

            if (connected)
            {
                fixedJoint = gameObject.AddComponent<FixedJoint2D>();
                fixedJoint.connectedBody = connectedModule.rigidbody2d;
                fixedJoint.breakForce = jointBreakForce;
                fixedJoint.breakTorque = jointBreakForce;
            }
        }
    }






    public void StartDrag(bool on = false)
    {
        collider2d.enabled = !on;
        rigidbody2d.isKinematic = !on;
        rigidbody2d.velocity = baseVelocity;
        //transform.parent = null;
        Destroy(fixedJoint);
        connectedModule = null;
        connected = false;
        //fixedJoint.connectedBody = null;
        //fixedJoint.enabled = false;
        endDrag = !on;
        dragging = on;

        Invoke("EndDrag", 0.1f);
    }

    void EndDrag()
    {
        endDrag = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (dragging)
            if (collision.gameObject.GetComponent<Module>())
                Debug.Log(collision.gameObject);

        if (endDrag)
        {
            endDrag = false;

            if (collision.gameObject.GetComponent<Module>())
                Connect(collision.gameObject.GetComponent<Module>());
        }
    }

    private void Connect(Module moduleToConnectTo)
    {
        //transform.parent = moduleToConnectTo.transform;
        connected = true;
        fixedJoint = gameObject.AddComponent<FixedJoint2D>();
        fixedJoint.connectedBody = moduleToConnectTo.rigidbody2d;
        fixedJoint.breakForce = jointBreakForce;
        fixedJoint.breakTorque = jointBreakForce;
        connectedModule = moduleToConnectTo;
        //fixedJoint.enabled = true;
    }








    // EDITOR
    private void GetComponentsIfNotReferenced()
    {
        // GET COMPONENTS IS NOT REFERENCED
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();

        if (collider2d == null)
            if (GetComponent<Collider2D>())
                collider2d = GetComponent<Collider2D>();

        if (fixedJoint == null)
            if (GetComponent<FixedJoint2D>())
                fixedJoint = GetComponent<FixedJoint2D>();
    }

    private void DisableEditorInfo()
    {
        if (editorObjects != null)
            editorObjects.SetActive(false);
    }

    private void OnDrawGizmosSelected()                                                         // ON DRAW GIZMOS SELECTED
    {
        GetComponentsIfNotReferenced();
    }
    #endregion
}
