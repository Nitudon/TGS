using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UdonCommons;
using ParticlePlayground;

public class ParticleManager : UdonBehaviourSingleton<ParticleManager> {

    [SerializeField]
    private List<Emit_Particle> ParticleEvents;

    public List<Emit_Particle> GetParticleEvents()
    {
        if(ParticleEvents == null)
        {
            ParticleEvents = new List<Emit_Particle>();
        }

        return ParticleEvents;
    }

    public Emit_Particle GetParticleEvent(int layer)
    {
        if(ParticleEvents == null || ParticleEvents.Count-1 < layer)
        {
            InstantLog.StringLogError("invalid access to ParticleEvents");
            return null;
        }
        else
        {
            return ParticleEvents.ElementAt(layer);
        }
    }

    public PlaygroundParticlesC GetParticle(int layer)
    {
        if (ParticleEvents == null || ParticleEvents.Count - 1 < layer)
        {
            InstantLog.StringLogError("invalid access to ParticleEvents");
            return null;
        }
        else
        {
            return ParticleEvents.ElementAt(layer).GetParticle;
        }
    }

}
