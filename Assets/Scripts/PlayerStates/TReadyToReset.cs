using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayerStates
{
    class TReadyToReset<T> : Transition<T>
    {
        public TReadyToReset(T owner) : base(owner)
        {
        }

        public override void EntryAction()
        {
            //base.EntryAction();
        }

        public override void ExitAction()
        {
            //base.ExitAction();
        }

        public override bool IsTriggered()
        {
            return false;
        }
    }
}
