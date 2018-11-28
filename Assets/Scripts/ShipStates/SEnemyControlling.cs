using UnityEngine;

namespace ShipStates
{
	public class SEnemyControlling : State<EnemyShip>
	{
        MovementType movement;

		public SEnemyControlling(EnemyShip Owner) : base(Owner)
		{
            movement = Owner.gameObject.GetComponent<MovementType>();
		}
		public override void Action()
		{
            movement.Move(Owner.RigidbodyRef);
        }
		public override void EntryAction()
		{

        }
        public override void ExitAction()
		{
		}

		public override string GetStateName()
		{
			return "EnemyControlling";
		}
	}
}

