using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsoleController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI consoleTextDisplay;
    [SerializeField] private TMP_InputField consoleInput;
    [SerializeField] private GameObject myConsole;
    private float waitTimeAfterConsoleClose = 1;

    [SerializeField] private List<string> history = new List<string>();
    [SerializeField] private int historyIndex = -1;

    public BallController ball;
    public Material ballMat;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!myConsole.activeSelf)
            {
                OpenConsole();
            }
        }

        if (myConsole.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (historyIndex - 1 >= 0)
                {
                    historyIndex--;
                    consoleInput.text = history[historyIndex];
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (historyIndex + 1 < history.Count)
                {
                    historyIndex++;
                    consoleInput.text = history[historyIndex];
                }
            }
        }
    }

    private IEnumerator RestartBall()
    {
        yield return new WaitForSecondsRealtime(waitTimeAfterConsoleClose);
        Time.timeScale = 1;
    }

    private void OpenConsole()
    {
        myConsole.SetActive(true);
        consoleTextDisplay.text +=
              ">" + "type 'help()' for a list of available commands\n"
            + ">" + "type 'exit()' to close the console\n"
            + ">";
        consoleInput.Select();
        Time.timeScale = 0;
    }

    public void CloseConsole()
    {
        consoleInput.text = "";
        myConsole.SetActive(false);
        StartCoroutine(RestartBall());
    }

    public void TryInput(string input)
    {
        if (input.Length == 0)
            return;
        // Example Inputs 
        // -> bgColor(blue)
        // -> bgColor(Red)
        // -> ballSpeed(20)
        // -> BallRadius(5)
        // -> GoalieSpeed(20)
        consoleTextDisplay.text += input + "\n>" + ParseInput(input);
        history.Add(input);
        historyIndex = history.Count;
        consoleInput.text = "";
        consoleInput.ActivateInputField();
        consoleInput.Select();
    }

    private string ParseInput(string input)
    {
        if (input.Equals("help()"))
        {
            return "available commands:\n" +
                "1. bgColor(color)\n" +
                "2. ballSize(num)\n" +
                "3. ballColor(color)\n" +
                "4. help()\n" +
                "5. exit()\n>";
        }
        else if (input.Equals("exit()"))
        {
            CloseConsole();
            return "exiting console\n";
        }
        else if (input.Length >= "bgColor".Length + 3
            && input.Substring(0, "bgColor".Length).Equals("bgColor"))
        {
            // User is trying to change background color
            string param = input.Split('(', ')')[1];
            Color color;
            if (ColorUtility.TryParseHtmlString(param, out color))
            {
                Camera.main.backgroundColor = color;
                return "background color changed to: " + color + "\n>";
            }
            else
            {
                return "color not recognized\n>";
            }
        }
        else if (input.Length >= "ballSize".Length + 3
            && input.Substring(0, "ballSize".Length).Equals("ballSize"))
        {
            string param = input.Split('(', ')')[1];
            float f;
            if (float.TryParse(param, out f))
            {
                ball.SetSize(f);
                return "ball size changed to " + f + "\n>";
            }
            else
            {
                return "invalid parameter\n>";
            }
        }
        else if (input.Length >= "ballColor".Length + 3
            && input.Substring(0, "ballColor".Length).Equals("ballColor"))
        {
            // User is trying to change ball color
            string param = input.Split('(', ')')[1];
            Color color;
            if (ColorUtility.TryParseHtmlString(param, out color))
            {
                ballMat.color = color;
                return "ball color changed to: " + color + "\n>";
            }
            else
            {
                return "color not recognized\n>";
            }
        }
        return "command not recognized; type 'help()' for a list of available commands\n>";
    }
}
