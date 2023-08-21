using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class SpawnPosition
{
    public int LastSceneIndex;
    public Vector3 NewSpawnPosition;

    public SpawnPosition(int lastSceneIndex, Vector3 newSpawnPosition)
    {
        LastSceneIndex = lastSceneIndex;
        NewSpawnPosition = newSpawnPosition;
    }
}

public class SpawnManager : MonoBehaviour
{
    public GameObject player;
    public List<SpawnPosition> spawnPositions;
    private Vector2 sceneData;
    public Image introPic;
    private bool startFadeOut;
    private float alpha = 1;

    private void Awake()
    {
        string filePath = Application.persistentDataPath + "/" + "sceneGD.json";
        sceneData = SaveGameManager.ReadFromJSON<Vector2>("sceneGD.json");
        if ((!File.Exists(filePath) || new FileInfo(filePath).Length <= 0) && SceneManager.GetActiveScene().buildIndex == 4)
        {
            player.transform.position = new Vector3(-5.9f, 6.6f, 0);
            StartCoroutine(FadeIntroOut());

        }
        else if(introPic != null)
        {
            introPic.gameObject.SetActive(false);
        }

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            Debug.Log("scene" + sceneData.x);
            if (sceneData.x == spawnPositions[i].LastSceneIndex)
            {
                player.transform.position = spawnPositions[i].NewSpawnPosition;
            }
        }
    }

    private void Update()
    {
        if (introPic != null)
        {
            if (alpha > 0 && startFadeOut)
            {
                alpha -= Time.deltaTime * 0.8f;
                introPic.color = new Color(introPic.color.r, introPic.color.g, introPic.color.b, alpha);
            }
            else if (alpha < 0.1f && startFadeOut)
            {
                introPic.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator FadeIntroOut()
    {
        yield return new WaitForSeconds(3);
        startFadeOut = true;
    }
}
