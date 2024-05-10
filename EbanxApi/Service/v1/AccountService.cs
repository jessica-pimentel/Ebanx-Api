using System.Collections.Generic;

public class AccountService : IAccountService
{
    private readonly Dictionary<string, int> accounts = new Dictionary<string, int>();

    public void Reset() => accounts.Clear();

    public int? GetBalance(string accountId)
    {
        if (string.IsNullOrEmpty(accountId))
            throw new ArgumentException("AccountId is required.");

        accountId = accountId.ToLower();

        if (accounts.ContainsKey(accountId))
            return accounts[accountId];

        return null;
    }

    public bool Deposit(string destination, int amount)
    {
        if (string.IsNullOrEmpty(destination) || amount <= 0)
            return false;

        destination = destination.ToLower();

        if (!accounts.ContainsKey(destination))
            accounts[destination] = amount;
        else
            accounts[destination] += amount;

        return true;
    }

    public bool Withdraw(string origin, int amount)
    {
        if (string.IsNullOrEmpty(origin) || amount <= 0)
            return false;

        origin = origin.ToLower();

        if (!accounts.ContainsKey(origin) || accounts[origin] < amount)
            return false;

        accounts[origin] -= amount;
        return true;
    }

    public bool Transfer(string origin, string destination, int amount)
    {
        if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination) || amount <= 0)
            return false;

        origin = origin.ToLower();
        destination = destination.ToLower();

        if (!accounts.ContainsKey(origin) || accounts[origin] < amount)
            return false;

        if (!accounts.ContainsKey(destination))
            accounts[destination] = 0;

        accounts[origin] -= amount;
        accounts[destination] += amount;

        return true;
    }
}
