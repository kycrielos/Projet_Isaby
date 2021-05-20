using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    public Slider slider;
    public Text progressText;

    public GameObject introductionText;
    public GameObject LoadingScreen;

    public Animator animator;

    bool fadeCompleted;

    // Start is called before the first frame update
    void Start()
    {
            StartCoroutine(LoadMainScene());
    }
    public IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(5);

        animator.SetTrigger("FadeOut");

        yield return new WaitUntil(() => fadeCompleted);

        introductionText.SetActive(false);
        LoadingScreen.SetActive(true);

        yield return new WaitForSeconds(5);

        fadeCompleted = false;
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = Mathf.Round(progress * 100f) + "%";
            yield return null;
        }

        slider.value = 1;
        progressText.text = "100%";
        animator.SetTrigger("FadeOut");

        yield return new WaitUntil(() => fadeCompleted);

        operation.allowSceneActivation = true;
    }

    public void OnFadeComplete()
    {
        fadeCompleted = true;
    }

}
