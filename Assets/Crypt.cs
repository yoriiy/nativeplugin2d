// �Í���
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
        // �Í���
        RijndaelManaged aes = new RijndaelManaged();

        aes.BlockSize = 128;
        aes.KeySize = 128;
        aes.Padding = PaddingMode.Zeros;
        aes.Mode = CipherMode.CBC;
        aes.Key = System.Text.Encoding.UTF8.GetBytes(AesKey);   // �Í����L�[�ݒ�
        aes.IV = System.Text.Encoding.UTF8.GetBytes(AesIV);     // �������x�N�^�[�ݒ�

        // �Í����̃������X�g���[���搶��
        ICryptoTransform encrypt = aes.CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        // �������݃X�g���[���ݒ�
        CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

        // �o�C�g����UTF8�`���Ŏ擾
        byte[] text_bytes = System.Text.Encoding.UTF8.GetBytes(text);

        // �X�g���[����������
        cryptStream.Write(text_bytes, 0, text_bytes.Length);
        cryptStream.FlushFinalBlock();  // �f�[�^�X�V

        // �Í����f�[�^���o��
        byte[] encrypted = memoryStream.ToArray();
        
        // �Í����f�[�^��Ԃ�
        return (System.Convert.ToBase64String(encrypted));
    }

    public static string Decrypt(string cryptText)
    {
        // �Í����̉���
        RijndaelManaged aes = new RijndaelManaged();
        aes.BlockSize = 128;
        aes.KeySize = 128;
        aes.Padding = PaddingMode.Zeros;
        aes.Mode = CipherMode.CBC;
        aes.Key = System.Text.Encoding.UTF8.GetBytes(AesKey);   // �Í����L�[�ݒ�
        aes.IV = System.Text.Encoding.UTF8.GetBytes(AesIV);     // �������x�N�^�[�ݒ�

        // �������̃������X�g���[������
        ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] encrypted = System.Convert.FromBase64String(cryptText);
        byte[] planeText = new byte[encrypted.Length];
        // �������X�g���[���Ƀf�[�^�i�[
        MemoryStream memoryStream = new MemoryStream(encrypted);
        // �ǂݍ��݃X�g���[���ݒ�
        CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        // �X�g���[���ǂݍ���
        cryptStream.Read(planeText, 0, planeText.Length);

        // �������f�[�^��Ԃ�
        return (System.Text.Encoding.UTF8.GetString(planeText));

    }
}