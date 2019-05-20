using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SceneLoad", 3);
    }

    public void SceneLoad()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
