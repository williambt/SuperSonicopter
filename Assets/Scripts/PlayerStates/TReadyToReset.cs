using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerStates
{
    class TReadyToReset<T> : Transition<T>
    {
		Animator animRef;
        public TReadyToReset(T owner) : base(owner)
        {
			PlayerShip reference = (PlayerShip)(object)Owner;
			animRef = reference.gameObject.GetComponent<Animator> ();
        }

        public override void EntryAction()
        {
            //base.EntryAction();
        }

        public override void ExitAction()
        {
            //base.ExitAction();
        }

        public override bool IsTriggered()
        {
			return animRef.GetCurrentAnimatorStateInfo(0).IsName("Dead");
        }
    }
}
