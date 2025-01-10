using Cinemachine;
using VContainer;

public class Context 
{
    [Inject] public EventBus EventBus;
    [Inject] public IAssetProvider AssetProvider;
    [Inject] public Loader Loader;
    [Inject] public IDataLoader DataLoader;
    [Inject] public LvlManager LvlManager;
    [Inject] public BulletPoolManager BulletPoolManager;
    [Inject] public EnemyUIBarPoolManager EnemyUIBarManager;
    [Inject] public MainCharactersManager MainCharactersManager;
    [Inject] public CinemachineBrain CameraBrain;

    public Data Data { get; set; }
}
