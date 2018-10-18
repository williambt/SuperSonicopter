using UnityEngine;

namespace PlayerStates
{
    class SShipExploding<T> : State<T>
    {
        public SShipExploding(T Owner) : base(Owner)
        {

        }

        public override void Action()
        {
            //base.Action();
        }

        public override void EntryAction()
        {
            Debug.Log("entrou no exploding");
        }


        public override void ExitAction()
        {
            //base.ExitAction();
        }


        public override string GetStateName()
        {
            return "ShipExploding";
        }


    }
}
