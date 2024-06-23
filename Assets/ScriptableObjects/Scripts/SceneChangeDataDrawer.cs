using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SceneChangeData
{
    public string sceneName;
}

[CustomPropertyDrawer(typeof(SceneChangeData))]
public class SceneChangeDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int margin = 5;
        var sceneNameProperty = property.FindPropertyRelative("sceneName");
        
        EditorGUI.BeginProperty(position, label, property);
        {
            GUIContent aLabel = new GUIContent(property.name);
            float labelWidth = GUI.skin.label.CalcSize(aLabel).x;
            Rect prefixRect = new Rect(position.x, position.y, labelWidth, position.height);
            EditorGUI.LabelField(prefixRect, property.name);

            Rect dropdown = new Rect(position.x + labelWidth + margin, position.y, position.width - labelWidth - margin, position.height);
            
            var scenes = GetScenesInBuildSettings();
            int currentIndex = Mathf.Max(0, System.Array.IndexOf(scenes, sceneNameProperty.stringValue));
            currentIndex = EditorGUI.Popup(dropdown, currentIndex, scenes);
            sceneNameProperty.stringValue = scenes[currentIndex];
        }
        EditorGUI.EndProperty();

        property.serializedObject.ApplyModifiedProperties();
    }

    private string[] GetScenesInBuildSettings()
    {
        int sceneCount = EditorBuildSettings.scenes.Length;
        string[] scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
        }

        return scenes;
    }
}