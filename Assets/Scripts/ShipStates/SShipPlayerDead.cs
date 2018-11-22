using UnityEngine;

namespace ShipStates
{
	class SShipPlayerDead : State<PlayerShip>
	{
		SpriteRenderer spriteRef;
		public SShipPlayerDead(PlayerShip Owner) : base(Owner)
		{
			MonoBehaviour ship = (MonoBehaviour)(object)Owner;
			spriteRef = ship.gameObject.GetComponent<SpriteRenderer> ();
		}

		public override void Action()
		{
			//base.Action();
		}

		public override void EntryAction()
		{
			spriteRef.enabled = false;
			Owner.GetComponent<PolygonCollider2D> ().enabled = false;
		}

		public override void ExitAction()
		{
			//base.ExitAction();
		}

		public override string GetStateName()
		{
			return "ShipPlayerDead";
		}

	}
}
