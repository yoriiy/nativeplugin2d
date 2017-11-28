using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class TextDataManager : MonoBehaviour {

    /// <summary>
    /// ファイル書き込み
    /// </summary>
    /// <param name="Path">ファイルパス</param>
    /// <param name="Text">書き込み文字列</param>
    /// <returns></returns>
	public bool SaveText(string Path, string Text)
    {
        // ファイル書き込み
        try
        {
            using (StreamWriter writer = new StreamWriter(Application.dataPath + Path, false))
            {
                writer.Write(Text); // ファイルstring型書き込み
                writer.Flush();     // ファイル更新
                writer.Close();     // ファイルを閉じる
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }

        return true;
    }

    /// <summary>
    /// ファイル読み込み
    /// </summary>
    /// <param name="Path">ファイルパス</param>
    /// <returns></returns>
    public string ReadText(string Path)
    {
        string strStream = "";

        // ファイル読み込み
        try
        {
            using (StreamReader sr = new StreamReader(Application.dataPath + Path))
            {
                strStream = sr.ReadToEnd(); // ファイルstring型読み込み
                sr.Close(); // ファイルを閉じる
            }
        }catch(Exception e)
        {
            Debug.Log(e.Message);
        }
        return strStream;
    }
}


