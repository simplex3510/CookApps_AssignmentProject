using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public enum EChapterNumber : int
{
    None = 0,
    Chapter1,
    Chapter2,
    Size,
}

public class ChapterManager : MonoSingleton<ChapterManager>
{
    public  EChapterNumber CurChapterNumber { get; private set; } = EChapterNumber.Chapter1;

    public List<BaseChapter> ChapterList { get; private set; } = new List<BaseChapter>()
    {
        null    // 0번 index는 편의를 위해 null 할당
    };

    // 기간적 한계로 인하여 한정적 구현
    private void Awake()
    {
        switch (CurChapterNumber)
        {
            case EChapterNumber.Chapter1:
                // temp code
                ChapterList.Add(new Chapter1());
                ChapterList[1].StageList.Add(new Stage1());
                ChapterList[1].StageList.Add(new Stage2());
                JsonManager.Instance.ConvertCSharpToJson(ChapterList, "ChapterAndStageInfo");
                // -----

                ChapterList = JsonManager.Instance.ConvertJsonToCSharp<List<BaseChapter>>("ChapterAndStageInfo");
                SpawnManager.Instance.SpawnEnemies((int)CurChapterNumber, 1);
                break;
            case EChapterNumber.Chapter2:
                // Chapter2에 대한 적절한 처리
                break;
            default:
                break;
        }
    }
}

#region Component Class

#region BaseChapter Class
public class BaseChapter
{
    // 다형성 활용을 위한 상위 클래스 선언
    public List<BaseStage> StageList { get; set; } = null;
}
#endregion

#region BaseStage Class
public class BaseStage
{   
    public int EnemyCount { get; set; } = 0;
    public bool IsStageClear { get; set; } = false;
    public int[,] EnemyMatrix { get; set; } = null;
}
#endregion

#region Chapter1 Class
public enum EChapterMobs : int
{
    None = 0,

    // 101 == 1스테이지 01번째 몹 - 한 챕터 당 99개의 몹을 가질 수 있음
    // 기간적 한계로 인하여 한정적 구현
    Mob1 = 1, 
    Mob2,
    Mob3,
    Boss,

    Size
}

public class Chapter1 : BaseChapter
{
    public Chapter1()
    {
        StageList = new List<BaseStage>()
        {
            null    // 0번 index는 편의를 위해 null 할당
        };
    }
}

#region Stage1 Class
public class Stage1 : BaseStage
{
    public Stage1()
    {
        EnemyCount = 3;
        IsStageClear = false;
        EnemyMatrix = new int[,]
        {
            { 1, 0, 0 },
            { 1, 0, 0 },
            { 1, 0, 0 }
        };
    }
}
#endregion

#region Stage2 Class
public class Stage2 : BaseStage
{
    public Stage2()
    {
        EnemyCount = 4;
        IsStageClear = false;
        EnemyMatrix = new int[,]
        {
            { 1, 0, 0 },
            { 1, 2, 0 },
            { 1, 0, 0 }
        };
    }
}
#endregion

#endregion

#endregion