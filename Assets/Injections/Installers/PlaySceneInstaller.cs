using UnityEngine;
using Zenject;

public class PlaySceneInstaller : MonoInstaller
{
    [SerializeField] private Movement _movement;
    [SerializeField] private PlayerHand _hand;
    [SerializeField] private ScannerInteractable _scanner;
    [SerializeField] private CameraShaker _shaker;
    [SerializeField] private InteractableViewer _viewer;
    [SerializeField] private HitEffectPool _hitEffectPool;
    [SerializeField] private TemporaryInfo _tempInfo;
    [SerializeField] private FlyZone _flyZone;
    [SerializeField] private FlyFactory _flyFactory;

    public override void InstallBindings()
    {
        Bind();
        Instantiate();
        Inject();
    }

    private void Instantiate()
    {
        _hitEffectPool.InstantiatePool();
    }

    private void Inject()
    {
        Container.InjectGameObject(_movement.gameObject);
        Container.InjectGameObject(_viewer.gameObject);
        Container.InjectGameObject(_tempInfo.gameObject);
        Container.InjectGameObject(_flyFactory.gameObject);
    }

    private void Bind()
    {
        PlayerInputMap instance = Container.Instantiate<PlayerInputMap>();
        instance.Enable();

        Container.Bind<PlayerInputMap>().FromInstance(instance).AsSingle().NonLazy();
        Container.Bind<CameraShaker>().FromInstance(_shaker).AsSingle();
        Container.Bind<ScannerInteractable>().FromInstance(_scanner).AsSingle();
        Container.Bind<HitEffectPool>().FromInstance(_hitEffectPool).AsSingle().NonLazy();
        Container.Bind<PlayerHand>().FromInstance(_hand).AsSingle().NonLazy();
        Container.Bind<FlyZone>().FromInstance(_flyZone).AsSingle().NonLazy();
    }
}