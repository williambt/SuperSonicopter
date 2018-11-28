using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHUD : StateMachineBehaviour
{

    public GameObject HUD;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.GetChild(1).GetComponent<Animator>().SetBool("Show", true);
    }
}
