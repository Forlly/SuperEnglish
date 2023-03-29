using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Views
{
    public class WordChecker : MonoBehaviour
    {
        public static WordChecker Instance;
        [SerializeField] private TMP_Text _wordDisplay;
        [SerializeField] private TMP_Text _countOfPointsText;
        [SerializeField] private TMP_Text _countOfWordsText;
        public Action<LetterBlock> SelectLetterEvent;
        public Action CheckWordEvent;
        public string CurrentWord;

        private List<string> _words = new List<string>();
        private List<LetterBlock> _lettersInWord = new List<LetterBlock>();
        private List<LetterBlock> _blocksMatrix = new List<LetterBlock>();
        private int _countOfPoints;
        private int _countOfWords;
        private int _countOfGuessedWords;
        private int _currentLettersIndx;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            SelectLetterEvent += AddSelectedLetter;
            CheckWordEvent += CheckWord;
            _countOfGuessedWords = 0;
        }

        public int GetPoints()
        {
            return _countOfPoints;
        }
        
        public int GetCountOfGuessedWords()
        {
            return _countOfGuessedWords;
        }

        public void SetWords(List<string> words, List<LetterBlock> letterBlocks)
        {
            _words = words;
            _blocksMatrix = letterBlocks;
            _countOfWords = _words.Count;
            _countOfWordsText.text = $"{_countOfGuessedWords}/{_countOfWords.ToString()}";

            for (int i = 0; i < words.Count; i++)
            {
                Debug.Log(words[i]);
            }
        }
        
        public void AddSelectedLetter(LetterBlock letterBlock)
        {
            if (letterBlock.IsUsed) return;
            int nextIndx = letterBlock.GetIndx();

            if (_lettersInWord.Count != 0)
            {
                if ((nextIndx - 1 == _currentLettersIndx && nextIndx % 5 != 0)
                    || (nextIndx + 1 == _currentLettersIndx && nextIndx%5 != 4)
                    || nextIndx - 5 == _currentLettersIndx
                    || nextIndx + 5 == _currentLettersIndx
                    || nextIndx - 6 == _currentLettersIndx
                    || nextIndx + 6 == _currentLettersIndx
                    || (nextIndx - 4 == _currentLettersIndx && nextIndx % 5 != 4)
                    || (nextIndx + 4 == _currentLettersIndx && nextIndx%5 != 0))
                {
                    _currentLettersIndx = letterBlock.GetIndx();
                }
                else
                {
                    return;
                }
            }
            else
            {
                _currentLettersIndx = letterBlock.GetIndx();
            }
            
            
            letterBlock.ChooseLetter();
            _lettersInWord.Add(letterBlock);
            CurrentWord += letterBlock._letterTxt.text;
            _wordDisplay.text = CurrentWord;
        }

        private void CheckWord()
        {
            for (int i = 0; i < _words.Count; i++)
            {
                if (CurrentWord ==_words[i].ToUpper())
                {
                    _words.Remove(_words[i]);
                    IncreaseIndicatorsParameters();
                    return;
                }
            }
            
            for (int j = 0; j < _lettersInWord.Count; j++)
            {
                _lettersInWord[j].ReturnLetterWordNotGuessed();
            }
            _lettersInWord.Clear();
            CurrentWord = "";
            _wordDisplay.text = CurrentWord;
        }

        private void IncreaseIndicatorsParameters()
        {
            _countOfGuessedWords++;
            _countOfWordsText.text = $"{_countOfGuessedWords}/{_countOfWords.ToString()}";
            for (int j = 0; j < _lettersInWord.Count; j++)
            {
                _countOfPoints += _lettersInWord[j].GetPrice();
                _lettersInWord[j].ReturnLetterWordGuessed();
            }

            _countOfPointsText.text = _countOfPoints.ToString();
            _lettersInWord.Clear();
            CurrentWord = "";
            _wordDisplay.text = CurrentWord;
        }
    }
}