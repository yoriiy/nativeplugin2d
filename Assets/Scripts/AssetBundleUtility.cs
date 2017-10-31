using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// AssetBundle ユーティリティクラス
// パスの取得クラス
public class AssetBundleUtility
{
    public static string GetDownloadPath()
    {
        return GetRelativePath() + "/" + GetPlatformFolder() + "/";
    }

    public static string GetPlatformFolder()
    {
#if UNITY_EDITOR
        return EditorPlatformFolder(EditorUserBuildSettings.activeBuildTarget);
#else
		return PlatformFolder(Application.platform);
#endif
    }

    // ストリーミングアセットのパスを取得
    private static string GetRelativePath()
    {
        return "http://github.com/yoriiy/nativeplugin2d/raw/master/Assets/StreamingAssets";
        //return "file://" + Application.streamingAssetsPath;
    }

#if UNITY_EDITOR
    private static string EditorPlatformFolder(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            //case BuildTarget.WebPlayer:
            //    return "WebPlayer";
            default:
                Debug.LogWarning("GetPlatformFolder " + target.ToString());
                return null;
        }
    }
#endif

    private static string PlatformFolder(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            //case RuntimePlatform.WindowsWebPlayer:
            //case RuntimePlatform.OSXWebPlayer:
            //    return "WebPlayer";
            default:
                Debug.LogWarning("GetPlatformFolder " + platform.ToString());
                return null;
        }
    }
}
