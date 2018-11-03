using UnityEngine;

namespace ShipStates
{
    class TIsDead<T> : Transition<T>
    {
        IShip shipRef;
        public TIsDead(T owner) : base(owner)
        {
            shipRef = (IShip)Owner;
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
            return shipRef.IsDead();
        }
    }
   
}
