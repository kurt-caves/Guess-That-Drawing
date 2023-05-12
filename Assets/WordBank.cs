using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    WordBank

    Randomly chooses a word from the words.txt
*/
public class WordBank : MonoBehaviour
{
    public static WordBank Instance { get; private set; }

    public TextAsset words;
    public string[] wordList;
    

    void Awake()
    {
       
        Instance = this;
        string[] allWords = words.text.Split('\n');
        foreach (string word in allWords)
        {
            wordList = AddToArray(wordList, word.Trim());
        }
    }

    string[] AddToArray(string[] array, string value)
    {
        List<string> list = new List<string>(array);
        list.Add(value);
        return list.ToArray();
    }

    public string GetRandomWord()
    {
        return wordList[Random.Range(0, wordList.Length)];
  
    }
}