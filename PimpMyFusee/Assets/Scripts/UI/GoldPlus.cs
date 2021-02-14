using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldPlus : MonoBehaviour
{
    float basePosition = 0;
    [SerializeField] float maxPos = 5;
    [SerializeField] float speed = 5;
    [SerializeField] public TextMeshProUGUI amountText = null;

    // Start is called before the first frame update
    void Start()
    {
        basePosition = transform.position.y;
    }



    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + Time.fixedDeltaTime * speed);

        if (transform.position.y >= basePosition + maxPos)
        {
            transform.position = new Vector2(transform.position.x, basePosition);
            gameObject.SetActive(false);
        }
    }
}
