using System;
using UnityEngine;

[Serializable]
public struct MatchingMaterialEffect
{
    [SerializeField] private Material _material;
    [SerializeField] private HitEffect _hitEffect;

    public Texture Texture => _material?.mainTexture;
    public HitEffect HitEffect => _hitEffect;
}