using UnityEngine;

namespace PlayerStates
{
    class TLevelStart<T> : Transition<T>
    {
        public TLevelStart(T owner) : base(owner)
        {
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
            return LevelManager.Instance.Begin;
        }
    }
}
