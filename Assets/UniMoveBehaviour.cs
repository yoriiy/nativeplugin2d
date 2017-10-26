using UnityEngine;
using System.Collections;
using System;

public class UniMoveBehaviour : MonoBehaviour {

    // ラジアンから角度へ変換
    float myRadiantoDegree(float deg, float rad)
    {
        // ラジアンを角度に変換
        rad = (rad / 3.141592653589793f) * 180.0f;

        // 数値の繰り下げ
        deg = (float)Math.Floor(rad);
        // 小数点を四捨五入する
        if ((rad - deg) >= 0.5) deg++;

        return deg;
    }

    // 区域座標配列
    Vector3[] AreaPos =
    {
        new Vector3( 4.0f, 2.0f,0.0f),
        new Vector3( 0.0f, 2.0f,0.0f ),
        new Vector3( 4.0f, 2.0f,0.0f ),
        new Vector3(-4.0f, 0.0f,0.0f ),
        new Vector3( 0.0f, 0.0f,0.0f ),
        new Vector3( 4.0f, 0.0f,0.0f ),
        new Vector3(-4.0f,-2.0f,0.0f ),
        new Vector3( 0.0f,-2.0f,0.0f ),
        new Vector3( 4.0f,-2.0f,0.0f )
    };

    float g_UTime;
    int g_USplit;
    int g_USplitCount;
    Vector3 UTargetPos;
    Vector3 UBasePos;
    Vector3 UNowPos;

    // Use this for initialization
    void Start () {
    }

    /* 等速移動初期化関数
	//	@param	float	Time	アクション再生時間(1.0f=１秒)
	//	@param	VECTOR3	Pos		開始座標
	//	@param	int		Area	移動エリア
	//================================*/
    internal void UniMoveInit(float Time, Vector3 Pos, int Area)
    {
        g_UTime = Time;
        // フレーム時間が０ならば
        // 1フレームでの瞬時処理
        if (g_UTime == 0)
        {
            g_USplit = 1;   // 分割数を設定
        }
        else
        {
            g_USplit = (int)(60 * g_UTime);    // 分割数を設定
        }
        g_USplitCount = 0;
        // アクションで使用するオブジェクトの座標を設定
        UTargetPos = AreaPos[Area];
        UTargetPos.x += 0.5f * (UnityEngine.Random.value * 10 % 10) - 2.5f;
        UTargetPos.y += 0.25f * (UnityEngine.Random.value * 10 % 10) - 1.25f;
        UBasePos = Pos;
        UNowPos = UBasePos;
    }

    // Update is called once per frame
    void Update () {
	
	}


    /* 等速移動更新関数
	// @return int	終了判定
	//===========================*/
    internal bool UniMoveUpdate()
    {
        bool ret = false;
        // 分割数分で処理を回す
        if (g_USplitCount < g_USplit)
        {
            // イーズインで座標を更新
            UNowPos += ((UTargetPos - UBasePos) / g_USplit);

            g_USplitCount++;    // カウントを進める
        }
        else
        {
            ret = true;     // 処理の終わり
        }
        return ret;
    }


    /* アクセサ関数
	// @return	Vector3	現在位置取得
	//===========================*/
    internal Vector3 GetUNowPos()
    {
        return UNowPos;
    }


    /* 目的地に向かう際の角度
	//
	//===========================*/
    internal float GetMoveRot()
    {
        float rad = (float)Math.Atan2(UBasePos.x - UTargetPos.x, UTargetPos.y - UBasePos.y);
        float deg = 0.0f;
        deg = myRadiantoDegree(deg, rad);
        return deg;
    }

    /* 目的地までの長さ
    //
    //
    //==========================*/
    internal float GetMoveLength()
    {
        float len = Vector3.Distance(UTargetPos,UBasePos);
        return len; 
    }
}
