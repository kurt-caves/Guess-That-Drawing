using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ChangeSceneWithButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

public class FrontPage : MonoBehaviour
{
    Button fpButton;
    private void OnEnable()
    {   
        // grab the UI document
        UIDocument UIDoc = GetComponent<UIDocument>();
        // grab the root of the UI
        VisualElement root = UIDoc.rootVisualElement;
        // grab the button
        fpButton = root.Q<Button>("front-page-button");

        fpButton.clicked += () => {
            Debug.Log("FrontPage Button Clicked");
            SceneManager.LoadScene("Game");
        };

        Debug.Log("FrontPage Enabled");
    }
}
