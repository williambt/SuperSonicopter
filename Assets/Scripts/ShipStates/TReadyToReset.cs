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
            UpdateAnimRef();
        }

        public override void EntryAction()
        {
            
        }

        public override void ExitAction()
        {
            //base.ExitAction();
        }

        public override bool IsTriggered()
        {
            if (animRef == null)
            {
                UpdateAnimRef();
            }
			return animRef.GetCurrentAnimatorStateInfo(0).IsName("Dead");
        }
        void UpdateAnimRef()
        {
            MonoBehaviour reference = (MonoBehaviour)(object)Owner;
            animRef = reference.gameObject.GetComponent<Animator>();
        }
    }
}
