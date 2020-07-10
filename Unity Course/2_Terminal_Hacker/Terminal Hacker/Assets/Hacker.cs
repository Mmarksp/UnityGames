using UnityEngine;

public class Hacker : MonoBehaviour
{
    // Game Configuration
    const string menuHint = "Type menu for more challenges";
    string[] level1Passwords = { "carpet", "shield", "guitar", "television", "color" };
    string[] level2Passwords = { "democracy", "nationality", "selection", "quality", "transcript" };
    string[] level3Passwords = { "equilibrium", "debugging", "spacious", "acquisition", "surveillance" };

    // Game State
    string user;
    int level;
    enum Screen { NameScreen, MainMenu, Password, Win}; //Finite State Machine
    Screen currentScreen;
    string password;

    // Start is called before the first frame update
    void Start()
    {
        currentScreen = Screen.NameScreen;
        ShowNameScreen();
    }

    void ShowNameScreen()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("Please type your name");
    }

    void ShowMainMenu(string input)
    {
        Terminal.ClearScreen();
        currentScreen = Screen.MainMenu;
        Terminal.WriteLine("Hello "+ input +",");
        Terminal.WriteLine("They say we all have an opportunity");
        Terminal.WriteLine("A chance to make something of ourselves");
        Terminal.WriteLine("What do you want to hack into first?");
        Terminal.WriteLine("Press 1 for Dad's Cellphone (Easy)");
        Terminal.WriteLine("Press 2 for Mayor's Office (Normal)");
        Terminal.WriteLine("Press 3 for NSA (Hard)");
        Terminal.WriteLine("");
        Terminal.WriteLine("We provide you with letters");
        Terminal.WriteLine("But you will have to finish the word");
    }

    void OnUserInput(string input)
    {
        print("The user typed " + input);
        if (input == "restart")
        {
            Start();
        }
        else if (currentScreen == Screen.NameScreen)
        {
            ChooseName(input);
        }
        else if (input == "menu")
        {
            currentScreen = Screen.MainMenu;
            ShowMainMenu(user);
        }
        else if (input == "exit" || input == "close" || input == "quit")
        {
            print("If on the web close the tab");
            Application.Quit();
        }
        else if (currentScreen == Screen.MainMenu)
        {
            RunMainMenu(input);
        }
        else if (currentScreen == Screen.Password)
        {
            ValidatePassword(input);
        }
    }

    void ChooseName(string input)
    {
        user = input;
        ShowMainMenu(user);
    }

    void RunMainMenu(string input)
    {
        bool isValidLevelNumber = (input == "1" || input == "2" || input == "3");
        if (isValidLevelNumber)
        {
            level = int.Parse(input);
            AskForPassword();
        }
        else if (input == "destroy")
        {
            Terminal.WriteLine("You have ruined EVERYTHING");
        }
        else
        {
            Terminal.WriteLine("Your input is invalid");
        }
    }

    void AskForPassword()
    {
        currentScreen = Screen.Password;
        Terminal.ClearScreen();
        SetRandomPassword();
        Terminal.WriteLine("Enter your password, hint: " + password.Anagram());
        Terminal.WriteLine(menuHint);
    }

    void SetRandomPassword()
    {
        switch (level)
        {
            case 1:
                password = level1Passwords[Random.Range(0, level1Passwords.Length)];
                break;
            case 2:
                password = level2Passwords[Random.Range(0, level2Passwords.Length)];
                break;
            case 3:
                password = level3Passwords[Random.Range(0, level3Passwords.Length)];
                break;
            default:
                Debug.LogError("Invalid level number");
                break;
        }
    }

    void ValidatePassword(string input)
    {
        if(input == password)
        {
            DisplayWinScreen();
        }
        else
        {
            AskForPassword();
        }
    }

   void DisplayWinScreen()
    {
        currentScreen = Screen.Win;
        Terminal.ClearScreen();
        Terminal.WriteLine("Password accepted");
        Terminal.WriteLine("Logging in...");
        ShowLevelReward();
    }

    void ShowLevelReward()
    {
        Terminal.WriteLine("Displaying the reward");
        switch (level)
        {
            case 1:
                Terminal.WriteLine(@"
    _______,
   /      //
  /      //
 /_____ //
(______(/    
");
                Terminal.WriteLine(menuHint);
                break;
            case 2:
                Terminal.WriteLine(@"
  ____  _____   ___   _   _  ______ 
 |  __||  _  \ / _ \ | | | ||  __  \
 | |__ | |_| || /_\ || | | || |  | |
 |  __||  _  /|  _  || |_| || |__| |
 |_|   |_| \_\|_| |_||_____||______/
");
                Terminal.WriteLine(menuHint);
                break;
            case 3:
                Terminal.WriteLine(@"
/0 \_______
\__/-=' = '  
");
                Terminal.WriteLine("We have secured the key");
                Terminal.WriteLine("Goodbye "+user);
                break;
            default:
                Debug.LogError("Invalid level reached");
                break;
        }
    }
}

