using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    public PlayerShip player;

    Image sprite;

	// Use this for initialization
	void Start ()
    {
        sprite = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        sprite.fillAmount = player.HP / player.MaxHP;
        if (sprite.fillAmount > 0.6f)
            sprite.color = Color.green;
        else if (sprite.fillAmount > 0.33f)
            sprite.color = Color.yellow;
        else
            sprite.color = Color.red;
	}
}
