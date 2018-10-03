﻿using UnityEngine;
namespace PlayerStates
{
    public class SPlayerBegin<T> : State<T>
    {
        public SPlayerBegin(T Owner) : base(Owner)
        {

        }

        public override void Action()
        {
            
        }

        public override void EntryAction()
        {
            //base.EntryAction();
            Debug.Log("entrou no estado inicial");
        }
        public override void ExitAction()
        {
            //base.ExitAction();
        }
        public override string GetStateName()
        {
            return "Begin";
        }
    }
}