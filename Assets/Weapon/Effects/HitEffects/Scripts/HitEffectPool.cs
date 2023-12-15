using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class HitEffectPool : MonoBehaviour
{
    [SerializeField] private int _countPreLoad = 10;
    [SerializeField] private List<MatchingMaterialEffect> _preload;
    [SerializeField] private HitEffect _baseEffect;

    private Dictionary<Texture, List<HitEffect>> _hitEffects;
    private List<HitEffect> _baseEffects;
    private MaterialGiver _materialGiver;

    private void Awake()
    {
        _materialGiver = new MaterialGiver();
    }

    public void InstantiatePool()
    {
        if(_hitEffects != null)
            return;

        _baseEffects = CreatePoolForEffect(_baseEffect);
        _hitEffects = new Dictionary<Texture, List<HitEffect>>();

        foreach (MatchingMaterialEffect effect in _preload)
        {
            if(_hitEffects.ContainsKey(effect.Texture) == false)
                _hitEffects.Add(effect.Texture, CreatePoolForEffect(effect.HitEffect));
        }
    }

    public void Show(RaycastHit hitInfo)
    {
        List<Material> materials = _materialGiver.GetMaterials(hitInfo);

        var selectedEffects = from effect in _hitEffects join material in materials on effect.Key equals material.mainTexture select effect.Value;

        if(selectedEffects.Count() > 0 )
        {
            ShowPull(selectedEffects.First(), hitInfo);
        }
        else
        {
            ShowPull(_baseEffects, hitInfo);
        }
    }

    private void ShowPull(List<HitEffect> current, RaycastHit hitInfo)
    {
        var freeEffects = current.Where(effect => effect.IsFree);

        if (freeEffects.Count() != 0)
        {
            HitEffect temp = freeEffects.First();
            temp.Show(hitInfo.point, hitInfo.normal, hitInfo.collider.transform);
            current.Remove(temp);
            current.Add(temp);
        }
        else
        {
            AddNewEffectToPool(current, current.First()).Show(hitInfo.point, hitInfo.normal, hitInfo.collider.transform);
        }
    }

    private HitEffect AddNewEffectToPool(List<HitEffect> tempList, HitEffect prefab)
    {
        HitEffect hitEffect = Instantiate(prefab.gameObject, transform).GetComponent<HitEffect>();
        tempList.Add(hitEffect);
        return hitEffect;
    }

    private List<HitEffect> CreatePoolForEffect(HitEffect prefab)
    {
        List<HitEffect> tempList = new List<HitEffect>();

        for (int i = 0; i < _countPreLoad; i++)
            AddNewEffectToPool(tempList, prefab);

        return tempList;
    }
}
