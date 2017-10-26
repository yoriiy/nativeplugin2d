using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppUtil : MonoBehaviour {

    // タッチ位置
    private static Vector3 TouchPosition = Vector3.zero;

    // タッチの状態取得
	public static TouchInfo GetTouch()
    {
        // Windowsエディタ判定
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Windows
            // タッチ開始状態を返す
            if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
            // タッチ移動状態を返す
            if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
            // タッチ終了状態を返す
            if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
        }
        // スマートフォン判定
        else
        {
            if (Input.touchCount > 0)
            {
                // タッチ判定を返す
                return (TouchInfo)((int)Input.GetTouch(0).phase);
            }
        }
        // タッチしていない
        return TouchInfo.None;
    }

    // タッチ位置を取得
    public static Vector3 GetTouchPosition()
    {
        // Windowsエディタ判定
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // タッチ位置の座標を返す
            TouchInfo touch = AppUtil.GetTouch();
            if (touch != TouchInfo.None) { return Input.mousePosition; }
        }
        // スマートフォン判定
        else
        {
            if (Input.touchCount > 0)
            {
                // タッチ位置の座標を返す
                Touch touch = Input.GetTouch(0);
                TouchPosition.x = touch.position.x;
                TouchPosition.y = touch.position.y;
                return TouchPosition;
            }
        }
        return Vector3.zero;
    }

    // スクリーンからワールド座標を取得する
    public static Vector3 GetTouchWorldPosition(Camera camera)
    {
        // タッチ位置からワールドの座標を取得
        return camera.ScreenToWorldPoint(GetTouchPosition());
    }
}

public enum TouchInfo
{
    // タッチしていない
    None = 99,
    // タッチ開始
    Began = 0,
    // タッチ移動
    Moved = 1,
    // 
    Statlonary = 2,
    // タッチ終了
    Ended = 3,
    // タッチキャンセル
    Canceled = 4
}