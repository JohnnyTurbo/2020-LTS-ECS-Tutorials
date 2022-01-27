using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SetupNewTutorialFolders : EditorWindow
{
    private string newTutorialDirectory;
    private string newTutorialName;
    
    [MenuItem("TMG/Create New Tutorial Folders")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SetupNewTutorialFolders));
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Create a new folder structure for tutorials", EditorStyles.largeLabel);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField($"Root Folder Name: {newTutorialDirectory}");
        newTutorialName = EditorGUILayout.TextField("", newTutorialName);
        newTutorialDirectory = $"ECS_{newTutorialName}";
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("GO!"))
        {
            AssetDatabase.CreateFolder("Assets", newTutorialDirectory);
            AssetDatabase.CreateFolder($"Assets/{newTutorialDirectory}", "Prefabs");
            AssetDatabase.CreateFolder($"Assets/{newTutorialDirectory}", "Scenes");
            AssetDatabase.CreateFolder($"Assets/{newTutorialDirectory}", "Scripts");
            AssetDatabase.CreateFolder($"Assets/{newTutorialDirectory}/Scripts", "AuthoringAndMono");
            AssetDatabase.CreateFolder($"Assets/{newTutorialDirectory}/Scripts", "ComponentsAndTags");
            AssetDatabase.CreateFolder($"Assets/{newTutorialDirectory}/Scripts", "Systems");

            var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, $"Assets/{newTutorialDirectory}/Scenes/{newTutorialName}-Demo.unity");
        }

        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        
        EditorGUILayout.EndHorizontal();
    }
}
