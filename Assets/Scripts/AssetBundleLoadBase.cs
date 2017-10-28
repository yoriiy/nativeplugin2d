using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AssetBundleLoaderの基底クラス
public abstract class AssetBundleLoadBase : IEnumerator {

	public object Current
    {
        get
        {
            return null;
        }
    }


    // 偽の場合、コルーチンの終了
    public bool MoveNext()
    {
        return IsDone() == false;
    }

    public void Reset()
    {

    }

    public abstract bool LoadRequest();

    public abstract bool IsDone();

    // Asset取得
    public abstract T GetAsset<T>() where T : UnityEngine.Object;
}
