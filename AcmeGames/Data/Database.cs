﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AcmeGames.Models;
using Newtonsoft.Json;
using System.Linq;
using AcmeGames.Interfaces;
using AcmeGames.ViewModels;

namespace AcmeGames.Data
{
	public class Database : IDatabase
	{
        private static readonly Random          locRandom       = new Random();

		private static IEnumerable<Game>		locGames		= new List<Game>();
		private static IEnumerable<GameKey>		locKeys			= new List<GameKey>();
		private static IEnumerable<Ownership>	locOwnership	= new List<Ownership>();
		private static IEnumerable<User>		locUsers		= new List<User>();

	    public Database()
		{
			locGames		= JsonConvert.DeserializeObject<IEnumerable<Game>>(File.ReadAllText(@"Data\games.json"));
			locKeys			= JsonConvert.DeserializeObject<IEnumerable<GameKey>>(File.ReadAllText(@"Data\keys.json"));
			locUsers		= JsonConvert.DeserializeObject<IEnumerable<User>>(File.ReadAllText(@"Data\users.json"));
			locOwnership	= JsonConvert.DeserializeObject<IEnumerable<Ownership>>(File.ReadAllText(@"Data\ownership.json"));
		}

	    // NOTE: This accessor function must be used to access the data.
	    private Task<IEnumerable<T>>PrivGetData<T>(IEnumerable<T>  aDataSource)
	    {
	        var delay = locRandom.Next(150, 1000);
            Thread.Sleep(TimeSpan.FromMilliseconds(delay));

	        return Task.FromResult(aDataSource);
	    }

        public User FindUser(string email, string password)
        {   
            return PrivGetData(locUsers).Result.
                SingleOrDefault(user => user.EmailAddress == email && user.Password == password);
        }

        public User GetUserData(string accountId)
        {
            return PrivGetData(locUsers).Result.
                SingleOrDefault(user => user.UserAccountId == accountId);
        }

        public List<Ownership> FindOwnership(string userAccountId)
        {
            return PrivGetData(locOwnership).Result
                .Where(ownership => ownership.UserAccountId == userAccountId)
                .ToList();
        }

        public List<GameListItem> FindOwned(string userAccountId)
        {
            var listA = PrivGetData(locGames).Result.ToList();
            var listB = PrivGetData(locOwnership).Result.ToList();

            return (from a in listB
                    where a.UserAccountId == userAccountId
                    join b in listA on a.GameId equals b.GameId
                    select new GameListItem { Game = b.Name, Registered = a.RegisteredDate, Thumb = b.Thumbnail }).ToList();
        }

        public Game FindGame(uint gameId)
        {
            return PrivGetData(locGames).Result
                .FirstOrDefault(game => game.GameId == gameId);
        }

        public void UpdateUserData(UpdateUserDataViewModel vm)
        {
            var user = PrivGetData(locUsers).Result.
                SingleOrDefault(x => x.UserAccountId == vm.UserAccountId);
            user.Update(vm);
        }

        public bool CheckPassword(string account, string password)
        {
            var user = PrivGetData(locUsers).Result.
                SingleOrDefault(x => x.UserAccountId == account);

            return (user.Password.Equals(password)) ? true : false;
        }

        public GameKey FindGameKey(string key)
        {
            return PrivGetData(locKeys).Result.
                SingleOrDefault(gameKey => gameKey.Key == key && gameKey.IsRedeemed == false);
        }

        public void NewOwnership(uint gameId, string date, OwnershipState state, string accountId)
        {
            var newOwnershipId = (uint) (locOwnership.Count() +1);
            var newOwnership = new Ownership(gameId, newOwnershipId, date, state, accountId);
            var newLocOwnership = locOwnership.ToList();
            newLocOwnership.Add(newOwnership);
            locOwnership = newLocOwnership;
        }
    }
}
