using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Loader 
{
    [Inject] private IAssetProvider _assetProvider;

    public async UniTask Load(Queue<ILoadingOperation> loadingOperations, bool withLoadScreen)
    {

        if (withLoadScreen)
        {
            float progressForOneOperation = 1f / loadingOperations.Count;

            var canvasPrefab = await _assetProvider.Load<GameObject>(AssetsConstants.LoadingScreen);
            var loadingScreen = Object.Instantiate(canvasPrefab).GetComponent<LoadScreen>();

            loadingScreen.OpenLoadScreen();

            foreach (var operation in loadingOperations)
            {
                await operation.Load();
                loadingScreen.AddFillProgress(progressForOneOperation);
            }

            loadingScreen.CloseLoadScreen();

            Object.Destroy(loadingScreen.gameObject);
            _assetProvider.Unload(AssetsConstants.LoadingScreen);
        } else
        {
            foreach (var operation in loadingOperations)
            {
                await operation.Load();
            }
        }
    }
}
