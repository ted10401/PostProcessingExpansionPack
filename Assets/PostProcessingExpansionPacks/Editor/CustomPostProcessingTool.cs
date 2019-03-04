using UnityEditor;
using UnityEngine;

public static class CustomPostProcessingTool
{
    private const string MENU_ITEM_PATH = "Assets/Create/Custom Post-processing Effect";

    [MenuItem(MENU_ITEM_PATH)]
    private static void Create()
    {
        CreateShader();
    }

    private static void CreateShader()
    {
        CustomPostProcessingEndNameEditAction customPostProcessingEndNameEditAction = ScriptableObject.CreateInstance<CustomPostProcessingEndNameEditAction>();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, customPostProcessingEndNameEditAction, "NewCustomPostProcessingShader.shader", EditorGUIUtility.IconContent("Shader Icon").image as Texture2D, string.Empty);
    }
}
