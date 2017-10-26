using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Texture_Blur {// : MonoBehaviour {

    // Linq Dictionary型の２次配列テクスチャ２D

    private static Dictionary<Texture2D, Texture2D> _textureChache = new Dictionary<Texture2D, Texture2D>();
	
    public static Texture2D CreateBlurTexture(Texture2D tex, float sig, bool isCache = true)
    {
        // 既にテクスチャキャッシュにデータが入っていたらそのまま返す
        if (isCache && _textureChache.ContainsKey(tex)) return _textureChache[tex];

        int W = tex.width;
        int H = tex.height;
        int Wm = (int)(Mathf.Ceil(3.0f * sig) * 2 + 1);
        int Rm = (Wm - 1) / 2;

        // フィルタ
        float[] msk = new float[Wm];

        sig = 2 * sig * sig;
        float div = Mathf.Sqrt(sig * Mathf.PI);

        // フィルタの作成
        for(int x = 0; x < Wm; x++)
        {
            int p = (x - Rm) * (x - Rm);
            msk[x] = Mathf.Exp(-p / sig) / div;
        }

        var src = tex.GetPixels(0).Select(x => x.a).ToArray();
        var tmp = new float[src.Length];
        var dst = new Color[src.Length];

        // 上下方向
        for(int x = 0; x < W; x++)
        {
            for(int y = 0; y < H; y++)
            {
                float sum = 0;
                for(int i = 0; i < Wm; i++)
                {
                    int p = y + i - Rm;
                    if (p < 0 || p >= H) continue;
                    sum += msk[i];// * src[x + p * W];
                }
                tmp[x + y * W] = sum;
            }
        }
        // 左右方向
        for(int x = 0; x < W; x++)
        {
            for (int y = 0; y < H; y++)
            {
                float sum = 0;
                for (int i = 0; i < Wm; i++)
                {
                    int p = x + i - Rm;
                    if (p < 0 || p >= W) continue;
                    sum += msk[i];// * tmp[p + y * W];
                }
                dst[x + y * W] = new Color(1, 1, 1, sum);
            }
        }
        var createTexture = new Texture2D(W, H);
        createTexture.SetPixels(dst);
        createTexture.Apply();

        if(isCache)
        {
            _textureChache.Add(tex, createTexture);
        }

        return createTexture;
    }


    // テクスチャキャッシュの廃棄
    public static void Release()
    {
        _textureChache.Clear();
    }
}
