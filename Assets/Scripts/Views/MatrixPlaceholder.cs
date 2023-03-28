using System;
using System.Collections;
using System.Collections.Generic;
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

    private List<string> _currentWords = new List<string>();

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
        int x = rng.Next(_height);
        int y = rng.Next(_width);
        int nextX;
        int nextY;

        while (_matrixLetters[x,y] != ' ')
        {
            x = rng.Next(_height);
            y = rng.Next(_width);
        }

        var nextPos = CheckSide(x, y);
        
        while (CheckSide(x, y).Item1 == -1)
        {
            x = rng.Next(_height);
            y = rng.Next(_width);
            
            while (_matrixLetters[x,y] != ' ')
            {
                x = rng.Next(_height);
                y = rng.Next(_width);
            }
            
            nextPos = CheckSide(x, y);
        }

        nextX = nextPos.Item1;
        nextY = nextPos.Item2;
        
        _matrixLetters[x, y] = word[0];
        Debug.Log(x + "XY" + y);
        Debug.Log(_matrixLetters[x, y]);
        
        for (int i = 1; i < word.Length; i++)
        {
            _matrixLetters[nextX, nextY] = word[i];
            nextPos = CheckSide(nextX, nextY);

            nextX = nextPos.Item1;
            nextY = nextPos.Item2;
        }

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

    private (int, int) CheckSide(int x, int y)
    {
        List<Direction> directions = new List<Direction>();
        bool directionFree = false;
        int nextX = x;
        int nextY = y;
        
        for (int i = 0; i < 8; i++)
        {
            directions.Add((Direction)i);
        }
        
        Shuffle(directions);

        while (directionFree == false)
        {
            switch (directions[0])
                    {
                        case Direction.Up:
                            if (y + 1 < _height && _matrixLetters[x, y+1] == ' ')
                            {
                                Debug.Log("Up" + "FREE" + _matrixLetters[x, y+1]);
                                directionFree = true;
                                nextY = y + 1;
                                return (nextX, nextY);
                            }
                            break;
                        case Direction.RightUp:
                            if (x + 1 < _width && y + 1 < _height && _matrixLetters[x+1, y+1] == ' ')
                            {
                                Debug.Log("RightUp" + "FREE" + _matrixLetters[x+1, y+1]);
                                directionFree = true;
                                nextX = x + 1;
                                nextY = y + 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.Right:
                            if (x + 1 < _width && _matrixLetters[x+1, y] == ' ')
                            {
                                Debug.Log("Right" + "FREE" + _matrixLetters[x+1, y]);
                                directionFree = true;
                                nextX = x + 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.RightDown:
                            if (x + 1 > _width && y - 1 >= 0 && _matrixLetters[x+1, y-1] == ' ')
                            {
                                Debug.Log("RightDown" + "FREE" + _matrixLetters[x+1, y-1]);
                                directionFree = true;
                                nextX = x + 1;
                                nextY = y - 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.Down:
                            if (y - 1 >= 0 && _matrixLetters[x, y - 1] == ' ')
                            {
                                Debug.Log("Down" + "FREE" + _matrixLetters[x, y - 1]);
                                directionFree = true;
                                nextY = y - 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.LeftDown:
                            if (x - 1 >= 0 && y - 1 >= 0 && _matrixLetters[x-1, y-1] == ' ')
                            {
                                Debug.Log("LeftDown" + "FREE" + _matrixLetters[x-1, y-1]);
                                directionFree = true;
                                nextX = x - 1;
                                nextY = y - 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.Left:
                            if (x - 1 >= 0 && _matrixLetters[x - 1, y] == ' ')
                            {
                                Debug.Log("Left" + "FREE" + _matrixLetters[x - 1, y]);
                                directionFree = true;
                                nextX = x - 1;
                                return (nextX, nextY);
                            }
                            break;  
                        case Direction.LeftUp:
                            if (x - 1 >= 0 && y + 1 < _height && _matrixLetters[x-1, y+1] == ' ')
                            {
                                Debug.Log("LeftUp" + "FREE" + _matrixLetters[x-1, y+1]);
                                directionFree = true;
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

        return (nextX, nextY);
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


