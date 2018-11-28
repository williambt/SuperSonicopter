using UnityEngine;

namespace ShipStates
{
    class SShipDead<T> : State<T>
    {
		SpriteRenderer spriteRef;
        public SShipDead(T Owner) : base(Owner)
        {
        }

        public override void Action()
        {
            //base.Action();
        }

        public override void EntryAction()
        {
            MonoBehaviour ship = (MonoBehaviour)(object)Owner;
            spriteRef = ship.gameObject.GetComponent<SpriteRenderer>();
            spriteRef.gameObject.SetActive(false);
        }

        public override void ExitAction()
        {
            //base.ExitAction();
        }

        public override string GetStateName()
        {
            return "ShipDead";
        }

    }
}
