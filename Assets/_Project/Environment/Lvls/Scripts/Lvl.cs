using System.Collections.Generic;
using UnityEngine;

public class Lvl : MonoBehaviour
{
    public Transform PlayerSP;
    public Transform NextLvlPos;
    public Transform BasePoint;

    private LevelPathCreator _levelPathCreator;

    public List<LvlWave> Waves;

    private void Awake()
    {
        _levelPathCreator = GetComponent<LevelPathCreator>();
    }
    public void Initialization(Context context, IDamageTaker target)
    {
        foreach (var wave in Waves)
        {
            foreach (var enemy in wave.Enemies)
            {
                enemy.Initialization(context, target);
            }
        }
    }
    public void DisableEnemies()
    {
        foreach (var wave in Waves)
        {
            wave.CalmAllEnemies();
        }
    }
    public void RestartLvl()
    {
        foreach (var wave in Waves)
        {
            wave.Reactivate();
        }
    }
    public List<Vector3> GetPath()
    {
        return _levelPathCreator.GetPath();
    }
    public void CreatePath(Vector3 lastPoint)
    {
        _levelPathCreator.CreatePath(lastPoint);
    }
}
