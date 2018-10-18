using UnityEngine;

namespace PlayerStates
{
    class SShipDead<T> : State<T>
    {
		SpriteRenderer spriteRef;
        public SShipDead(T Owner) : base(Owner)
        {
			PlayerShip ship = (PlayerShip)(object)Owner;
			spriteRef = ship.gameObject.GetComponent<SpriteRenderer> ();
        }

        public override void Action()
        {
            //base.Action();
        }

        public override void EntryAction()
        {
			Debug.Log("entrou no Dead");
			spriteRef.enabled = false;
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
