// 暗号化
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crypt
{
    private const string AesIV = @"jCddaOybW3zEh0K1";
    private const string AesKey = @"glVJrbHR1WBDIggF";

    public static string Encrypt(string text)
    {
        // 暗号化
        RijndaelManaged aes = new RijndaelManaged();

        aes.BlockSize = 128;
        aes.KeySize = 128;
        aes.Padding = PaddingMode.Zeros;
        aes.Mode = CipherMode.CBC;
        aes.Key = System.Text.Encoding.UTF8.GetBytes(AesKey);   // 暗号化キー設定
        aes.IV = System.Text.Encoding.UTF8.GetBytes(AesIV);     // 初期化ベクター設定

        // 暗号化のメモリストリーム先生成
        ICryptoTransform encrypt = aes.CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        // 書き込みストリーム設定
        CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

        // バイト数をUTF8形式で取得
        byte[] text_bytes = System.Text.Encoding.UTF8.GetBytes(text);

        // ストリーム書き込み
        cryptStream.Write(text_bytes, 0, text_bytes.Length);
        cryptStream.FlushFinalBlock();  // データ更新

        // 暗号化データ取り出し
        byte[] encrypted = memoryStream.ToArray();
        
        // 暗号化データを返す
        return (System.Convert.ToBase64String(encrypted));
    }

    public static string Decrypt(string cryptText)
    {
        // 暗号化の解除
        RijndaelManaged aes = new RijndaelManaged();
        aes.BlockSize = 128;
        aes.KeySize = 128;
        aes.Padding = PaddingMode.Zeros;
        aes.Mode = CipherMode.CBC;
        aes.Key = System.Text.Encoding.UTF8.GetBytes(AesKey);   // 暗号化キー設定
        aes.IV = System.Text.Encoding.UTF8.GetBytes(AesIV);     // 初期化ベクター設定

        // 復号化のメモリストリーム生成
        ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] encrypted = System.Convert.FromBase64String(cryptText);
        byte[] planeText = new byte[encrypted.Length];
        // メモリストリームにデータ格納
        MemoryStream memoryStream = new MemoryStream(encrypted);
        // 読み込みストリーム設定
        CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        // ストリーム読み込み
        cryptStream.Read(planeText, 0, planeText.Length);

        // 復号化データを返す
        return (System.Text.Encoding.UTF8.GetString(planeText));

    }
}