using UnityEngine;

namespace PlayerStates
{
    class SPlayerControlling<T> : State<T>
    {
        PlayerShip player;

        public SPlayerControlling(T Owner) : base(Owner)
        {
            player = (PlayerShip)(object)Owner;
        }

        public override void Action()
        {
            if (player.lerpStartY != player.lerpTargetY)
            {
                player.Move();
            }

            if (player.keyboardMode)
            {
                bool moved = false;
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    player.lerpTargetY = player.yStart + 2 * player.offset;
                    moved = true;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    player.lerpTargetY = player.yStart + -2 * player.offset;
                    moved = true;
                }
                if (moved)
                {
                    player.lerpStartY = player.transform.position.y;
                    player.lerpT = 0;
                }
            }
            else
            {
                string fire = player.device.Read();
                if (fire == null)
                {
                    return;
                }
                char[] delim = { '|' };
                string[] a = fire.Split(delim);
                if (a[0] == "fire")
                {
                    player.Fire();
                }
                if (a[1] != "!" && a[1] != null)
                {
                    float cm = float.Parse(a[1]) / 10.0f;
                    if (Mathf.Abs(player.lastPos - cm) >= player.sensitivity)
                    {
                        player.lerpTargetY = player.yStart + (cm / 10.0f - 3.0f) * player.offset;
                        player.lerpStartY = player.transform.position.y;
                        player.lerpT = 0;
                        player.lastPos = cm;
                    }
                }
            }
        }

        public override void EntryAction()
        {
            Debug.Log("entrou no controlling");
        }


        public override void ExitAction()
        {
            base.ExitAction();
        }

        public override string GetStateName()
        {
            return "Controlling";
        }
    }
}
