using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TextMeshProUGUI _progressText;

    public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        _loaderCanvas.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            _progressBar.value = progress;
            _progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}
