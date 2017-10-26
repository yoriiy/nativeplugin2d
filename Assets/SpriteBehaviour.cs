using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class SpriteBehaviour : MonoBehaviour {
    // NativePlugin <sample-dll>の読み込み
    // CountUp関数を定義
    [DllImport("sample-dll")]
    private static extern int CountUp();
    // カーブ関数を定義
    [DllImport("sample-dll")]
    private static extern float CarvePoint(float X);

    public struct VECTOR3
    {
        public float x;
        public float y;
        public float z;
    };

    // イーズイン関数を定義
    [DllImport("sample-dll")]
    private static extern VECTOR3 EaseIn(VECTOR3 Change, VECTOR3 Base, float Duration, float Time);
    // イーズイン初期化関数を定義
    [DllImport("sample-dll")]
    private static extern void EaseInInit(float Time, VECTOR3 Pos, int Area);
    // イーズイン更新関数を定義
    [DllImport("sample-dll")]
    private static extern int EaseInUpdate();
    // 現在位置
    [DllImport("sample-dll")]
    private static extern VECTOR3 GetNowPos();

    // イーズイン初期化関数を定義
    [DllImport("sample-dll")]
    private static extern void UniMoveInit(float Time, VECTOR3 Pos, int Area);
    // イーズイン更新関数を定義
    [DllImport("sample-dll")]
    private static extern bool UniMoveUpdate();
    // 現在位置
    [DllImport("sample-dll")]
    private static extern VECTOR3 GetUNowPos();

    // 目的地に向かう際の角度
    [DllImport("sample-dll")]
    private static extern float GetMoveRot();

    UniMoveBehaviour uM;
    VECTOR3 pos;
    // Use this for initialization
    void Start () {
        // イーズイン初期化
        //EaseInInit(1.0f, pos, 8);
        UniMoveInit(0.8f, pos, 7);

        uM = GetComponent<UniMoveBehaviour>();

        uM.UniMoveInit(0.8f, transform.position, 7);
        Animator anim = GetComponent<Animator>();
        anim.speed = uM.GetMoveLength() / 5;
    }

    int seq = 0;
    // Update is called once per frame
	void Update () {
        Vector3 npos;
        //int ret = EaseInUpdate();
        //bool ret = UniMoveUpdate();
        bool ret = uM.UniMoveUpdate();
        if (ret)
        {
            Animator anim;
            switch (seq)
            {
                case 0:
                    //UniMoveInit(0.8f, pos, 5);
                    uM.UniMoveInit(0.8f, transform.position, 5);
                    anim = GetComponent<Animator>();
                    anim.speed = uM.GetMoveLength() / 5;
                    break;
                case 1:
                    //UniMoveInit(0.8f, pos, 2);
                    uM.UniMoveInit(0.8f, transform.position, 2);
                    anim = GetComponent<Animator>();
                    anim.speed = uM.GetMoveLength() / 5;
                    break;
            }
            
            seq++;
        }
        // 上下運動開始
        //npos = GetNowPos();
        npos = uM.GetUNowPos();

        transform.position = new Vector3(npos.x, npos.y, npos.z);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, uM.GetMoveRot());
    }
}
