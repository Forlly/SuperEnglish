using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using Random = System.Random;

public class MatrixPlaceholder : MonoBehaviour
{
    [SerializeField] private int _countOfWords;
    [SerializeField] private List<string> _words = new List<string>();
    [SerializeField] private List<LetterBlock> _blocksMatrix = new List<LetterBlock>();
    private int _height = 5;
    private int _width = 5;
    private Char[,] _matrixLetters;
    private List<char> allLetters = new List<char>()
    {
        'A', 'B', 'B', 'D', 'E', 'F', 'G', 'H', 
        'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
        'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    };

    private int _countOfReplaceWord = 0;

    private List<string> _currentWords = new List<string>();
    private List<int> positionsOfLetters = new List<int>();

    private void Start()
    {
        _matrixLetters = new char[_height, _width];
        Shuffle(_words);

        for (int i = 0; i < _countOfWords; i++)
        {
            _currentWords.Add(_words[i]);
        }

        for (int i = 0; i < _currentWords.Count; i++)
        {
            Debug.Log(_currentWords[i]);
        }

        GenerateMatrix();

        List<char> lettersInCurrentWords = _currentWords.SelectMany(word => word).ToList();
        
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                if ( _matrixLetters[i, j] == ' ')
                {
                    _matrixLetters[i, j] = allLetters[rng.Next(allLetters.Count)];
                }
            }
        }
        
        ShowWordsMatrix();
    }

    public void GenerateMatrix()
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                _matrixLetters[i, j] = ' ';
            }
        }
        
        for (int i = 0; i < _currentWords.Count; i++)
        {
           GenerateWord(_currentWords[i]);
           
        }
        
    }

    private void GenerateWord(string word)
    {
        positionsOfLetters.Clear();
        
        int x = rng.Next(_height);
        int y = rng.Next(_width);
        int nextX;
        int nextY;


        while (_matrixLetters[x,y] != ' ' && _matrixLetters[x,y] != word[0])
        {
            x = rng.Next(_height);
            y = rng.Next(_width);
        }

        var nextPos = CheckSide(word[0], x, y);
        
        while (nextPos.Item1 == -1)
        {
            x = rng.Next(_height);
            y = rng.Next(_width);
            
            while (_matrixLetters[x,y] != ' '&& _matrixLetters[x,y] != word[0])
            {
                x = rng.Next(_height);
                y = rng.Next(_width);
            }
            
            nextPos = CheckSide(word[0], x, y);
        }

        positionsOfLetters.Add(x);
        positionsOfLetters.Add(y);
        Debug.Log(x + "" + y);
        nextX = nextPos.Item1;
        nextY = nextPos.Item2;
        
        _matrixLetters[x, y] = char.ToUpper(word[0]);
        
        for (int i = 1; i < word.Length; i++)
        {
            _matrixLetters[nextX, nextY] = char.ToUpper(word[i]);
            positionsOfLetters.Add(nextX);
            positionsOfLetters.Add(nextY);

            if (i + 1 < word.Length)
            {
                nextPos = CheckSide(word[i + 1], nextX, nextY);

                nextX = nextPos.Item1;
                nextY = nextPos.Item2;
            }
            
            if (nextX == -1)
            {
                Debug.Log("REPLACE WORD");
                _countOfReplaceWord++;

                for (int j = 0; j < word.Length; j++)
                {
                    _matrixLetters[positionsOfLetters[0], positionsOfLetters[1]] = ' ';
                    if (positionsOfLetters.Count > 2)
                    {
                        positionsOfLetters.RemoveAt(0);
                        positionsOfLetters.RemoveAt(0);
                    }
            
                }

                if (_countOfReplaceWord > 15)
                {
                    Debug.Log("REPLACE ANOTHER WORD");
                    ReplaceWord(word);
                    _countOfReplaceWord = 0;
                    return;
                }

                GenerateWord(word);
                return;
            }
        }


        _countOfReplaceWord = 0;
        for (int i = 0; i < word.Length; i++)
        {
            for (int j = 0; j < allLetters.Count; j++)
            {
                if (allLetters[j] == word[i])
                {
                    allLetters.Remove(allLetters[j]);
                }
            }
        }
    }

    private void ReplaceWord(string word)
    {
        for (int i = 0; i < _currentWords.Count; i++)
        {
            if (_currentWords[i] == word && _words.Count > _countOfWords)
            {
                _currentWords.Remove(_currentWords[i]);
                _words.RemoveAt(0);
                _currentWords.Add(_words[_countOfWords]);
            }
        }
        
        GenerateWord(_words[_countOfWords]);

    }

    private (int, int) CheckSide(char currentLetter,int x, int y)
    {
        List<Direction> directions = new List<Direction>();
        int nextX = x;
        int nextY = y;
        
        for (int i = 0; i < 8; i++)
        {
            directions.Add((Direction)i);
        }
        
        Shuffle(directions);

        while (true)
        {
            switch (directions[0])
                    {
                        case Direction.Up:
                            if (y + 1 < _height && (_matrixLetters[x, y + 1] == ' '|| _matrixLetters[x, y + 1] == currentLetter))
                            {
                                nextY = y + 1;
                                return (nextX, nextY);
                            }
                            break;
                        case Direction.RightUp:
                            if (x + 1 < _width && y + 1 < _height && (_matrixLetters[x + 1, y + 1] == ' ' || _matrixLetters[x + 1, y + 1] == currentLetter))
                            {
                                nextX = x + 1;
                                nextY = y + 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.Right:
                            if (x + 1 < _width && (_matrixLetters[x + 1, y] == ' ' || _matrixLetters[x+1, y] == currentLetter))
                            {
                                nextX = x + 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.RightDown:
                            if (x + 1 > _width && y - 1 >= 0 && (_matrixLetters[x + 1, y - 1] == ' ' || _matrixLetters[x+1, y-1] == currentLetter))
                            {
                                nextX = x + 1;
                                nextY = y - 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.Down:
                            if (y - 1 >= 0 && (_matrixLetters[x, y - 1] == ' ' || _matrixLetters[x, y - 1] == currentLetter))
                            {
                                nextY = y - 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.LeftDown:
                            if (x - 1 >= 0 && y - 1 >= 0 && (_matrixLetters[x - 1, y - 1] == ' ' || _matrixLetters[x-1, y-1] == currentLetter))
                            {
                                nextX = x - 1;
                                nextY = y - 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.Left:
                            if (x - 1 >= 0 && (_matrixLetters[x - 1, y] == ' ' || _matrixLetters[x - 1, y] == currentLetter))
                            {
                                nextX = x - 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.LeftUp:
                            if (x - 1 >= 0 && y + 1 < _height && (_matrixLetters[x - 1, y + 1] == ' ' || _matrixLetters[x-1, y+1] == currentLetter))
                            {
                                nextX = x - 1;
                                nextY = y + 1;
                                return (nextX, nextY);
                            }
                            break;
                    }

            directions.RemoveAt(0);
            if (directions.Count == 0)
            {
                Debug.Log("No free direction");
                return (-1,-1);
            }
        }
    }


    private void ShowWordsMatrix()
    {
        List<char> tmp = new List<char>();

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                tmp.Add(_matrixLetters[i, j]);
            }
        }

        for (int i = 0; i < _blocksMatrix.Count; i++)
        {
            _blocksMatrix[i]._letterTxt.text = tmp[i].ToString();
        }
    }

    private static readonly Random rng = new Random();  
    
    public static void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
}


