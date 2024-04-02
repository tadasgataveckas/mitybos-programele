using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionManager
{
    private const string ENCRYPTION_KEY = "SOME_RANDOM_KEY_IDK";

    public static void StoreIdKey(int id)
    {
        string encrypted_id = EncryptId(id);
        PlayerPrefs.SetString("id_user", encrypted_id);
    }

    public static int GetIdKey()
    {
        string encryptedId = PlayerPrefs.GetString("id_user");
        int id = DecryptId(encryptedId);
        return id;
    }

    private static string EncryptId(int id)
    {
        // TO DO: encryption
        string encrypted_id = id.ToString();

        return encrypted_id;
    }

    private static int DecryptId(string encrypted_id)
    {
        Debug.Log("Encrypted ID is: " + encrypted_id);
        // TO DO: decryption
        int id = int.Parse(encrypted_id.ToString());

        return id;
    } 

}
