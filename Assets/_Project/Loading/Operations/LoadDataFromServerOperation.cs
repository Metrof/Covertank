using Cysharp.Threading.Tasks;

public class LoadDataFromServerOperation : ILoadingOperation
{
    private Context _context;

    public LoadDataFromServerOperation(Context context)
    {
        _context = context;
    }

    public async UniTask Load()
    {
        var data = _context.DataLoader.LoadData();
        _context.Data = data;
        await UniTask.Yield();
    }
}
