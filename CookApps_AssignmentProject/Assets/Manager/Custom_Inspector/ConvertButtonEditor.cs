using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JsonManager))]
public class ConverButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Convert C# to Json"))
        {
            ChapterManager.Instance.ChapterList.Add(new Chapter1());
            ChapterManager.Instance.ChapterList[1].StageList.Add(new Stage1());
            ChapterManager.Instance.ChapterList[1].StageList.Add(new Stage2());

            JsonManager.Instance.ConvertCSharpToJson(ChapterManager.Instance.ChapterList, "ChapterAndStageInfo");
        }

        if (GUILayout.Button("Convert Json to C#"))
        {
            var ChapterList = JsonManager.Instance.ConvertJsonToCSharp<List<BaseChapter>>("ChapterAndStageInfo");
        }
    }
}