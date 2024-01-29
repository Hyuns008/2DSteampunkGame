using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject mapObj;
    [SerializeField] private Vector3 playerPos;

    private void Update()
    {
        playerMapCheck();
    }

    private void playerMapCheck()
    {
        mapObj.transform.position = (playerObj.transform.position * 50) + playerPos;
    }
}
