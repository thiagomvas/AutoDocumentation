public class BankAccount
{
    // Fields
    private string accountNumber;
    private string accountHolder;
    private double balance;

    // Constructor
    public BankAccount(string accountNumber, string accountHolder, double initialBalance)
    {
        this.accountNumber = accountNumber;
        this.accountHolder = accountHolder;
        this.balance = initialBalance;
    }

    // Methods
    public void Deposit(double amount)
    {
        if (amount > 0)
        {
            balance += amount;
            Console.WriteLine($"Deposited {amount:C}. New balance: {balance:C}");
        }
        else
        {
            Console.WriteLine("Invalid deposit amount.");
        }
    }

    public void Withdraw(double amount)
    {
        if (amount > 0 && balance >= amount)
        {
            balance -= amount;
            Console.WriteLine($"Withdrawn {amount:C}. New balance: {balance:C}");
        }
        else
        {
            Console.WriteLine("Invalid withdrawal amount or insufficient balance.");
        }
    }

    public void TransferTo(BankAccount destinationAccount, double amount)
    {
        if (amount > 0 && balance >= amount)
        {
            balance -= amount;
            destinationAccount.Deposit(amount);
            Console.WriteLine($"Transferred {amount:C} to {destinationAccount.accountHolder}. New balance: {balance:C}");
        }
        else
        {
            Console.WriteLine("Invalid transfer amount or insufficient balance.");
        }
    }

    public void DisplayAccountInfo()
    {
        Console.WriteLine($"Account Number: {accountNumber}");
        Console.WriteLine($"Account Holder: {accountHolder}");
        Console.WriteLine($"Balance: {balance:C}");
    }


}
