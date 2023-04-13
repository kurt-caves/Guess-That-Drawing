using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBank : MonoBehaviour
{
    public TextAsset words;
    public string[] easyWords;
    public string[] mediumWords;
    public string[] hardWords;

    void Start()
    {
        string[] allWords = words.text.Split('\n');
        foreach (string word in allWords)
        {
            if (word.Contains("[easy]"))
            {
                easyWords = AddToArray(easyWords, word.Replace("[easy]", "").Trim());
            }
            else if (word.Contains("[medium]"))
            {
                mediumWords = AddToArray(mediumWords, word.Replace("[medium]", "").Trim());
            }
            else if (word.Contains("[hard]"))
            {
                hardWords = AddToArray(hardWords, word.Replace("[hard]", "").Trim());
            }
        }
    }

    string[] AddToArray(string[] array, string value)
    {
        List<string> list = new List<string>(array);
        list.Add(value);
        return list.ToArray();
    }

    public string GetRandomWord(string difficulty)
    {
        if (difficulty == "easy")
        {
            return easyWords[Random.Range(0, easyWords.Length)];
        }
        else if (difficulty == "medium")
        {
            return mediumWords[Random.Range(0, mediumWords.Length)];
        }
        else if (difficulty == "hard")
        {
            return hardWords[Random.Range(0, hardWords.Length)];
        }
        else
        {
            return "";
        }
    }
}