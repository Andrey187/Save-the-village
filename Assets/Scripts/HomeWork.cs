using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class HomeWork : MonoBehaviour
{
    [SerializeField] private Text timerText; //текст таймера
    [SerializeField] private Text[] pins = new Text[3]; //массив текста для пинов
    [SerializeField] private GameObject[] button = new GameObject[3]; //массив кнопок

    [SerializeField] private float pinLeftScoreWin; //значение выигрыша для левого пина
    [SerializeField] private float pinMidScoreWin; //значение выигрыша для центрального пина
    [SerializeField] private float pinRightScoreWin; //значение выигрыша для правого пина
    [SerializeField] private float timeLose; //время на игру

    [SerializeField] private GameObject winPanel; //окно выигрыша
    [SerializeField] private GameObject losePanel; //окно проигрыша

    private readonly Random rnd = new Random(); 
    private static int pinLeftScore; //текущее значение левого пина
    private static int pinMidScore; //текущее значение центрального пина
    private static int pinRightScore; //текущее значение правого пина
    private int[] pinScore = new int[] { pinLeftScore, pinMidScore, pinRightScore }; //массив значений каждого пина
    private int[] scoreRandom = new int[3]; //массив генерации случайных чисел для 3х пинов
    private float currentTime; // текущее время
    

    public void Start()
    {
        Time.timeScale = 1f;
        ScoreManagerStart();
    }

    public void Update()
    {
        Timer();
        ScoreManagerWinOrLose();
    }

    public void Timer()
    {
        currentTime = (int)Mathf.Round(Time.timeSinceLevelLoad);
        timerText.text = currentTime.ToString();
    }

    public void ScoreManagerStart() //генерация случаных чисел в каждый пин
    {
        for(int i = 0; i < scoreRandom.Length; i++)
        {
            scoreRandom[i] = rnd.Next(0, 10);
            pins[i].text = scoreRandom[i].ToString();
        }
    }

    public void DrillButtonClick() //кнопка для дрели
    {
        pinScore[0] = ++scoreRandom[0];
        pinScore[1] = --scoreRandom[1];
        pinScore[2] = scoreRandom[2];
        DisplayTextData();
    }

    public void HammerButtonClick() //кнопка для молотка
    {
        pinScore[0] = --scoreRandom[0];
        pinScore[1] = scoreRandom[1] += 2;
        pinScore[2] = --scoreRandom[2];
        DisplayTextData();
    }

    public void MasterKeyButtonClick() //кнопка для отмычки
    {
        pinScore[0] = --scoreRandom[0];
        pinScore[1] = ++scoreRandom[1];
        pinScore[2] = ++scoreRandom[2];
        DisplayTextData();
    }

    public void DisplayTextData() //вывод чисел в текст каждого пина
    {
        int min = 0;
        int max = 10;
        for (int i = 0; i < pins.Length; i++)
        {
            scoreRandom[i] = Mathf.Clamp(scoreRandom[i], min, max);//ограничение не ниже 0 и не выше 10
            pinScore[i] = Mathf.Clamp(pinScore[i], min, max);

            pins[i].text = pinScore[i].ToString();
        }

    }

    public void ScoreManagerWinOrLose() //меню победы или поражения
        {
            for (int i = 0; i < pins.Length; i++) //для кажого пина проверяется условие
            {
                if ((pinScore[0] == pinLeftScoreWin) 
                && (pinScore[1] == pinMidScoreWin) 
                && (pinScore[2] == pinRightScoreWin))
                {
                    Time.timeScale = 0f;
                    for (int j = 0; j < 3; j++)
                    {
                        button[j].GetComponent<Button>().interactable = false;
                    }
                    winPanel.SetActive(true);
                }
            
            }

            if (currentTime == timeLose)
            {   
                Time.timeScale = 0f;
                for(int i = 0; i < 3; i++)
                {
                    button[i].GetComponent<Button>().interactable = false;
                }
                losePanel.SetActive(true);
            }
        }

    public void Restart() //перезагрузка
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}




