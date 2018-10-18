using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShipStates
{
    class TReadyToReset<T> : Transition<T>
    {
		Animator animRef;
        public TReadyToReset(T owner) : base(owner)
        {
			MonoBehaviour reference = (MonoBehaviour)(object)Owner;
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
