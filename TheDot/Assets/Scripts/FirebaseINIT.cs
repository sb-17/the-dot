using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.Networking;

public class FirebaseINIT : MonoBehaviour
{
    void Start()
    {
        CheckIfReady();
    }

    public static void CheckIfReady()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                PlayerPrefs.SetInt("offline", 0);
                SceneManager.LoadScene("Menu");
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                PlayerPrefs.SetInt("offline", 1);
                SceneManager.LoadScene("MenuOffline");
            }
        });
    }
}