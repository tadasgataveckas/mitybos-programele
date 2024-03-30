using Mono.Data.Sqlite;
using System;
using System.Data;
using System.Text;
using UnityEngine;

public class Z___TEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DBManager.CreateDatabase();
        //ClientMethods clientMethods = new ClientMethods(new DatabaseMethods());



    }

    // Update is called once per frame
    void Update()
    {

    }
}
