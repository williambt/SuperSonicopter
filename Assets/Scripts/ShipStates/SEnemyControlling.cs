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
		}
		public override void EntryAction()
		{
			Debug.Log("entrou no controlling");
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

