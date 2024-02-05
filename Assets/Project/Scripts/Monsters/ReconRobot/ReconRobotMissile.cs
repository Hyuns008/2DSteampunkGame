using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconRobotMissile : MonoBehaviour
{
    [Header("미사일 설정")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player playerSc = collision.GetComponent<Player>();
            playerSc.PlayerHit(damage, true, false);
        }
    }

    private void Update()
    {
        transform.position += transform.rotation * Vector3.left * moveSpeed * Time.deltaTime;
        Destroy(gameObject, 4f);
    }
}
