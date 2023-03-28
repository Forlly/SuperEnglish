using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    [SerializeField] private ViewManager _viewManager;
    private GameModel _gameModel;

    private void Awake()
    {
        _gameModel = new GameModel();
        _viewManager.Init(_gameModel);
        _gameModel.Init();
    }


    private void OnDisable()
    {
        _gameModel.EndModel();
    }
}
