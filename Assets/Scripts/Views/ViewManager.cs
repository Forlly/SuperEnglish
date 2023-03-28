using TMPro;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
     public Collider _field;
     [SerializeField] private TMP_Text _countOfCoinTxt;
     
     private GameModel _gameModel;
     private int _countOfCoin;


    public void Init(GameModel gameModel)
    {
        _gameModel = gameModel;
    }


    private void IncreaseCountOfCoin(int value)
    {
        _countOfCoin += value;
        _countOfCoinTxt.text = _countOfCoin.ToString();
    }

    private void ShowGameOverPanel()
    {
        _gameModel.EndModel();
    }
}
