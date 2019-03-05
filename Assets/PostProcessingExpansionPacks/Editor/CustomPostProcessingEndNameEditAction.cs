using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.IO;

public class CustomPostProcessingEndNameEditAction : EndNameEditAction
{
    private const string TEMPLATE_PATH = "PostProcessingExpansionPacks/Editor/Templates/";
    public string injection;

    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        string scriptName = Path.GetFileNameWithoutExtension(pathName);
        CreateFile(pathName, scriptName, "shader", "TemplateShader.txt");
        CreateFile(pathName, scriptName, "cs", "TemplateScript.txt");
    }

    private void CreateFile(string pathName, string fileName, string extension, string resourceFile)
    {
        pathName = Path.ChangeExtension(pathName, extension);
        string scriptName = Path.GetFileNameWithoutExtension(pathName);
        pathName = pathName.Replace(scriptName + "." + extension, fileName + "." + extension);

        resourceFile = Path.Combine(Application.dataPath, TEMPLATE_PATH + resourceFile);
        string text = File.ReadAllText(resourceFile);
        text = text.Replace("$INJECTION", injection);
        text = text.Replace("$CUSTOMNAME", scriptName);

        StreamWriter sr = File.CreateText(pathName);
        sr.WriteLine(text);
        sr.Close();

        AssetDatabase.ImportAsset(pathName);
        ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(pathName));
        AssetDatabase.Refresh();
    }
}