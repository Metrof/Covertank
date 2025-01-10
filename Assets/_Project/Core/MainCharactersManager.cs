using UnityEngine;

public class MainCharactersManager 
{
    public Player Solder;
    public Tank Tank;

    public void Initialization(Player player, Tank agent)
    {
        Solder = player; 
        Tank = agent;

        Tank.VirtualCamera.Priority = Solder.PlayerCameraPriority + 1;
    }
    public void OffsetPlayerAndTank(float offset)
    {
        Vector3 cPos = Solder.transform.position;
        cPos.z += offset;
        Solder.transform.position = cPos;

        Vector3 tPos = Tank.transform.position;
        tPos.z += offset;
        Tank.transform.position = tPos;
    }
    public void PlasedPlayerAndTank(LvlManager lvlManager)
    {
        Solder.transform.position = lvlManager.CurrentLvl.PlayerSP.position;
        Tank.transform.position = lvlManager.CurrentLvl.BasePoint.position;
    }
    public void EnablePlayerAndTank()
    {
        Solder.gameObject.SetActive(true);
        Tank.gameObject.SetActive(true);

        Solder.InputEnable();
    }
    public void DisablePlayerAndAgent()
    {
        Solder.InputDisable();

        Solder.transform.position = Vector3.zero;
        Tank.transform.position = Vector3.zero;

        Solder.gameObject.SetActive(false);
        Tank.gameObject.SetActive(false);
    }
}
