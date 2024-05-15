using System.Collections.Generic;

/// <summary>
/// Klasė skirta naudoti bendravimui su duomenu baze.
/// </summary>
public class ClientMethods// : IClientInterface
{
	private readonly DatabaseMethods _databaseMethods;

	public ClientMethods(DatabaseMethods databaseMethods)
	{
		this._databaseMethods = databaseMethods;
	}

   /// <summary>
   /// Login confirmation methid
   /// </summary>
   /// <param name="username">Account username</param>
   /// <param name="password">Account password</param>
   /// <returns>Returns id if successful, otherwise returns -1</returns>
    public int Login(string username, string password)
	{
        return _databaseMethods.Login(username, password);
	}

    /// <summary>
    /// Register user in database
    /// </summary>
    /// <param name="email">Account email</param>
    /// <param name="username">Account username</param>
    /// <param name="password">Account password</param>
    /// <returns>If user account was created successfully</returns>
    public bool RegisterUser(string email, string username, string password)
    {
        return _databaseMethods.RegisterUser(email, username, password);
    }

    public void InsertUserData(UserData userData)
    {
        _databaseMethods.InsertUserData(userData);
    }

    public void UpdateUserData(UserData userData)
    {
        _databaseMethods.UpdateUserData(userData);
    }

    public bool CheckIfSurveyCompleted(int id)
    {
        return _databaseMethods.CheckIfSurveyCompleted(id);
    }

    public string ReturnUsername(int id)
    {
        return _databaseMethods.ReturnUsername(id); ;
    }

    public List<FoodClass> ReturnFoodList()
    {
        return _databaseMethods.ReturnFoodList();
    }

    public bool IsUsernameTaken(string username)
    {
        return _databaseMethods.IsUsernameTaken(username);
    }

    public bool IsEmailInUse(string email)
    {
        return _databaseMethods.IsEmailInUse(email);
    }

    public bool IsPasswordCorrect(string username, string email)
    {
        return _databaseMethods.IsPasswordCorrect(username, email);
    }

    public bool InsertUserLevelCoins(int id_user, int level, int xp, int coins)
    {
        return _databaseMethods.InsertUserLevelCoins(id_user, level, xp, coins);
    }

    public bool UpdateUserLevelCoins(int id_user, int level, int xp, int coins)
    {
        return _databaseMethods.UpdateUserLevelCoins(id_user, level, xp, coins);
    }

    public bool UpdateUserXp(int id_user, int xp)
    {
        return _databaseMethods.UpdateUserXp(id_user, xp);
    }

    public bool UpdateUserCoins(int id_user, int coins)
    {
        return _databaseMethods.UpdateUserCoins(id_user, coins);
    }

    public bool UpdateUserLevel(int id_user, int level)
    {
        return _databaseMethods.UpdateUserLevel(id_user, level);
    }

    public bool InsertUserAllergy(int id_user, int id_allergy)
    {
        return _databaseMethods.InsertUserAllergy(id_user, id_allergy);
    }

    public List<int> GetAllUserAllergies(int id_user)
    {
        return _databaseMethods.GetAllUserAllergies(id_user);
    }

    public bool DeleteUserAllergies(int id_user)
    {
        return _databaseMethods.DeleteUserAllergies(id_user);
    }
}
