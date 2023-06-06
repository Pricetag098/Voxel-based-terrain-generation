
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AsyncOperation = UnityEngine.AsyncOperation;

public class Menu : MonoBehaviour
{
    bool loading = false;
    public Image bar;
    public GameObject startIndicator;
    AsyncOperation loadProgress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loading)
        {
            if(loadProgress.progress >= 0.9f)
            {
                startIndicator.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    loadProgress.allowSceneActivation = true;
                }
            }
        }
    }

    public void loadScene()
    {
        if(!loading)
        loadProgress = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        loadProgress.allowSceneActivation = false;
        loading = true;
    }
}
