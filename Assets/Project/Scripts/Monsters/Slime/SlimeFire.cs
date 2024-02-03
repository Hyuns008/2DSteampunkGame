using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFire : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;

    [Header("화염방사 공격력")]
    [SerializeField] private float damage;

    private void onTriggerCheck(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player playerSc = collision.gameObject.GetComponent<Player>();
            playerSc.PlayerHit(damage, true, true);
        }
    }

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        playerCollCheck();
    }

    private void playerCollCheck()
    {
        Collider2D playerColl = Physics2D.OverlapBox(boxCollider2D.bounds.center,
                     boxCollider2D.bounds.size, 0, LayerMask.GetMask("Player"));

        if (playerColl != null)
        {
            onTriggerCheck(playerColl);
        }
    }

    public void AttackEnd()
    {
        Destroy(gameObject);
    }
}
