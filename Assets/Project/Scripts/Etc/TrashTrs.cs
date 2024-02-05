using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTrs : MonoBehaviour
{
    public static TrashTrs instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
