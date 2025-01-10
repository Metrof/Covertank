using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class LoadScreen : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private Image _fillSliderImage;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void OpenLoadScreen()
    {
        _content.SetActive(true);
        _fillSliderImage.fillAmount = 0;
    }
    public void AddFillProgress(float progress)
    {
        _fillSliderImage.fillAmount += progress;
    }
    public void CloseLoadScreen()
    {
        _content.SetActive(false);
        _fillSliderImage.fillAmount = 1;
    }
}
