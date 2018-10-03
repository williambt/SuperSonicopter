using UnityEngine;

namespace PlayerStates
{
    class TIsDead<T> : Transition<T>
    {
        public TIsDead(T owner) : base(owner)
        {
        }
        public override void EntryAction()
        {
            base.EntryAction();
        }

        public override void ExitAction()
        {
            base.ExitAction();
        }

        public override bool IsTriggered()
        {
            return false;
        }
    }
   
}
