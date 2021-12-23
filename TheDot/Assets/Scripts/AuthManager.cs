using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public static FirebaseAuth auth;
    public static FirebaseUser User;
    public static DatabaseReference DBReference;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("Menu")]
    public TMP_InputField nick;
    public static TMP_InputField nick1;
    public Text rating;
    public Text xp;
    public Text level;
    public Text coins;
    public static Text rating1;
    public static Text xp1;
    public static Text level1;
    public static Text coins1;
    public GameObject menuPanel;

    public static int xpNow = -1;
    public static int coinsNow = -1;
    public static int levelNow = -1;
    public static int ratingNow = -1;

    bool hasCheckedLogin = false;

    static AuthManager instance;

    private void Awake()
    {
        instance = this;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void Start()
    {
        nick1 = nick;
        rating1 = rating;
        xp1 = xp;
        level1 = level;
        coins1 = coins;
    }

    private void FixedUpdate()
    {
        if (auth != null && hasCheckedLogin == false && SceneManager.GetActiveScene().name == "Menu")
        {
            CheckAutoLogin();
            hasCheckedLogin = true;
        }
    }

    void CheckAutoLogin()
    {
        if (auth.CurrentUser != null)
        {
            menuPanel.SetActive(true);
            loginPanel.SetActive(false);

            nick1 = nick;
            rating1 = rating;
            xp1 = xp;
            level1 = level;
            coins1 = coins;

            StartCoroutine(LoadUserData());

            StartCoroutine(CheckVersion());
        }
    }
   
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    public void SignOutButton()
    {
        auth.SignOut();
        menuPanel.SetActive(false);
        loginPanel.SetActive(true);
        ClearRegisterForm();
        ClearLoginForm();
    }

    public void SaveNickButton()
    {
        StartCoroutine(UpdateUsernameAuth(nick.text));
        StartCoroutine(UpdateUsernameDatabase(nick.text));
    }

    private void ClearLoginForm()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        confirmLoginText.text = "";
        warningLoginText.text = "";
    }

    private void ClearRegisterForm()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
        warningRegisterText.text = "";
    }

    private IEnumerator Login(string _email, string _password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            User = LoginTask.Result;
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";

            menuPanel.SetActive(true);
            loginPanel.SetActive(false);

            nick.text = User.DisplayName;

            ClearRegisterForm();
            ClearLoginForm();

            StartCoroutine(LoadUserData());
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                User = RegisterTask.Result;

                if (User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        StartCoroutine(UpdateRating(1000));
                        StartCoroutine(UpdateXP(0));
                        StartCoroutine(UpdateLevel(1));
                        StartCoroutine(UpdateID(User.UserId, User.DisplayName));
                        StartCoroutine(UpdateCoins(100));

                        StartCoroutine(LoadUserData());

                        loginPanel.SetActive(true);
                        registerPanel.SetActive(false);
                        warningRegisterText.text = "";

                        ClearRegisterForm();
                        ClearLoginForm();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        UserProfile profile = new UserProfile { DisplayName = _username };

        var ProfileTask = User.UpdateUserProfileAsync(profile);
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    public static IEnumerator UpdateID(string id, string nick)
    {
        var DBTask = DBReference.Child("users").Child(nick).Child("id").SetValueAsync(id);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
    }

    public static IEnumerator UpdateRating(int _rating)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).Child("rating").SetValueAsync(_rating);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        if (SceneManager.GetActiveScene().name == "Menu")
            instance.StartCoroutine(LoadUserData());
    }

    public static IEnumerator UpdateXP(int _xp)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).Child("xp").SetValueAsync(_xp);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        if (SceneManager.GetActiveScene().name == "Menu")
            instance.StartCoroutine(LoadUserData());
    }

    public static IEnumerator UpdateCoins(int _coins)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).Child("coins").SetValueAsync(_coins);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        if (SceneManager.GetActiveScene().name == "Menu")
            instance.StartCoroutine(LoadUserData());
    }

    public static IEnumerator UpdateLevel(int _level)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).Child("level").SetValueAsync(_level);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        if (SceneManager.GetActiveScene().name == "Menu")
            instance.StartCoroutine(LoadUserData());
    }

    public static IEnumerator LoadUserData()
    {
        User = auth.CurrentUser;
        nick1.text = User.DisplayName;
        PlayerPrefs.SetString("Nick", User.DisplayName);

        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;

            rating1.text = "Rating: " + snapshot.Child("rating").Value.ToString();
            level1.text = "Level: " + snapshot.Child("level").Value.ToString();
            xp1.text = "XP: " + snapshot.Child("xp").Value.ToString();
            coins1.text = "TheDotCoins: " + snapshot.Child("coins").Value.ToString();

            PlayerPrefs.SetInt("Level", int.Parse(snapshot.Child("level").Value.ToString()));
        }
    }

    public static IEnumerator CheckVersion()
    {
        var DBTask = DBReference.Child("version").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;

            if (snapshot.Value.ToString() != Application.version)
            {
                SceneManager.LoadScene("UpdateApp");
            }
        }
    }

    public void AdRewardFunction()
    {
        StartCoroutine(AdReward());
    }

    public static IEnumerator AdReward()
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;

            xpNow = int.Parse(snapshot.Child("xp").Value.ToString());

            instance.StartCoroutine(UpdateXP(xpNow + 20));
        }
    }

    public static IEnumerator CheckXP()
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;

            levelNow = int.Parse(snapshot.Child("level").Value.ToString());
            xpNow = int.Parse(snapshot.Child("xp").Value.ToString());

            if (xpNow >= 80 && xpNow > -1)
            {
                int i = Mathf.FloorToInt(xpNow / 80);

                instance.StartCoroutine(UpdateLevel(levelNow + i));
                instance.StartCoroutine(UpdateXP(xpNow - 80 * i));
            }
            else if (xpNow <= -1)
            {
                int i = Mathf.FloorToInt(xpNow / 80);

                instance.StartCoroutine(UpdateLevel(levelNow + i));
                instance.StartCoroutine(UpdateXP(xpNow + 80 * (i + 1)));
            }
        }
    }

    public static IEnumerator AddXPAfterSPGame()
    {
        int random = UnityEngine.Random.Range(10, 20);

        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;

            xpNow = int.Parse(snapshot.Child("xp").Value.ToString());
            levelNow = int.Parse(snapshot.Child("level").Value.ToString());

            if (xpNow + random >= 80)
            {
                instance.StartCoroutine(UpdateXP(xpNow + random - 80));
                instance.StartCoroutine(UpdateLevel(levelNow + 1));
            }
            else
            {
                instance.StartCoroutine(UpdateXP(xpNow + random));
            }
        }
    }

    public static IEnumerator SubstractXPAfterSPGame()
    {
        int random = UnityEngine.Random.Range(10, 20);

        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;

            xpNow = int.Parse(snapshot.Child("xp").Value.ToString());
            levelNow = int.Parse(snapshot.Child("level").Value.ToString());

            if (levelNow == 1 && xpNow - random < 0)
            {
                instance.StartCoroutine(UpdateXP(0));
            }
            else
            {
                instance.StartCoroutine(UpdateXP(xpNow - random));
            }

        }
    }

    public static IEnumerator AddCoins(int coinsToAdd)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;

            coinsNow = int.Parse(snapshot.Child("coins").Value.ToString());

            instance.StartCoroutine(UpdateCoins(coinsNow + coinsToAdd));
        }
    }

    public static IEnumerator SubstractCoins(int coinsToSubtract)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;

            coinsNow = int.Parse(snapshot.Child("coins").Value.ToString());

            instance.StartCoroutine(UpdateCoins(coinsNow - coinsToSubtract));

        }
    }

    public static IEnumerator AddRatingAfterGame(string opponentNick, Text ratingChangeText)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var DBTask1 = DBReference.Child("users").Child(opponentNick).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;
            DataSnapshot snapshot1 = DBTask1.Result;

            var DBTask2 = DBReference.Child("users").Child(snapshot1.Child("id").Value.ToString()).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

            DataSnapshot snapshot2 = DBTask2.Result;

            ratingNow = int.Parse(snapshot.Child("rating").Value.ToString());
            int ratingOpponent = int.Parse(snapshot2.Child("rating").Value.ToString());

            int ratingChange = 0;
            if(ratingNow < ratingOpponent)
            {
                ratingChange = Mathf.Abs(ratingNow - ratingOpponent) / 25 + 10;
            }
            else if (ratingNow > ratingOpponent && Mathf.Abs(ratingNow - ratingOpponent) / 25 < 10)
            {
                ratingChange = 10 - Mathf.Abs(ratingNow - ratingOpponent) / 25;
            }
            else if (ratingNow == ratingOpponent)
            {
                ratingChange = 10;
            }

            ratingChangeText.text = "Rating: " + ratingNow.ToString() + " + " + ratingChange.ToString() + " = " + (ratingNow + ratingChange).ToString();

            instance.StartCoroutine(UpdateRating(ratingNow + ratingChange));
        }
    }

    public static IEnumerator SubstractRatingAfterGame(string opponentNick, Text ratingChangeText)
    {
        var DBTask = DBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        var DBTask1 = DBReference.Child("users").Child(opponentNick).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            DataSnapshot snapshot = DBTask.Result;
            DataSnapshot snapshot1 = DBTask1.Result;

            var DBTask2 = DBReference.Child("users").Child(snapshot1.Child("id").Value.ToString()).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

            DataSnapshot snapshot2 = DBTask2.Result;

            ratingNow = int.Parse(snapshot.Child("rating").Value.ToString());
            int ratingOpponent = int.Parse(snapshot2.Child("rating").Value.ToString());

            int ratingChange = 0;
            if (ratingNow < ratingOpponent && Mathf.Abs(ratingNow - ratingOpponent) / 25 < 10)
            {
                ratingChange = 10 - Mathf.Abs(ratingNow - ratingOpponent) / 25;
            }
            else if (ratingNow > ratingOpponent)
            {
                ratingChange = Mathf.Abs(ratingNow - ratingOpponent) / 25 + 10;
            }
            else if (ratingNow == ratingOpponent)
            {
                ratingChange = 10;
            }

            ratingChangeText.text = "Rating: " + ratingNow.ToString() + " - " + ratingChange.ToString() + " = " + (ratingNow - ratingChange).ToString();

            if (ratingNow - ratingChange >= 0)
                instance.StartCoroutine(UpdateRating(ratingNow - ratingChange));
            else
                instance.StartCoroutine(UpdateRating(0));
        }
    }
}
