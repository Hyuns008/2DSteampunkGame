using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconRobotTarget : MonoBehaviour
{
    private SpriteRenderer sprRen;
    private Color color;

    private void Awake()
    {
        sprRen = GetComponent<SpriteRenderer>();

        color = sprRen.color;
    }

    private void Update()
    {
        color.a -= Time.deltaTime / 2;
        sprRen.color = color;
        Vector3 myScale = transform.localScale;
        myScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime / 2;
        transform.localScale = myScale;
        Destroy(gameObject, 2f);
    }
}
