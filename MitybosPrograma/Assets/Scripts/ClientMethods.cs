using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasė skirta naudoti bendravimui su duomenu baze.
/// </summary>
public class ClientMethods : IClientInterface
{
	public ClientMethods() { }

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
    /// <param name="c">DB prisijungimo eilute</param>
    /// <param name="id">Vartotojo grazintas ID</param>
    /// <returns>Vartotojo ID, -1 jeigu nera vartotojo</returns>
    public int Login(string username, string password, string c, out int id)
	{
		id = _databaseMethods.Login(username, password, c, out id);
		return id;
	}

	public void SendSurveyResults()
	{
		new NotImplementedException();
	}


}
