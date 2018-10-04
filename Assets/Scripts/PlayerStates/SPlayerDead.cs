using UnityEngine;

namespace PlayerStates
{
    class SPlayerDead<T> : State<T>
    {
        public SPlayerDead(T Owner) : base(Owner)
        {
        }

        public override void Action()
        {
            //base.Action();
        }

        public override void EntryAction()
        {
            //base.EntryAction();
        }

        public override void ExitAction()
        {
            //base.ExitAction();
        }

        public override string GetStateName()
        {
            return "PlayerDead";
        }

    }
}
