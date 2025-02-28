using UnityEditor;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public static class MainSceneAutoLoader
    {
        
        static MainSceneAutoLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        [MenuItem("Tools/MainSceneAutoLoader/Enable")]
        public static void Enable()
        {
            EditorPrefs.SetBool("Enable", true);
        }

        [MenuItem("Tools/MainSceneAutoLoader/Disable")]
        public static void Disable()
        {
            EditorPrefs.SetBool("Enable", false);
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!EditorPrefs.GetBool("Enable", false)) return;
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorPrefs.SetString("LastScene", SceneManager.GetActiveScene().path);
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    string path = EditorBuildSettings.scenes[0].path;

                    try
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                    catch
                    {
                        Debug.LogError($"Cannot load scene: {path}");
                    }
                }
                else
                    EditorApplication.isPlaying = false;
            }
            if (state == PlayModeStateChange.EnteredEditMode)
                EditorSceneManager.OpenScene(EditorPrefs.GetString("LastScene"));
        }
    }
}