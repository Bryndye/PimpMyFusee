using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Main script for the modules
public class Module : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] public Rigidbody2D rigidbody2d = null;
    [SerializeField] Collider2D collider2d = null;
    FixedJoint2D fixedJoint = null;

    [Header("DATA")]
    [SerializeField] LayerMask defaultLayer = 0;
    [SerializeField] LayerMask connectedLayer = 0;


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
    [SerializeField] Control01 control01 = null;
    [SerializeField] Shield01 shield01 = null;




    [Header("EDITOR")]
    [SerializeField] GameObject editorObjects = null;


    [Header("Animator")]
    private Animator anim;
    bool JustInitialized = true;






    #region FUNCTIONS
    private void Awake()                                                            // AWAKE
    {
        GetComponentsIfNotReferenced();
        DisableEditorInfo();

        anim = GetComponentInChildren<Animator>();

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
        if (control01 != null)
            control01.TriggerControl(state);
        if (shield01 != null)
            shield01.TriggerShield(state);


        rigidbody2d.velocity = Vector3.zero;
        rigidbody2d.angularVelocity = 0;

        // Position
        if (state)
        {
            startRotation = transform.eulerAngles.z;
            startPosition = transform.position;
            if (anim && reactor01 != null)
            {
                anim.SetTrigger("On");
            }
        } 
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startRotation);
            transform.position = startPosition;

            if (anim && reactor01 != null)
            {
                if (JustInitialized)
                    JustInitialized = false;
                else
                    anim.SetTrigger("Off");
            }

            if (connected)
            {
                //fixedJoint = gameObject.AddComponent<FixedJoint2D>();
                //fixedJoint.connectedBody = connectedModule.rigidbody2d;
                //fixedJoint.breakForce = jointBreakForce;
                StartDrag(true);
                StartDrag(false);
                //Connect(connectedModule);
                //fixedJoint.breakTorque = jointBreakForce;
            }
        }
    }






    public void StartDrag(bool on = false)
    {
        collider2d.enabled = !on;
        rigidbody2d.isKinematic = !on;
        rigidbody2d.velocity = Vector3.zero;
        rigidbody2d.angularVelocity = 0;
        if (on)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startRotation);
        //transform.parent = null;
        Destroy(fixedJoint);
        connectedModule = null;
        connected = false;
        //fixedJoint.connectedBody = null;
        //fixedJoint.enabled = false;
        endDrag = !on;
        dragging = on;

        Invoke("EndDrag", 0.1f);


        // COLLISION LAYER
        //gameObject.layer = defaultLayer;
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
        if (fixedJoint != null)
            Destroy(fixedJoint);
        fixedJoint = gameObject.AddComponent<FixedJoint2D>();
        fixedJoint.connectedBody = moduleToConnectTo.rigidbody2d;
        fixedJoint.breakForce = jointBreakForce;
        fixedJoint.breakTorque = jointBreakForce;
        connectedModule = moduleToConnectTo;
        rigidbody2d.velocity = Vector3.zero;
        rigidbody2d.angularVelocity = 0;
        rigidbody2d.isKinematic = true;
        //fixedJoint.enabled = true;

        // COLLISION LAYER
        //gameObject.layer = connectedLayer;
        //Debug.Log(connectedLayer);
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
        // GET COMPONENTS IF NOT REFERENCED
        if (rigidbody2d == null)
            if (GetComponent<Rigidbody2D>())
                rigidbody2d = GetComponent<Rigidbody2D>();

        if (collider2d == null)
            if (GetComponent<Collider2D>())
                collider2d = GetComponent<Collider2D>();

        if (fixedJoint == null)
            if (GetComponent<FixedJoint2D>())
                fixedJoint = GetComponent<FixedJoint2D>();

        if (reactor01 == null)
            if (GetComponent<Reactor01>())
                reactor01 = GetComponent<Reactor01>();

        if (mother01 == null)
            if (GetComponent<Mother01>())
                mother01 = GetComponent<Mother01>();

        if (blaster01 == null)
            if (GetComponent<Blaster01>())
                blaster01 = GetComponent<Blaster01>();

        if (control01 == null)
            if (GetComponent<Control01>())
                control01 = GetComponent<Control01>();

        if (shield01 == null)
            if (GetComponent<Shield01>())
                shield01 = GetComponent<Shield01>();
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
