using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XBL.Core.Tools;

public class ParicleTest : MonoBehaviour {
    public float ParticleScale;
	// Use this for initialization
	void Start () {
        ScaleParticleSystem(ParticleScale);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ScaleParticleSystem(float scale)
    {
        ParticleTools particle = gameObject.GetComponent<ParticleTools>();
        if (particle == null)
        {
            particle = gameObject.AddComponent<ParticleTools>();
        }
        particle.ScaleParticleSystem(ParticleScale);
    }
}
