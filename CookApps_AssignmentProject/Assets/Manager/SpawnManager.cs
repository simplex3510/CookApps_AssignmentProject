using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using Base.Entity;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    public List<GameObject> cachedMobList = new List<GameObject>();
    public List<BaseEntity> spawnedMobList = new List<BaseEntity>();

    private readonly int MATRIX_SIZE = 3;
    private readonly Vector2[,] EnemySpawnMatrix = new Vector2[3, 3]
    {
        { new Vector2(2,  2), new Vector2(4,  2), new Vector2(6,  2) },
        { new Vector2(2,  0), new Vector2(4,  0), new Vector2(6,  0) },
        { new Vector2(2, -2), new Vector2(4, -2), new Vector2(6, -2) }
    };

    private readonly Vector2[,] CharacterSpawnMatrix = new Vector2[3, 3]
    {
        { new Vector2(-2,  2), new Vector2(-4,  2), new Vector2(-6,  2) },
        { new Vector2(-2,  0), new Vector2(-4,  0), new Vector2(-6,  0) },
        { new Vector2(-2, -2), new Vector2(-4, -2), new Vector2(-6, -2) }
    };

    // 현재 선택된 챕터에 따라서 유동적으로 Mob pooling을 자동화할 계획이었으나
    // 기간적 한계로 인하여 한정적 구현
    public void SpawnEnemies(int chapterNUmber, int stageNumber)
    {
        var enemyMatrix = ChapterManager.Instance.ChapterList[chapterNUmber].StageList[stageNumber].EnemyMatrix;

        for (int row = 0; row < MATRIX_SIZE; row++)
        {
            for (int col = 0; col < MATRIX_SIZE; col++)
            {
                switch(enemyMatrix[row, col])
                {
                    case (int)EChapterMobs.Mob1:
                        spawnedMobList.Add(Instantiate(cachedMobList[(int)EChapterMobs.Mob1]).GetComponent<BaseEntity>());
                        spawnedMobList[spawnedMobList.Count - 1].transform.position = EnemySpawnMatrix[row, col];
                        break;
                    case (int)EChapterMobs.Mob2:
                        Instantiate(cachedMobList[(int)EChapterMobs.Mob2]).transform.position = EnemySpawnMatrix[row, col];
                        break;
                    case (int)EChapterMobs.Mob3:
                        Instantiate(cachedMobList[(int)EChapterMobs.Mob3]).transform.position = EnemySpawnMatrix[row, col];
                        break;
                    case (int)EChapterMobs.Boss:
                        Instantiate(cachedMobList[(int)EChapterMobs.Boss]).transform.position = EnemySpawnMatrix[row, col];
                        break;
                }
            }
        }
    }
}

#region Component Class

#endregion
