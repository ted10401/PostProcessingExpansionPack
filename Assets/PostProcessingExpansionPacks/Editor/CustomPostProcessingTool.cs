using UnityEditor;
using UnityEngine;

public static class CustomPostProcessingTool
{
    private const string MENU_ITEM_PATH = "Assets/Create/Custom Post-processing Effect/";

    [MenuItem(MENU_ITEM_PATH + "Before Transparent")]
    private static void CreateBeforeTransparentEffect()
    {
        CreateShader("BeforeTransparent");
    }

    [MenuItem(MENU_ITEM_PATH + "Before Stack")]
    private static void CreateBeforeStackEffect()
    {
        CreateShader("BeforeStack");
    }

    [MenuItem(MENU_ITEM_PATH + "After Stack")]
    private static void CreateAfterStackEffect()
    {
        CreateShader("AfterStack");
    }

    private static void CreateShader(string injection)
    {
        CustomPostProcessingEndNameEditAction customPostProcessingEndNameEditAction = ScriptableObject.CreateInstance<CustomPostProcessingEndNameEditAction>();
        customPostProcessingEndNameEditAction.injection = injection;
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, customPostProcessingEndNameEditAction, "NewCustomPostProcessingShader.shader", EditorGUIUtility.IconContent("Shader Icon").image as Texture2D, string.Empty);
    }
}
