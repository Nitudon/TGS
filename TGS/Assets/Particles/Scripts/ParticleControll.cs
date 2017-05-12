using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticlePlayground;
using UniRx;

public class ParticleControll : MonoBehaviour {

    [SerializeField] List<TimeUnit_Particle> timeUnitEvents;
    [SerializeField] List<Emit_Particle> emitEvents;
    private void ParticleSet()
    {
        for (int i = 0;i < timeUnitEvents.Count; ++i)
        {
            timeUnitEvents[i].SetEnable(false);
            timeUnitEvents[i].SetParticle();
        }

        for (int i = 0;i<emitEvents.Count;++i)
        {
            emitEvents[i].SetParticle();
        }

    }

	// Use this for initialization
	private void Awake () {
        ParticleSet();
	}
	
}
