using Cysharp.Threading.Tasks;

public class LoadLvlOperation : ILoadingOperation
{
    private Context _context;

    public LoadLvlOperation(Context context)
    {
        _context = context;
    }
    public async UniTask Load()
    {
        await _context.LvlManager.LoadLvl(_context.Data.CurrentLvl, _context.MainCharactersManager.Tank.Health);
    }
}
