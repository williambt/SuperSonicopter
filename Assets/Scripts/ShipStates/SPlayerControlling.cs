﻿using UnityEngine;

namespace ShipStates
{
    class SPlayerControlling<T> : State<T>
    {
        PlayerShip player;
        Rigidbody2D rigidbody;


        public SPlayerControlling(T Owner) : base(Owner)
        {
            player = (PlayerShip)(object)Owner;
        }
        float Clock = 0;
        float ShootInterval = 0.1f;
        public override void Action()
        {
            if (player.lerpStartY != player.lerpTargetY)
            {
                player.Move();
            }

            if (ArduinoInput.Instance.KeyboardMode)
            {
                bool moved = false;
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    player.lerpTargetY = player.transform.position.y + 1 ;
                    moved = true;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    player.lerpTargetY = player.transform.position.y - 1 ;
                    moved = true;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    player.Fire();
                }
                if (moved)
                {
                    player.lerpStartY = player.transform.position.y;
                    player.lerpT = 0;
                }
            }
            else
            {
               
                if (ArduinoInput.GetFire())
                {
                    Clock += Time.deltaTime;
                    if (Clock >= ShootInterval)
                    {
                        Clock = 0;
                        player.Fire();
                    }
                }

                float cm = ArduinoInput.GetUSensor();// / 10.0f;
                if (Mathf.Abs(player.lastPos - cm) >= player.sensitivity)
                {
                    player.lerpTargetY = Mathf.Lerp(player.LowerLimit, player.UpperLimit, cm / player.MaxSensorValue);
                    player.lerpStartY = player.transform.position.y;
                    player.lerpT = 0;
                    player.lastPos = cm;
                }
                
            }
        }

        public override void EntryAction()
        {
//            Debug.Log("entrou no controlling");
        }


        public override void ExitAction()
        {
            //base.ExitAction();
        }

        public override string GetStateName()
        {
            return "PlayerControlling";
        }
    }
}
