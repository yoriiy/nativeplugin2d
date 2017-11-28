using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextSave : MonoBehaviour {

    public TextDataManager TdManager;

    // Use this for initialization
    void Start () {
        TdManager = this.transform.GetComponent<TextDataManager>();

        string text = TdManager.ReadText("/pcparam.txt");

        // 改行ごとに文字列を格納
        string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        for (int i = 0; i < lines.Length; i++)
        {
            // 改行時の文字列""(EOF)は無視
            if (lines[i] != "")
            {
                string ifobj = "InputField" + (i + 1);
                // =より後ろの値を取得
                string[] value = lines[i].Split(new string[] { "=" }, StringSplitOptions.None);
                Transform IFObj = GameObject.Find(ifobj).transform;
                InputField tx = IFObj.GetComponent<InputField>();
                // 表示するテキストに設定
                tx.text = value[1].ToString();
                Debug.Log(value[0] + ":" + value[1]);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    ///  データ保存
    /// </summary>
    public void DataSave()
    {
        string save_data = "";

        string text = TdManager.ReadText("/pcparam.txt");

        // 改行ごとに文字列を格納
        string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        for (int i = 0; i < lines.Length; i++)
        {
            // 改行時の文字列""(EOF)は無視
            if (lines[i] != "")
            {
                string ifobj = "InputField" + (i + 1);
                // =より前を識別名　後ろを値として取得
                string[] value = lines[i].Split(new string[] { "=" }, StringSplitOptions.None);
                Transform IFObj = GameObject.Find(ifobj).transform;
                InputField tx = IFObj.GetComponent<InputField>();
                // テキストファイル用に値を文字列で保存
                save_data += value[0] + "=";
                save_data += tx.text + "\r\n";
            }
        }

        // ファイル保存
        TdManager.SaveText("/pcparam.txt", save_data);
    }
}
