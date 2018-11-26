using UnityEngine;
namespace ShipStates
{
    public class SShipBegin<T> : State<T>
    {
        public SShipBegin(T Owner) : base(Owner)
        {

        }

        public override void Action()
        {
            
        }

        public override void EntryAction()
        {
        }
        public override void ExitAction()
        {
        }
        public override string GetStateName()
        {
            return "ShipBegin";
        }
    }
}
