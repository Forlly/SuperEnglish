using Models.Abstract;
using TMPro;
using UnityEngine;

public class LetterBlock : BlockBase
{
    [SerializeField] public TMP_Text _letterTxt;
    public bool IsUsed;
    private int _price = 1;
}
