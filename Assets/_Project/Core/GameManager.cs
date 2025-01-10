using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameManager : IGameManager
{
    [Inject] public Context Context;

    public UICanvas UICanvas;

    public void Init()
    {
        Context.EventBus.Subscribe(this);
        Context.BulletPoolManager.CreatePools();
        Context.EnemyUIBarManager.CreatePools();
    }
    public async void CloseGame()
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new SimpleOperationWithLoadScreen(() =>
        {
            Context.MainCharactersManager.PlasedPlayerAndTank(Context.LvlManager);
            Context.MainCharactersManager.Tank.VirtualCamera.Priority = Context.MainCharactersManager.Solder.PlayerCameraPriority + 1;
            Context.MainCharactersManager.Solder.InputEnable();
            Context.MainCharactersManager.Tank.ComeOnTheBase();
            Context.LvlManager.CurrentLvl.RestartLvl();
            UICanvas.SetActiveForContinieGameButton(false);
            Context.MainCharactersManager.Tank.NextMovePoint = 0;
        }));

        await Context.Loader.Load(loadingOperations, true);
    }

    public async void LvlEnd()
    {
        Context.MainCharactersManager.Tank.VirtualCamera.Priority = Context.MainCharactersManager.Solder.PlayerCameraPriority + 1;

        await UniTask.WaitForSeconds(Context.CameraBrain.m_DefaultBlend.BlendTime);
        Context.MainCharactersManager.OffsetPlayerAndTank(-Context.LvlManager.CurrentLvl.NextLvlPos.position.z);
        Context.LvlManager.OffsetLvls(-Context.LvlManager.CurrentLvl.NextLvlPos.position.z);
        Physics.SyncTransforms();

        Context.Data.CurrentLvl++;
        if (Context.Data.CurrentLvl > Context.LvlManager.LvlCount - 1)
        {
            Context.Data.CurrentLvl = 0;
        }

        Context.LvlManager.LoadLvl(Context.Data.CurrentLvl, Context.MainCharactersManager.Tank.Health).Forget();

        UICanvas.SetActiveForContinieGameButton(true);
    }

    public void LvlStart()
    {
        Context.MainCharactersManager.Tank.NextMovePoint = 0;
        Context.MainCharactersManager.Tank.VirtualCamera.Priority = Context.MainCharactersManager.Solder.PlayerCameraPriority - 1;

        UICanvas.SetActiveForContinieGameButton(false);
        Context.LvlManager.BuildNavMesh();
        Context.MainCharactersManager.Tank.SetPath(Context.LvlManager.CurrentLvl.GetPath());
        Context.MainCharactersManager.Tank.StartMove();
    }
    public void GameLost()
    {
        Context.MainCharactersManager.Tank.NextMovePoint = 0;
        Context.LvlManager.CurrentLvl.DisableEnemies();
        UICanvas.OpenLoseUI();
        Context.MainCharactersManager.Solder.InputDisable();
        Context.MainCharactersManager.Solder.CalmStateEnable();
    }
}
