using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LvlManager : MonoBehaviour
{
    [SerializeField] private List<AssetReferenceT<GameObject>> _lvlReferences = new();
    [SerializeField] private NavMeshSurface _meshSurface;

    private Context _contex;

    private AssetReference _nextLoadLvl;
    private AssetReference _currentLoadLvl;

    public int LvlCount { get { return _lvlReferences.Count; } }

    [HideInInspector] public Lvl CurrentLvl;
    private Lvl _nextLvl;

    private AssetReferenceT<GameObject> GetLvlReference(int lvlNum)
    {
        if (lvlNum > _lvlReferences.Count - 1)
        {
            return _lvlReferences[0];
        } else if (lvlNum < 0)
        {
            return _lvlReferences[_lvlReferences.Count - 1];
        } else
        {
            return _lvlReferences[lvlNum];
        }
    }

    public void Init(Context context)
    {
        _contex = context;
    }
    public void BuildNavMesh()
    {
        _meshSurface.BuildNavMesh();
    }
    public void OffsetLvls(float offset)
    {
        Vector3 cPos = CurrentLvl.transform.position;
        cPos.z += offset;
        CurrentLvl.transform.position = cPos;

        Vector3 nPos = _nextLvl.transform.position;
        nPos.z += offset;
        _nextLvl.transform.position = nPos;
    }
    public async UniTask LoadLvl(int lvlNum, IDamageTaker tankT)
    {
        AssetReferenceT<GameObject> nextReference = GetLvlReference(lvlNum + 1);

        Vector3 spawnPos = Vector3.zero;

        if (CurrentLvl == null)
        {
            AssetReferenceT<GameObject> reference = GetLvlReference(lvlNum);
            var lvl = await _contex.AssetProvider.Load(reference);
            CurrentLvl = Instantiate(lvl, spawnPos, Quaternion.identity).GetComponent<Lvl>();
            _currentLoadLvl = reference;
        } else
        {
            UnloadLvl(false, _currentLoadLvl);
            Destroy(CurrentLvl.gameObject);
            if (_nextLvl != null)
            {
                _currentLoadLvl = _nextLoadLvl;
                CurrentLvl = _nextLvl;
            }
        }
        spawnPos = CurrentLvl.NextLvlPos.position;

        var nextLvl = await _contex.AssetProvider.Load(nextReference);
        _nextLvl = Instantiate(nextLvl, spawnPos, Quaternion.identity).GetComponent<Lvl>();
        _nextLoadLvl = nextReference;
        CurrentLvl.CreatePath(_nextLvl.BasePoint.position);
        CurrentLvl.Initialization(_contex, tankT);
    }
    public void UnloadLvl(bool isGameClose, AssetReference lvlReference = null)
    {
        if (isGameClose)
        {
            _contex.AssetProvider.Unload(_currentLoadLvl);
            Destroy(CurrentLvl.gameObject);
            _currentLoadLvl = null;
            CurrentLvl = null;

            _contex.AssetProvider.Unload(_nextLoadLvl);
            Destroy(_nextLvl.gameObject);
            _nextLoadLvl = null;
            _nextLvl = null;
        } else
        {
            _contex.AssetProvider.Unload(lvlReference);
        }
    }
}
