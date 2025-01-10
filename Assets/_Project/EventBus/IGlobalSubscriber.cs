public interface IGlobalSubscriber { }

public interface IMusicChanger : IGlobalSubscriber
{
    void ChangeSoundVolume(float volume);
}
public interface IGameManager : IGlobalSubscriber
{
    void CloseGame();
    void LvlEnd();
    void LvlStart();
    void GameLost();
}
public interface ICanvas : IGlobalSubscriber
{
    void TankHPChage(float hp, float maxHP);
}
