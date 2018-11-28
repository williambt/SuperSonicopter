using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationEnterBar : MonoBehaviour
{

    Animator titleAnimator = null;
    Image image = null;

	// Use this for initialization
	void Start ()
    {
        titleAnimator = transform.parent.parent.GetComponent<Animator>();
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        image.fillAmount = titleAnimator.GetFloat("EnterCalibrationBar");
	}
}
