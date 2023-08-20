using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScene : MonoBehaviour
{
    public int sceneIndex;

    public DataCollector dataCollector;
    public Image blackScreen;

    private bool endFading;
    public bool screenManager;
    private bool startFading;
    private bool loadNext;
    public float alpha = 1;

    private void Awake()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1);
        if(screenManager)
            alpha = 1;
        else
            alpha = 0;
        StartCoroutine(EndFrame());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(EndFrame());
            endFading = false;
            dataCollector.CollectData();
            startFading = true;
        }
    }

    private void Update()
    {
        if(alpha > 0 && endFading)
        {
            alpha -= Time.deltaTime * 0.8f;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
        }
        else if(alpha < 0.1f && endFading)
        {
            endFading = false;
        }

 
        if (alpha < 1 && startFading)
        {
            alpha += Time.deltaTime * 0.8f;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
        }
        else if(alpha > 0.9f && startFading)
        {
            loadNext = true;
        }

        if (loadNext)
            StartCoroutine(LoadYourAsyncScene(sceneIndex));
    }

    public IEnumerator LoadYourAsyncScene(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator EndFrame()
    {
        yield return new WaitForSeconds(0.5f);
        if (screenManager)
            endFading = true;
        StopCoroutine(EndFrame());
    }
}
