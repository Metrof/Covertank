using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using VContainer;
using VContainer.Unity;

public class AppStart : IAsyncStartable
{
    [Inject] private GameManager _gameManager;
    [Inject] private MusicSource _musicSource;
    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new LoadDataFromServerOperation(_gameManager.Context));
        loadingOperations.Enqueue(new CreateUIOperation(_gameManager));
        loadingOperations.Enqueue(new LoadCharactersOperation(_gameManager));
        loadingOperations.Enqueue(new LoadLvlOperation(_gameManager.Context));
        loadingOperations.Enqueue(new SimpleOperationWithLoadScreen(() =>
        {
            _gameManager.Context.MainCharactersManager.EnablePlayerAndTank();
            _gameManager.Context.MainCharactersManager.PlasedPlayerAndTank(_gameManager.Context.LvlManager);
        }));

        _gameManager.Init();
        _musicSource.Init(_gameManager.Context.EventBus);
        _gameManager.Context.LvlManager.Init(_gameManager.Context);

        await _gameManager.Context.Loader.Load(loadingOperations, true);
    }
}
