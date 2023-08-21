using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTester : MonoBehaviour
{
    public DataCollector dataCollector;
    public int tmpIndex = 1;
    private string path;

    private void Start()
    {
        path = Application.persistentDataPath + "/";
    }

    public void LoadGame()
    {
        if (File.Exists(path + "sceneGD.json"))
        {
            Vector2 tmp = SaveGameManager.ReadFromJSON<Vector2>("scene.json");
            tmpIndex = (int)tmp.x;
        }
        SceneManager.LoadScene(tmpIndex);
    }

    public void NewGame()
    {
        DeleteData("sceneGD.json");
        DeleteData("itemsGD.json");
        DeleteData("recipesGD.json");
        DeleteData("pointsGD.json");
        DeleteData("tutorialGD.json");
        DeleteData("bushesGD.json");
        SceneManager.LoadScene(tmpIndex);
    }

    public void SceneChangeButton(int index)
    {
        dataCollector.CollectData();
        SceneManager.LoadScene(index);
    }

    public void BackToMainMenu(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void DeleteData(string dataName)
    {
        if (File.Exists(path + dataName))
            File.Delete(path + dataName);
    }
}
