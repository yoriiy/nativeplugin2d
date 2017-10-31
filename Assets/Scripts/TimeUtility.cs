using System;
using System.Collections;
using UnityEngine;

public static class TimeUtility
{
    // コルーチンに制限時間を設ける
    public static IEnumerator CoroutineTimeOutCheck(this MonoBehaviour component, IEnumerator coroutine, float limitTime)
    {
        DateTime startTime = DateTime.Now;
        component.StartCoroutine(coroutine);
        while(coroutine.MoveNext())
        {
            if(startTime.IsElapsedTime(limitTime) == false)
            {
                Debug.LogError("coroutine Fore Break");
                yield break;
            }
            yield return null;
        }
    }

    // 経過時間チェック
    public static bool IsElapsedTime(this DateTime startTime, float checkTime)
    {
        return (DateTime.Now - startTime).TotalSeconds< checkTime;
    }
}
