using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorRouter : MonoBehaviour
{
    private Dictionary<AnimationType, int> _animations = new Dictionary<AnimationType, int>();

    [SerializeField] private Animator _main;

    private void Awake()
    {
        foreach(AnimationType animationType in Enum.GetValues(typeof(AnimationType)))
            _animations.Add(animationType, Animator.StringToHash(animationType.ToString()));
    }

    public void Play(AnimationType animationType) => _main.SetTrigger(_animations[animationType]);

    public void SetBool(AnimationType animationType, bool value) => _main.SetBool(_animations[animationType], value);
}
