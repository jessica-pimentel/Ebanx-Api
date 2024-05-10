using System.Collections.Generic;

public interface IAccountService
{
    void Reset(); 
    int? GetBalance(string accountId); 
    bool Deposit(string destination, int amount); 
    bool Withdraw(string origin, int amount); 
    bool Transfer(string origin, string destination, int amount); 
}