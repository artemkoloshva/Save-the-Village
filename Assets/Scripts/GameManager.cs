using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ImageTimer EatingTimer;
    public Image RaidTimerImg;
    public Image PeasantTimerImg;
    public Image WarriorTimerImg;
    public Image WheatTimerImg;

    public Button peasantButton;
    public Button warriorButton;
    public Button harvestButton;

    public Text peasantCountText;
    public Text warriorCountText;
    public Text wheatCountText;
    public Text raidText;
    public Text peasantCostText;
    public Text warriorCostText;
    public Text raidsToWinText;
    public Text statisticWheatCountText;
    public Text statisticWheatUsedCountText;
    public Text statisticPeasantCountText;
    public Text statisticWarroirCountText;
    public Text statisticRaidCountText;
    public Text statisticEnemisKilledCountText;
    public Text statisticTimeText;

    public int peasantCount;
    public int warriorCount;
    public int wheatCount;

    public int wheatPerPeasant;
    public int wheatToWarrior;

    public int peasantCost;
    public int warriorCost;

    public float peasantCreateTime;
    public float warriorCreateTime;
    public float wheatTime;
    public float raidMaxTime;
    public int raidIncrease;
    public int cycleBeforeTheRaid;
    public int nextRaid;

    public int raidsToWin;

    public AudioSource audioRaid;

    public GameObject GameLoseScreen;
    public GameObject GameWinScreen;
    public GameObject Statistic;
    public GameObject Gameplay;

    private float peasantTimer = -2;
    private float warriorTimer = -2;
    private float wheatTimer = -2;
    private float raidTimer;
    private bool safeTime;

    private int statisticAllWheat;
    private int statisticAllWarrior;
    private int statisticEnemisKilled;
    private float statisticTime;

    private void Start()
    {
        GameTimeStop(true);

        safeTime = true;

        statisticAllWheat += wheatCount;

        UpdateText();
        warriorCostText.text = warriorCost.ToString();
        peasantCostText.text = peasantCost.ToString();
        raidsToWinText.text = raidsToWin.ToString();

        raidTimer = raidMaxTime * cycleBeforeTheRaid;
        RaidTimerImg.color = Color.gray;
    }

    private void Update()
    {
        statisticTime += Time.deltaTime;

        BlockButton();

        Raid();

        EatingCycle();

        PeasantTimerReload();
        WarriorTimerReload();
        HarvestTimerReload();

        UpdateText();

        VictoryCheck();
    }
    /// <summary>
    /// Создает крестьянина
    /// </summary>
    public void CreatePeasant()
    {
        wheatCount -= peasantCost;
        peasantTimer = peasantCreateTime;
        peasantButton.interactable = false;
    }
    /// <summary>
    /// Создает воина
    /// </summary>
    public void CreateWarrior()
    {
        wheatCount -= warriorCost; //Платим 
        warriorTimer = warriorCreateTime;
        warriorButton.interactable = false;
    }
    /// <summary>
    /// Собрать пшеницу
    /// </summary>
    public void HarvastTheWheat()
    {
        wheatCount += peasantCount * wheatPerPeasant;
        statisticAllWheat += peasantCount * wheatPerPeasant;
        wheatTimer = wheatTime;
        harvestButton.interactable = false;
    }
    /// <summary>
    /// Меняет состояние игрового времени
    /// </summary>
    /// <param name="timeStop"></param>
    public void GameTimeStop(bool timeStop)
    {
        if (timeStop == true) Time.timeScale = 0;
        if (timeStop == false) Time.timeScale = 1;
    }
    private void UpdateText()
    {
        peasantCountText.text = peasantCount.ToString();
        warriorCountText.text = warriorCount.ToString();
        wheatCountText.text = wheatCount.ToString();

        if (safeTime) raidText.text = "Безопасно";
        else raidText.text = "В отряде " + nextRaid.ToString();

    }
    private void BlockButton()
    {
        if (wheatCount < peasantCost) peasantButton.interactable = false;
        else if (peasantTimer == -2) peasantButton.interactable = true;

        if (wheatCount < warriorCost) warriorButton.interactable = false;
        else if (warriorTimer == -2) warriorButton.interactable = true;
    }
    private void PeasantTimerReload()
    {
        if (peasantTimer > 0)
        {
            peasantTimer -= Time.deltaTime;
            PeasantTimerImg.fillAmount = peasantTimer / peasantCreateTime;
        }
        else if (peasantTimer > -1)
        {
            PeasantTimerImg.fillAmount = 0;
            peasantCount++;
            peasantTimer = -2;
        }
    }
    private void WarriorTimerReload()
    {
        if (warriorTimer > 0)
        {
            warriorTimer -= Time.deltaTime;
            WarriorTimerImg.fillAmount = warriorTimer / warriorCreateTime;
        }
        else if (warriorTimer > -1)
        {
            WarriorTimerImg.fillAmount = 0;
            warriorCount++;
            statisticAllWarrior++;
            warriorTimer = -2;
        }
    }
    private void HarvestTimerReload()
    {
        if (wheatTimer > 0)
        {
            wheatTimer -= Time.deltaTime;
            WheatTimerImg.color = new Color(1, 1, 1, 1 - wheatTimer / wheatTime);
        }
        else if (wheatTimer > -1)
        {
            WheatTimerImg.color = new Color(1, 1, 1, 1);
            wheatTimer = -2;
            harvestButton.interactable = true;
        }
    }
    private void EatingCycle()
    {
        if (EatingTimer.Tick)
        {
            wheatCount -= warriorCount * wheatToWarrior;

            if (wheatCount < 0) wheatCount = 0;
        }
    }
    private void Raid() 
    {
        raidTimer -= Time.deltaTime;

        if (safeTime)
        {
            RaidTimerImg.fillAmount = raidTimer / (raidMaxTime * cycleBeforeTheRaid);

            if (raidTimer <= 0)
            {
                safeTime = false;
                raidTimer = raidMaxTime;
                RaidTimerImg.color = new Color(0.7f, 0.1f, 0.1f, 1);
            }
        }
        else
        {
            RaidTimerImg.fillAmount = raidTimer / raidMaxTime;

            if (raidTimer <= 0)
            {
                audioRaid.Play();
                raidTimer = raidMaxTime;
                warriorCount -= nextRaid;

                if(warriorCount < 0)
                {
                    GameLose();
                }
                else
                {
                    statisticEnemisKilled += nextRaid;
                    nextRaid += raidIncrease;
                }
            }
        }
    }
    private void StatisticTextUpdate()
    {
        statisticWheatCountText.text = statisticAllWheat.ToString();
        statisticWheatUsedCountText.text = (statisticAllWheat - wheatCount).ToString();
        statisticPeasantCountText.text = peasantCount.ToString();
        statisticWarroirCountText.text = statisticAllWarrior.ToString();
        statisticRaidCountText.text = (nextRaid / raidIncrease - 1).ToString();
        statisticEnemisKilledCountText.text = statisticEnemisKilled.ToString();
        statisticTimeText.text = Mathf.Round(statisticTime).ToString() + " секунд";
    }
    private void VictoryCheck()
    {
        if(nextRaid / raidIncrease - 1 == raidsToWin)
        {
            GameTimeStop(true);

            Gameplay.SetActive(false);
            Statistic.SetActive(true);
            GameWinScreen.SetActive(true);
            StatisticTextUpdate();
        }
    }
    private void GameLose()
    {
        GameTimeStop(true);

        GameLoseScreen.SetActive(true);
        Statistic.SetActive(true);
        Gameplay.SetActive(false);
        StatisticTextUpdate();
    }
}