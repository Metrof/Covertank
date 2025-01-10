using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class SimpleOperationWithLoadScreen : ILoadingOperation
{
    private UnityAction _action;

    public SimpleOperationWithLoadScreen(UnityAction action)
    {
        _action = action;
    }
    public UniTask Load()
    {
        _action?.Invoke();
        return UniTask.CompletedTask;
    }
}
