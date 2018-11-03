using UnityEngine;

namespace ShipStates
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
            IShip ship = Owner as IShip;
            if (ship != null)
            {
                ship.Explode();
                Debug.Log("entrou com sucesso no exploding");
            }
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
