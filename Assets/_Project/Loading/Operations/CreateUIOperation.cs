using Cysharp.Threading.Tasks;
using UnityEngine;

public class CreateUIOperation : ILoadingOperation
{
    private GameManager _gameManager;

    public CreateUIOperation(GameManager manager)
    {
        _gameManager = manager;
    }
    public async UniTask Load()
    {
        var canvasPrefab = await _gameManager.Context.AssetProvider.Load<GameObject>(AssetsConstants.MainCanvas);
        var canvas = Object.Instantiate(canvasPrefab).GetComponent<UICanvas>();
        canvas.Initialize(_gameManager.Context.EventBus);

        _gameManager.UICanvas = canvas;
    }
}
