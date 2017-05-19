using System;
using System.Collections;
using System.Collections.Generic;
using ParticlePlayground;
using System.Linq;
using UniRx;
using UnityEngine;

[System.Serializable]
public abstract class ParticleEvent {
    [SerializeField]
    protected PlaygroundParticlesC particle;

    #region[Property]
    public PlaygroundParticlesC GetParticle { get { return particle; } }
    public bool emit { get { return particle.emit; } }
    public Transform transform { get { return particle.gameObject.transform; } }
    public bool active { get { return particle.gameObject.activeSelf; } }
    public virtual void SetActive(bool value) { particle.gameObject.SetActive(value); }
    public virtual void SetEnable(bool value) { particle.emit = value; }
    public virtual Type GetComponent<Type>() { return particle.gameObject.GetComponent<Type>(); }
    #endregion

    public abstract void SetParticle();

    protected virtual void Emit()
    {
        particle.Emit();
    }

}

[System.Serializable]
public class TimeUnit_Particle : ParticleEvent
{
    public enum type { enable, disable, both}
 
    #region[member]
    public type eventType;
    [SerializeField] private float _start = 0f;
    [SerializeField] private float _end = 0f;
    #endregion

    public override void SetParticle()
    {
        switch (eventType)
        {
            case type.enable:
             ParticleStart(_start);
                break;
                
            case type.disable:
             ParticleEnd(_end);
                break;

            case type.both:
             ParticleStartToEnd(_start, _end);
                break;
    
            default:
             Debug.LogError("イベントタイプが指定されていません");
                break;
        }
    }

    public void ParticleStart(float time)
    {
        Observable.Timer(TimeSpan.FromSeconds(time))
            .Subscribe(_ => SetEnable(true));
    }

    public void ParticleStartToEnd(float start, float end)
    {
        Observable.Timer(TimeSpan.FromSeconds(start))
            .Subscribe(_ => SetEnable(true), () => ParticleEnd(end));
    }

    public void ParticleEnd(float time)
    {
        Observable.Timer(TimeSpan.FromSeconds(time))
            .Subscribe(_ => SetEnable(false));
    }
}

[System.Serializable]
public class Emit_Particle : ParticleEvent
{
    #region[member]
    [SerializeField]
    private float time = 0f;    
    [SerializeField]
    private float giveLifeTime = 0f;
    [SerializeField]
    private AnimationCurve lifetimeSize = AnimationCurve.Linear(0,0,1f,1f);
    [SerializeField]
    private int ParticleNum;
    [SerializeField]
    private Vector3[] positions;

    private float _giveLifeTime {get { return (giveLifeTime == 0) ? particle.lifetime : giveLifeTime; } }

    public override void SetParticle()
    {
        ParticleEmitter();
    }

    public void NumEmit()
    {
        for(int i=0;i < ParticleNum; ++i)
        particle.Emit();
    }

    public void NumEmit(Vector3 pos)
    {
        for (int i = 0; i < ParticleNum; ++i)
            particle.Emit(pos);
    }

    protected override void Emit()
    {
        for(int i=0;i < positions.Length;++i)
        {
            particle.lifetimeSize = lifetimeSize;
            particle.Emit(positions[i],_giveLifeTime);
        }
    }

    private void ParticleEmitter()
    {
        Observable.Timer(TimeSpan.FromSeconds(time))
            .Subscribe(_ => Emit());
    }

    #endregion
}
