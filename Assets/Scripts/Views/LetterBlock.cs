using System;
using Models.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Views;

public class LetterBlock : BlockBase
{
    [SerializeField] public TMP_Text _letterTxt;
    public bool IsUsed;
    private int _price = 1;

    public int GetPrice()
    {
        return _price;
    }
    
}
