using UnityEditor;

public class ScriptViewWindow : EditorWindow {

	[MenuItem("Window/ScriptViewWindow")]
    public static void OpenWindow()
    {
        EditorWindow.GetWindow<ScriptViewWindow>();
    }
}
