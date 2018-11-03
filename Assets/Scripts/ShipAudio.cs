using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAudio : MonoBehaviour
{
    AudioSource BodySource;
    AudioSource GunSource;
    public AudioClip ExplosionClip;
    public AudioClip FireClip;
    void Start ()
    {
        BodySource = gameObject.AddComponent<AudioSource>();
        GunSource = gameObject.AddComponent<AudioSource>();
        BodySource.loop = false;
        BodySource.playOnAwake = false;
        BodySource.clip = ExplosionClip;
        GunSource.loop = false;
        GunSource.playOnAwake = false;
        GunSource.clip = FireClip;
    }

    void Update ()
    {
		
	}
    public void PlayFireSound()
    {
        GunSource.Play();
    }
    public void PlayExplosionSound()
    {
        BodySource.Play();
    }
}
