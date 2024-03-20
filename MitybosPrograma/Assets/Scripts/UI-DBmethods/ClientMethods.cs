using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasė skirta naudoti bendravimui su duomenu baze.
/// </summary>
public class ClientMethods : IClientInterface
{
	private readonly DatabaseMethods _databaseMethods;

	public ClientMethods(DatabaseMethods databaseMethods)
	{
		this._databaseMethods = databaseMethods;
	}

    /// <summary>
    /// Prisijungimo patvirtinimo metodas
    /// </summary>
    /// <param name="username">Vartotojo ivestas vardas UI elemente</param>
    /// <param name="password">Vartotojo ivestas slaptazodis UI elemente</param>
    /// <param name="constring">DB prisijungimo eilute</param>
    /// <param name="id">Vartotojo grazintas ID</param>
    /// <returns>Vartotojo ID, -1 jeigu nera vartotojo</returns>
    public int Login(string username, string password, out int id, string constring)
	{
		id = _databaseMethods.Login(username, password, out id, constring);
		return id;
	}
    /// <summary>
    /// Naujo vartotojo registracijos metodas
    /// </summary>
    /// <param name="email">Vartotojo pateiktas el. pastas</param>
    /// <param name="name">Vartotojo prisijungimo vardas</param>
    /// <param name="password">Vartotojo slaptažodis</param>
    /// <param name="constring">DB prisijungimo eilute</param>
    /// <returns>True, jei pridėtas naujas irašas. False kitais atvejais</returns>
    public bool Register(string email, string name, string password, string constring)
    {
        return _databaseMethods.Register(email, name, password, constring);
    }

    public void InsertRegisterPlaceholder(int id, string constring)
    {
        _databaseMethods.InsertRegisterPlaceholder(id, constring);
    }
    public void UpdateProfile(int id, string gender, double height, double weight, string goal, int dateOfBirth, int activity, string constring)
    {
        _databaseMethods.UpdateProfile(id, gender, height, weight, goal,dateOfBirth,activity, constring);
    }

    public bool CheckSurveyCompleted(int id, string constring)
    {
        return _databaseMethods.CheckSurveyCompleted(id, constring);
    }

    public string ReturnUserData(int id, string constring)
    {
        return _databaseMethods.ReturnUserData(id, constring); ;
    }
}
