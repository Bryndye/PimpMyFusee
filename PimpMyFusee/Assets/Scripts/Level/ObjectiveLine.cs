using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveLine : MonoBehaviour
{
    [SerializeField] Transform objectToFollowX = null;
    [SerializeField] TextMeshPro text = null;

    [SerializeField] private int GoldBonus;

    // Start is called before the first frame update
    private void Awake()                                                            // AWAKE
    {
        GetReferences();
        if (text != null)
            text.text = Mathf.RoundToInt(transform.position.y).ToString() + "m";
    }

    // Update is called once per frame
    void FixedUpdate()                                                              // FIXED UPDATE
    {
        if (objectToFollowX != null)
            transform.position = new Vector2(objectToFollowX.transform.position.x, transform.position.y);
    }



    void GetReferences()
    {
        if (objectToFollowX == null)
            objectToFollowX = FindObjectOfType<Module>().gameObject.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Mother01>() != null)
        {
            Debug.Log("Bonus");
            ManagerShop.Instance.AddGold(GoldBonus, transform.position.y);
        }
    }
}
