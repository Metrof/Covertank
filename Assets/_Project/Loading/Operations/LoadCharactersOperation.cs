using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoadCharactersOperation : ILoadingOperation
{
    private GameManager _gameManager;

    public LoadCharactersOperation(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    public async UniTask Load()
    {
        var playerRef = await _gameManager.Context.AssetProvider.Load<GameObject>(AssetsConstants.PlayerName);
        var player = Object.Instantiate(playerRef).GetComponent<Player>();

        var tankRef = await _gameManager.Context.AssetProvider.Load<GameObject>(AssetsConstants.AgentName);
        var tank = Object.Instantiate(tankRef).GetComponent<Tank>();

        tank.Initialization(_gameManager.Context);
        player.Initialization(_gameManager.Context);
        _gameManager.Context.MainCharactersManager.Initialization(player, tank);
    }
}
