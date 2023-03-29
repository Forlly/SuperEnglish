using System;
using System.Collections;
using Models.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Views;

public class LetterBlock : BlockBase
{
    [SerializeField] public TMP_Text _letterTxt;
    [SerializeField] private Image _backgroungImg;
    public bool IsUsed;
    private int _price = 1;
    private int _indx;

    public int GetPrice()
    {
        return _price;
    }

    public void SetIndx(int indx)
    {
        _indx = indx;
    }
    public int GetIndx()
    {
        return _indx;
    }

    public void ChooseLetter()
    {
        IsUsed = true;
        _backgroungImg.color = new Color(0.33f, 0.8f, 1f);
    }

    private IEnumerator WordGuessed()
    {
        int i = 100;
        _backgroungImg.color = new Color(0.34f, 1f, 0.46f);
        while (i > 0)
        {
            i -= 10;
            yield return new WaitForFixedUpdate();
        }
        _backgroungImg.color = Color.white;
    }
    private IEnumerator WordNotGuessed()
    {
        int i = 100;
        _backgroungImg.color = new Color(1f, 0.36f, 0.3f);
        while (i > 0)
        {
            i -= 10;
            yield return new WaitForFixedUpdate();
        }
        _backgroungImg.color = Color.white;
    }
    public void ReturnLetterWordGuessed()
    {
        IsUsed = false;
        StartCoroutine(WordGuessed());
    }
    public void ReturnLetterWordNotGuessed()
    {
        IsUsed = false;
        StartCoroutine(WordNotGuessed());
    }


}
