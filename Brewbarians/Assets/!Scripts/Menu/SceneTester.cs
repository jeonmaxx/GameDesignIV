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
        if (File.Exists(path + "scene.json"))
        {
            Vector2 tmp = SaveGameManager.ReadFromJSON<Vector2>("scene.json");
            tmpIndex = (int)tmp.x;
        }
        SceneManager.LoadScene(tmpIndex);
    }

    public void NewGame()
    {
        DeleteData("scene.json");
        DeleteData("items.json");
        DeleteData("seeds.json");
        DeleteData("recipes.json");
        DeleteData("points.json");
        DeleteData("plants.json");
        DeleteData("fields.json");
        DeleteData("signSeeds.json");
        DeleteData("brewing.json");
        DeleteData("tutorial.json");
        DeleteData("bushes.json");
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
