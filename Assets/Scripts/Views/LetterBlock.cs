using System;
using System.Collections;
using System.Collections.Generic;
using Models.Abstract;
using TMPro;
using UnityEngine;

public class LetterBlock : BlockBase
{
    [SerializeField] public TMP_Text _letterTxt;
    public char _letterChar;
}
