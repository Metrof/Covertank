using VContainer;
using VContainer.Unity;
using UnityEngine;
using Cinemachine;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private MusicSource musicSource;
    [SerializeField] private LvlManager lvlManager;
    [SerializeField] private BulletPoolManager bulletPoolManager;
    [SerializeField] private EnemyUIBarPoolManager hpBarPoolManager;
    [SerializeField] private CinemachineBrain cameraBrain;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<EventBus>(Lifetime.Singleton).AsSelf();
        builder.Register<AssetProviderService>(Lifetime.Singleton).As<IAssetProvider>();
        builder.Register<Loader>(Lifetime.Singleton).AsSelf();
        builder.Register<DataLoader>(Lifetime.Singleton).As<IDataLoader>();
        builder.Register<MainCharactersManager>(Lifetime.Singleton).AsSelf();

        builder.RegisterComponent(musicSource).AsSelf();
        builder.RegisterComponent(lvlManager).AsSelf();
        builder.RegisterComponent(bulletPoolManager).AsSelf();
        builder.RegisterComponent(hpBarPoolManager).AsSelf();
        builder.RegisterComponent(cameraBrain).AsSelf();

        builder.Register<Context>(Lifetime.Singleton).AsSelf();

        builder.Register<GameManager>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<AppStart>(Lifetime.Singleton).AsSelf();
    }
}
