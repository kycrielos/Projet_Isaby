using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    public Slider slider;
    public Text progressText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    public IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(4);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = Mathf.Round(progress * 100f) + "%";
            yield return null;
        }

    }
}
