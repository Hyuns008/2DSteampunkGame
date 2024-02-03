using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Monster monsterSc;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        monsterSc = GetComponent<Monster>();
    }

    private void Update()
    {
        monsterAnim();
    }

    private void monsterAnim()
    {
        anim.SetInteger("isWalk", (int)monsterSc.MoveVecReturn().x);
    }
}
