using System;


public class ClientMethods : IClientInterface
{
	public ClientMethods()
	{
		private readonly DatabaseMethods _databaseMethods;

		public ClientMethods(DatabaseMethods databaseMethods)
		{
		this._databaseMethods = databaseMethods;
		}
		

	}
}
