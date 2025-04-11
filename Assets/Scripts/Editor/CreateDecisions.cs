using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class CreateDecisions : EditorWindow
{
    private string scriptName = "";
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Create Decision"))
        {

            CreateScript();
            

        }
        GUILayout.Label("Decision Name");
        scriptName = EditorGUILayout.TextField("",scriptName);
        EditorGUILayout.EndVertical();

    }

    public void CreateScript()
    {
        if (scriptName.Length == 0)
        {
            return;
        }
        scriptName = scriptName.Replace(" ", "");
        scriptName = char.ToUpper(scriptName[0]) + scriptName.Substring(1);
        string assetPath = "Assets/Scripts/Decisions/" + scriptName + ".cs";
        if (File.Exists(assetPath))
        {
            Debug.Log("Ya existe un archivo en " + assetPath);
            return;
        }
        using StreamWriter outfile = new StreamWriter(assetPath);
        outfile.WriteLine("using UnityEngine;");
        outfile.WriteLine("");
        outfile.WriteLine("[CreateAssetMenu(menuName = \"PlayerDecisions/" + scriptName + "\")]\r\n");
        outfile.WriteLine("public class " + scriptName + " : Decision");
        outfile.WriteLine("{");
        outfile.WriteLine("public override bool Decide(StateMachineController stateMachine)");
        outfile.WriteLine("{");
        outfile.WriteLine("return false;");
        outfile.WriteLine("}");
        outfile.WriteLine("}");
        AssetDatabase.Refresh();

    }
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGui;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGui;
    }
    private void OnSceneGui(SceneView obj)
    {

    }
    [MenuItem("Tools/Create Decisions")]
    private static void OpenWindow()
    {
        CreateDecisions window = new CreateDecisions();
        window.titleContent = new GUIContent("Create Decision");
        window.Show();
    }
}
