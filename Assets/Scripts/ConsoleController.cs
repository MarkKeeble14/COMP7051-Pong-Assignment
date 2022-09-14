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
    public GameManager gameManager;
    public ScrollRect display;
    public bool open;

    private void Update()
    {
        open = myConsole.activeSelf;
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
        if (!open)
            Time.timeScale = 1;
    }

    private void OpenConsole()
    {
        Time.timeScale = 0;
        myConsole.SetActive(true);
        consoleTextDisplay.text +=
              ">" + "type 'help()' for a list of available commands\n"
            + ">" + "type 'exit()' to close the console\n"
            + ">";
        consoleInput.ActivateInputField();
        consoleInput.Select();
        // https://stackoverflow.com/a/59968835
        StartCoroutine(ApplyScrollPosition(display, 0));
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
        // https://stackoverflow.com/a/59968835
        float backup = display.verticalNormalizedPosition;

        consoleTextDisplay.text += input + "\n>" + ParseInput(input);
        history.Add(input);
        historyIndex = history.Count;
        consoleInput.text = "";
        consoleInput.ActivateInputField();
        consoleInput.Select();

        // https://stackoverflow.com/a/59968835
        StartCoroutine(ApplyScrollPosition(display, backup));
    }

    // https://stackoverflow.com/a/59968835
    IEnumerator ApplyScrollPosition(ScrollRect sr, float verticalPos)
    {
        yield return new WaitForEndOfFrame();
        sr.verticalNormalizedPosition = verticalPos;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)sr.transform);
    }

    private string ParseInput(string input)
    {
        if (input.Equals("help()"))
        {
            return "available commands:\n" +
                "1. bgColor(color)\n" +
                "2. ballColor(color)\n" +
                "3. ballSize(num) - default is 3\n" +
                "4. aiPaddleSpeed(num) - default is 50\n" +
                "5. resetBall()\n" +
                "6. restartGame()\n" +
                "7. help()\n" +
                "8. exit()\n>";
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
                ballMat.SetColor("_EmissionColor", color);
                return "ball color changed to: " + color + "\n>";
            }
            else
            {
                return "color not recognized\n>";
            }
        }
        else if (input.Length >= "aiPaddleSpeed".Length + 3
            && input.Substring(0, "aiPaddleSpeed".Length).Equals("aiPaddleSpeed"))
        {
            string param = input.Split('(', ')')[1];
            float f;
            if (float.TryParse(param, out f))
            {
                gameManager.SetAIPaddleSpeed(f);
                return "ai paddle speed changed to " + f + "\n>";
            }
            else
            {
                return "invalid parameter\n>";
            }
        }
        else if (input.Length >= "resetBall".Length + 2
          && input.Substring(0, "resetBall".Length).Equals("resetBall"))
        {
            gameManager.ResetBall();
            return "ball reset\n>";
        }
        else if (input.Length >= "restartGame".Length + 2
         && input.Substring(0, "restartGame".Length).Equals("restartGame"))
        {
            gameManager.RestartGame();
            return "if you see this, something has gone terribly wrong\n>";
        }
        return "command not recognized; type 'help()' for a list of available commands\n>";
    }
}
