
public static class Configuration2y
{
    // Static fields
    private static BankAccount account;
    private static string apiKey;
    private static int maxConnections;

    // Static constructor to initialize the fields
    static Configuration2y()
    {
        apiKey = "your_api_key";
        maxConnections = 10;
    }

    // Static properties
    public static string ApiKey
    {
        get { return apiKey; }
        set { apiKey = value; }
    }


    public static int MaxConnections
    {
        get { return maxConnections; }
        set { maxConnections = value; }
    }

    // Method with parameters to set API key
    public static void SetApiKey(string newApiKey)
    {
        apiKey = newApiKey;
    }

    public static void SetBankAccount(BankAccount bankAccount) => account = bankAccount;

    // Method with parameters to set max connections
    public static void SetMaxConnections(int newMaxConnections)
    {
        maxConnections = newMaxConnections;
    }

    // Static methods
    public static void DisplayConfig()
    {
        Console.WriteLine($"API Key: {apiKey}");
        Console.WriteLine($"Max Connections: {maxConnections}");
    }
}
