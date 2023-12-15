using System.Collections.Generic;
using UnityEngine;

public class ShotEffect : MonoBehaviour
{
    private readonly int PlayHash = Animator.StringToHash("Play");

    [SerializeField] private List<ParticleSystem> _effects;
    [SerializeField] private List<Animator> _animations;

    public void PlayEffect()
    {
        foreach (ParticleSystem effect in _effects)
        {
            effect.Stop();
            effect.Play();
        }

        foreach (Animator animation in _animations)
        {
            animation.SetTrigger(PlayHash);
        }
    }
}
