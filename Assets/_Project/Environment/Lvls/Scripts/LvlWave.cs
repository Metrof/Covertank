using System.Collections.Generic;
using UnityEngine;

public class LvlWave : MonoBehaviour
{
    [SerializeField] private LvlWaveTrigger _lvlWaveTrigger;

    public List<Enemy> Enemies;

    private void Start()
    {
        _lvlWaveTrigger.OnTankEnter += ActivateEnemies;
    }
    private void OnDestroy()
    {
        _lvlWaveTrigger.OnTankEnter -= ActivateEnemies;
    }
    public void CalmAllEnemies()
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.CalmState();
        }
    }
    public void Reactivate()
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.PlaceOnDefaultPosition();
        }
        _lvlWaveTrigger.gameObject.SetActive(true);
    }
    private void ActivateEnemies()
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.SearchState();
        }
        _lvlWaveTrigger.gameObject.SetActive(false);
    }
}
