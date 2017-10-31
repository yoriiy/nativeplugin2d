using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;


public class AutoBehaviour : MonoBehaviour {

    private TextAsset csvFile; 
    private List<string[]> csvDatas = new List<string[]>();

    public AssetsBundleManager assetBundleManager;
    public Image image;

    [SerializeField]
    public class Player
    {
        [SerializeField]
        public int hp;
        public float atk;
        public string name;
        public List<string> items;
        public Player()
        {
            items = new List<string>();
            items.Add("回復薬");
            items.Add("リキュール");
            items.Add("バッテリー");
            hp = 10;
            atk = 100f;
            name = "リード";
        }
    }

    // Use this for initialization
    public IEnumerator Start () {
        /*csvFile = Resources.Load("CSV/test0") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        

        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
        }*/

        // セーブデータ削除
        PlayerPrefs.DeleteKey("score");
        PlayerPrefs.DeleteKey("positionx");
        PlayerPrefs.DeleteKey("text");

        // キーの存在確認
        bool b = PlayerPrefs.HasKey("score");
        if(b)
        {
            b = PlayerPrefs.HasKey("positionx");
            if(b)
            {
                b = PlayerPrefs.HasKey("text");
                if(b)
                {

                }
            }
        }
        // 全データ削除
        //PlayerPrefs.DeleteAll();

        // セーブデータ初期化
        int score = PlayerPrefs.GetInt("score", 0);
        float positionx = PlayerPrefs.GetFloat("positionx", 0.0f);
        string text = PlayerPrefs.GetString("text", "none");

        // セーブデータ保存
        PlayerPrefs.SetInt("score", 1);
        PlayerPrefs.SetFloat("positionx", 3.2f);
        PlayerPrefs.SetString("text", "modify");

        // キーの存在確認
        b = PlayerPrefs.HasKey("score");
        if (b)
        {
            b = PlayerPrefs.HasKey("positionx");
            if (b)
            {
                b = PlayerPrefs.HasKey("text");
                if (b)
                {

                }
            }
        }

        // セーブデータ読み込み
        score = PlayerPrefs.GetInt("score");
        positionx = PlayerPrefs.GetFloat("positionx");
        text = PlayerPrefs.GetString("text");

        // セーブデータの設定
        SaveData.SetInt("i", 100);
        SaveData.SetClass<Player>("p1", new Player());
        SaveData.Save();

        Player getPlayer = SaveData.GetClass<Player>("p1", new Player());

        Debug.Log("int i = " + SaveData.GetInt("i"));
        Debug.Log(getPlayer.name);
        Debug.Log(getPlayer.items.Count + "個");
        Debug.Log(getPlayer.items[0] + getPlayer.items[1] + getPlayer.items[2]);

        // 新しいゲームオブジェクトを作成
        GameObject kinG = Instantiate(GameObject.Find("Kingyo_S"));
        // Kingyo_Sを親として子に登録
        GameObject.Find("Kingyo_S").transform.parent = kinG.transform;
        // 座標設定
        kinG.transform.position = new Vector3(-5, 4, 0);

        // スプライトをテクスチャ名を決めて作成
        //Sprite spriteImage = Resources.Load("kingyo", typeof(Sprite)) as Sprite;
        // 新規にゲームオブジェクトとして登録
        //new GameObject("Sprite").AddComponent<SpriteRenderer>().sprite = spriteImage;

        // アセットバンドルのロード
        yield return StartCoroutine(assetBundleManager.LoadAssetBundleCoroutine());
        // アセットバンドル内のテクスチャをスプライトに設定
        image.sprite = assetBundleManager.GetSpriteFromAssetBundle("cat");
    }



    // Update is called once per frame
    void Update () {

        // タッチの状態取得
        TouchInfo info = AppUtil.GetTouch();
        // タッチ開始状態
        if( info == TouchInfo.Began)
        {

        }
	}
}
