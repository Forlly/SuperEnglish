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
        public Action<LetterBlock> SelectLetter;
        public string CurrentWord;

        private List<string> _words = new List<string>();
        private List<LetterBlock> _lettersInWord = new List<LetterBlock>();
        private int _countOfPoints;
        private int _countOfWords;
        private int _countOfGuessedWords;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            SelectLetter += AddSelectedLetter;
            _countOfGuessedWords = 0;
        }
        void Update ()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5f;
            Vector3 CurMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(CurMousePos);
                RaycastHit hitData;
                if (Physics.Raycast(CurMousePos, transform.TransformDirection(Vector3.forward)*100, out hitData))
                {

                    AddSelectedLetter(hitData.transform.gameObject.GetComponent<LetterBlock>());
                }
            }
        }
        
        public void SetWords(List<string> words)
        {
            _words = words;
            _countOfWords = _words.Count;
            _countOfWordsText.text = $"{_countOfGuessedWords}/{_countOfWords.ToString()}";
        }
        
        public void AddSelectedLetter(LetterBlock letterBlock)
        {
            _lettersInWord.Add(letterBlock);
            CurrentWord += letterBlock._letterTxt.text;
            _wordDisplay.text = CurrentWord;

            for (int i = 0; i < _words.Count; i++)
            {
                if (CurrentWord ==_words[i].ToUpper())
                {
                    IncreaseIndicatorsParameters();
                }
            }
            
            Debug.Log(CurrentWord);
        }

        private void IncreaseIndicatorsParameters()
        {
            _countOfGuessedWords++;
            _countOfWordsText.text = $"{_countOfGuessedWords}/{_countOfWords.ToString()}";
            for (int j = 0; j < _lettersInWord.Count; j++)
            {
                _countOfPoints += _lettersInWord[j].GetPrice();
            }

            _countOfPointsText.text = _countOfPoints.ToString();
            _lettersInWord.Clear();
            CurrentWord = "";
        }
    }
}