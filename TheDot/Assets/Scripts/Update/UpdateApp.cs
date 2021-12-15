using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateApp : MonoBehaviour
{
    public void OpenPlayStore()
    {
        if (Application.isMobilePlatform)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.LostInGames.TheDot");
        }
        else
        {
            Application.OpenURL("https://lostingames.itch.io/the-dot");
        }
    }
}
