using System.Collections.Generic;
using UnityEngine;

public class DropView : MonoBehaviour
{
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private List<ParticleSystem> _effects;
    [SerializeField] private Collider _collider;

    public void ChangeState(bool vision)
    {
        if(_collider != null)
            _collider.enabled = vision;

        foreach (Renderer renderer in _renderers)
            renderer.enabled = vision;

        if (vision)
            PlayParticle();
        else
            StopParticle();

    }

    private void PlayParticle()
    {
        foreach (ParticleSystem effect in _effects)
            effect.Play();
    }

    private void StopParticle()
    {
        foreach (ParticleSystem effect in _effects)
            effect.Stop();
    }
}
