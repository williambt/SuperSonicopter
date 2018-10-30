using UnityEngine;

namespace ShipStates
{
	public class SEnemyControlling : State<EnemyShip>
	{
		public SEnemyControlling(EnemyShip Owner) : base(Owner)
		{
			
		}
		public override void Action()
		{
            Owner.RigidbodyRef.AddForce(new Vector2(-1, 0));
		}
		public override void EntryAction()
		{
			Debug.Log("entrou no controlling inimigo");
            Owner.RigidbodyRef.AddTorque(-0.1f, ForceMode2D.Impulse);
		}


		public override void ExitAction()
		{
			//base.ExitAction();
		}

		public override string GetStateName()
		{
			return "EnemyControlling";
		}
	}
}

