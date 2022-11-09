using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ImageTimer HarvestTimer; //Время сбора урожая
    public ImageTimer EatingTimer; //Время траты еды
    public ImageTimer RaidTimer; //Время до нападения
    public SFXmusic SFXmusic;
    public VillageHealth health;
    public GameObject pauseMenu;
    public GameObject victoryMenu;
    public Image RaidTimerImage;
    public Image PeasantTimerImage;
    public Image WarriorTimerImage;

    public Button peasantButton,warriorButton,pauseButton;
    
    public Text resourcesText;
    public Text raidCountText;
    public Text harvestCountText;
    public Text wheatDecreaseText;
    public Text gameOverTextScore;
    public Text raidTickText;
    public Text resourcesForVictoryCount;
    public Text wavesForVictoryCount;

    public int wheatPerPeasant, wheatToWarriors; //Количество еды, которую приносит один крестьянин //Количество еды,что потребляет один воин
    public int peasantCost, warriorCost; //Стоимость создания крестьянина //Воина

    public float peasantCreateTime, warriorCreateTime, raidMaxTime; //Время создания крестьянина/Воина //Время до нападения

    public int raidIncrease; //Сколько врагов прибаляется к нападению каждый цикл
    public int nextRaid; //Количество врагов в нападении

    public int peasantCount; //Количество крестьян
    public int warriorsCount; //Количество воинов
    public int wheatCount; //Количество еды в деревне

    private float peasantTimer = -2;
    private float warriorTimer = -2;
    private float raidTimer;

    private int eatingCount; //Остаток еды
    private int warriorEatCount; //Количество еды потребляемая воинами
    private int surviveWarriors; //Количество выживших после нападения

    private int totalHarvestCount = 0;
    private int totalPeasantCount = 0;
    private int totalWarriorCount = 0;
    private int totalSurviveRaidCount = 0;
    private int totalDeathWarrior = 0;
    public int totalResourcesForVictory = 200; //Сколько собрать ресурсов для победы
    public int waveForVictory = 10; //Сколько продержаться волн для победы

    private int raidTick = 0;
    private int raidWave = 3; //Количество циклов до нападения

    [SerializeField] private AudioClip[] _clipPeasant;
    public AudioClip[] _audioEffectsClipMassive;
    public AudioClip _clickSound;


    private void Start()
    {
        Time.timeScale = 1f;
        raidTimer = raidMaxTime;
    }

    private void Update()
    {
       
        RaidTimeManager();
        HarvestTimeManager();

        UnitTimerManager(ref peasantCost,ref wheatCount,ref peasantTimer,
            ref peasantButton,ref PeasantTimerImage,ref peasantCount, 
            ref peasantCreateTime, ref totalPeasantCount, ref _clipPeasant[Random.Range(0, _clipPeasant.Length)]);

        UnitTimerManager(ref warriorCost,ref wheatCount,ref warriorTimer,
            ref warriorButton,ref WarriorTimerImage,ref warriorsCount,
            ref warriorCreateTime, ref totalWarriorCount, ref _audioEffectsClipMassive[0]);

        UpdateText();
        GameOverTextMenu();
        Victory();
    }

    /// <summary>
    /// Метод создания Воинов и Крестьян
    /// </summary>
    /// <param name="unitCost">Стоимость создания</param>
    /// <param name="wheatCount">Количество еды в деревне</param>
    /// <param name="unitTimer">Время создания</param>
    /// <param name="button">Кнопка</param>
    /// <param name="imgT">Картинка таймера</param>
    /// <param name="unitCount">Количество воинов или крестьян</param>
    /// <param name="creatT">Время создания</param>
    /// <param name="totalCount">Параметр для итоговой статистики</param>
    /// <param name="_clip">Аудио для воина или крестьянина</param>
    private void UnitTimerManager(ref int unitCost, ref int wheatCount, ref float unitTimer, ref Button button, ref Image imgT, ref int unitCount, ref float creatT,ref int totalCount, ref AudioClip _clip)
    {
        if (unitCost > wheatCount || unitTimer > -1)
        {
            button.interactable = false;
        }
        else if (unitCost <= wheatCount)
        {
            button.interactable = true;
        }

        if (unitTimer > 0)
        {
            unitTimer -= Time.deltaTime;
            imgT.fillAmount = unitTimer / creatT;
        }
        else if (unitTimer > -1)
        {
            imgT.fillAmount = 1;
            button.interactable = true;
            unitCount += 1;
            unitTimer = -2;
            totalCount += 1;
            SFXmusic.PlaySoundEffects(_clip);
        }

    }

    private void RaidTimeManager()
    {
        raidTimer -= Time.deltaTime;
        RaidTimerImage.fillAmount = raidTimer / raidMaxTime;

        if (RaidTimer.Tick)
        {
            raidTimer = raidMaxTime;
            raidTick += 1;
            
            surviveWarriors = warriorsCount - nextRaid;

            if (surviveWarriors >= 0)
            {
                totalSurviveRaidCount = raidTick;
                if (nextRaid > 0)
                {
                    totalDeathWarrior = totalDeathWarrior + nextRaid;
                    SFXmusic.PlaySoundEffects(_audioEffectsClipMassive[1]);
                }
            }
            else if (surviveWarriors < 0)
            {
                totalDeathWarrior = totalDeathWarrior + warriorsCount;
                totalSurviveRaidCount = raidTick - 1; 
                health.Death();
                SFXmusic.PlaySoundEffects(_audioEffectsClipMassive[1]);
            }
            
            warriorsCount = surviveWarriors;
            
            if(raidTick <= 2)
            {   
                raidWave -= 1;
                nextRaid = 0;
            }
            else
            {
                if (raidTick == 3)
                {
                    raidWave -= 1;
                }
                nextRaid += raidIncrease;
                
            }
        }
    }
    private void HarvestTimeManager()
    {
        if (HarvestTimer.Tick)
        {
            wheatCount += peasantCount * wheatPerPeasant;
            totalHarvestCount += peasantCount * wheatPerPeasant;
        }
        if (EatingTimer.Tick)
        {
            warriorEatCount = warriorsCount * wheatToWarriors;
            eatingCount = wheatCount - warriorEatCount;
            
            if (wheatCount > 0)
            {
                if(warriorEatCount > wheatCount)
                {
                    health.ChangeHealth(eatingCount);
                }
            }
            else if (wheatCount < 0)
            {
                health.ChangeHealth(-warriorEatCount);
            }
            wheatCount = eatingCount;
            SFXmusic.PlaySoundEffects(_audioEffectsClipMassive[2]);
        }
    }
    public void CreatePeasant()
    {
        wheatCount -= peasantCost;
        peasantTimer = peasantCreateTime;
        SFXmusic.PlaySoundEffects(_clickSound);
        peasantButton.interactable = false;
    }
    public void CreateWarrior()
    {
        wheatCount -= warriorCost;
        warriorTimer = warriorCreateTime;
        SFXmusic.PlaySoundEffects(_clickSound);
        warriorButton.interactable = false;
    }
    private void UpdateText()
    {
        resourcesText.text = peasantCount + "\n\n" + warriorsCount + "\n\n" + wheatCount;
        raidTickText.text = "" + raidWave;
        raidCountText.text = "" + nextRaid;
        harvestCountText.text = "+" + peasantCount * wheatPerPeasant;
        wheatDecreaseText.text = "-" + warriorsCount * wheatToWarriors;
        resourcesForVictoryCount.text = "" + wheatCount + "\n" + "/" + totalResourcesForVictory;
        wavesForVictoryCount.text = "" + totalSurviveRaidCount + "/" + waveForVictory;
    }
    
    public void Restart()
    {
        SFXmusic.PlaySoundEffects(_clickSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameOverTextMenu()
    {
        gameOverTextScore.text = totalHarvestCount + "\n\n" + totalPeasantCount + "\n\n" +
            totalWarriorCount + "\n\n" + totalSurviveRaidCount + "\n\n" + totalDeathWarrior;
    }

    private void Victory()
    {
        if (surviveWarriors >= 0 && nextRaid > 0)
        {
            if (totalSurviveRaidCount >= waveForVictory && wheatCount >= totalResourcesForVictory)
            {
                victoryMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        
    }

    public void Pause()
    {
        SFXmusic.PlaySoundEffects(_clickSound);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        SFXmusic.PlaySoundEffects(_clickSound);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    
}
