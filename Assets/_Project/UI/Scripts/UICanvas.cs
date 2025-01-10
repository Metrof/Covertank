using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour, ICanvas
{
    [SerializeField] private MainPanel _mainPanel;
    [SerializeField] private GamePanel _gamePanel;
    [SerializeField] private LobbyPanel _lobbyPanel;
    [SerializeField] private SettingPanel _settingPanel;
    [SerializeField] private TemporaryOfferUI _temporaryOfferUI;
    [SerializeField] private TaskPanel _taskPanel;
    [SerializeField] private TankUpdatePanel _tankUpdatePanel;
    [SerializeField] private CollectionPanel _collectionPanel;
    [SerializeField] private LosePanel _losePanel;

    private EventBus _eventBus;
    private List<GameObject> _panelObjects = new List<GameObject>();

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe(this);

        _panelObjects.Add(_mainPanel.gameObject);
        _panelObjects.Add(_gamePanel.gameObject);
        _panelObjects.Add(_lobbyPanel.gameObject);
        _panelObjects.Add(_settingPanel.gameObject);
        _panelObjects.Add(_temporaryOfferUI.gameObject);
        _panelObjects.Add(_taskPanel.gameObject);
        _panelObjects.Add(_tankUpdatePanel.gameObject);
        _panelObjects.Add(_collectionPanel.gameObject);
        _panelObjects.Add(_losePanel.gameObject);

        _mainPanel.PlayButton.onClick.AddListener(() => _eventBus.RaiseEvent<IGameManager>(g => g.LvlStart()));

        _gamePanel.CloseGameButton.onClick.AddListener(() => _eventBus.RaiseEvent<IGameManager>(g => g.CloseGame()));
        _gamePanel.ContinieGameButton.onClick.AddListener(() => _eventBus.RaiseEvent<IGameManager>(g => g.LvlStart()));

        _losePanel.RestartButton.onClick.AddListener(() => _eventBus.RaiseEvent<IGameManager>(g => g.CloseGame()));

        OpenMenuUI();
    }
    public void SetActiveForContinieGameButton(bool active)
    {
        _gamePanel.ContinieGameButton.gameObject.SetActive(active);
    }
    public void OpenMenuUI()
    {
        foreach (GameObject go in _panelObjects)
        {
            go.SetActive(false);
        }
        _mainPanel.gameObject.SetActive(true);
    }
    public void OpenLoseUI()
    {
        _losePanel.gameObject.SetActive(true);
    }

    public void TankHPChage(float hp, float maxHP)
    {
        _gamePanel.HealthBar.Fill.fillAmount = hp / maxHP;
    }
}
