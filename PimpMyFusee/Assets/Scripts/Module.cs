using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Main script for the modules
public class Module : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] Rigidbody2D rigidbody2d = null;
    [SerializeField] Collider2D collider2d = null;
    FixedJoint2D fixedJoint = null;


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


    bool connectedGraphicsEnabled = false;
    float connectedGraphicsEnableStartTime = 0f;


    [Header("GRAPHICS")]
    [SerializeField] Color connectColor = Color.red;
    SpriteRenderer[] spriteRenderers = null;
    List<Color> spriteRenderersBaseColors = new List<Color>();



    [Header("SUB MODULES")]
    [SerializeField] Mother01 mother01 = null;
    [SerializeField] Reactor01 reactor01 = null;
    [SerializeField] Blaster01 blaster01 = null;




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


        // GRAPHICS
        GetSpriteRenderers();
    }


    private void OnDestroy()                                                        // ON DESTROY
    {
        // COST
        if (ManagerShop.Instance != null)
            ManagerShop.Instance.AddEspace(-cost);
    }


    private void Update()                                                                                   // UPDATE
    {
        if (enabled && isActiveAndEnabled)
            if (connectedGraphicsEnabled && connectedGraphicsEnableStartTime + 0.05f < Time.time)
                EnableConnectGraphics(false);
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
        if (blaster01 != null)
            blaster01.TriggerBlaster(state);
        if (mother01 != null)
            mother01.TriggerMother(state);



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
                //fixedJoint = gameObject.AddComponent<FixedJoint2D>();
                //fixedJoint.connectedBody = connectedModule.rigidbody2d;
                //fixedJoint.breakForce = jointBreakForce;
                Connect(connectedModule);
                //fixedJoint.breakTorque = jointBreakForce;
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

    private void OnTriggerStay2D(Collider2D collision)                                                          // ON TRIGGER STAY 2D
    {
        if (dragging)
            if (collision.gameObject.GetComponent<Module>())
                collision.gameObject.GetComponent<Module>().EnableConnectGraphics(true);

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
        rigidbody2d.velocity = baseVelocity;
        rigidbody2d.angularVelocity = 0;
        //fixedJoint.enabled = true;
    }








    // GRAPHICS
    public void EnableConnectGraphics(bool on = false)
    {
        if (on)
        {
            if (spriteRenderers != null && spriteRenderers.Length > 0)
                for (int i = 0; i < spriteRenderers.Length; i++)
                    spriteRenderers[i].color = connectColor;

            connectedGraphicsEnabled = true;
            connectedGraphicsEnableStartTime = Time.time;
        }
        else
        {
            if (connectedGraphicsEnabled)
            {
                if (spriteRenderers != null && spriteRenderers.Length > 0)
                    for (int i = 0; i < spriteRenderers.Length; i++)
                        spriteRenderers[i].color = spriteRenderersBaseColors[i];

                connectedGraphicsEnabled = false;
            }
        }
    }


    // Store sprite renderers references and their colors on start
    void GetSpriteRenderers()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderersBaseColors.Add(spriteRenderers[i].color);
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
