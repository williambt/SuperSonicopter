using UnityEngine;

namespace ShipStates
{
    class SShipDead<T> : State<T>
    {
		SpriteRenderer spriteRef;
        public SShipDead(T Owner) : base(Owner)
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
			Debug.Log("entrou no Dead");
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
