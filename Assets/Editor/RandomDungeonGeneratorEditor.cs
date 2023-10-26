using System;
using UnityEditor;
using UnityEngine;

//자식 클래스도 동일하게 적용
[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    private AbstractDungeonGenerator _generator;

    private void Awake()
    {
        _generator = target as AbstractDungeonGenerator;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Dungeon"))
        {
            _generator.GenerateDungeon();
        }
        
        if (GUILayout.Button("Clear Dungeon"))
        {
            _generator.Clear();
        }
    }
}
