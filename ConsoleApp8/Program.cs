using System;
using System.Linq;

class Program
{
    static List<User> users = new List<User>();

    static void Main(string[] args)
    {
        InitializeUsers();

        while (true)
        {
            Console.Write("PIN kodunuzu giriniz: ");
            string enteredPin = Console.ReadLine();

            User user = AuthenticateUser(enteredPin);

            if (user != null)
            {
                Console.WriteLine($"{user.Name} {user.Surname} xos gelmisiniz!");
                ShowMenu(user);
            }
            else
            {
                Console.WriteLine("Bu PIN koduna aid kart tapilmadi.");
            }
        }
    }

    static void InitializeUsers()
    {
        User user1 = new User("Ramiz", "Atakisiyev", new Card("1111", "1234", "123", "06/23", 1000.0m));
        User user2 = new User("Hikmet", "Humbetli", new Card("2222", "5678", "456", "12/24", 2500.0m));
        User user3 = new User("Telman", "Ismayilov", new Card("3333", "9876", "789", "09/23", 500.0m));
        User user4 = new User("Rehim", "Balayev", new Card("4444", "4321", "654", "03/25", 1500.0m));
        User user5 = new User("Ismayil", "Alakbarov", new Card("5555", "8765", "321", "08/22", 2000.0m));

        users.AddRange(new User[] { user1, user2, user3, user4, user5 });
    }

    static User AuthenticateUser(string enteredPin)
    {
        return users.FirstOrDefault(user => user.CreditCard.Pin == enteredPin);
    }

    static void ShowMenu(User user)
    {
        while (true)
        {
            Console.WriteLine("Lütfen bir işlem seçiniz:");
            Console.WriteLine("1. Balans");
            Console.WriteLine("2. nagd pul cekme ");
            Console.WriteLine("3. History");
            Console.WriteLine("4. Karttan Karta pul Transferi");
            Console.WriteLine("5. cixis");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine($"Kartınızın bakiyesi: {user.CreditCard.Balance} AZN");
                    break;
                case 2:
                    WithdrawCash(user);
                    break;
                case 3:
                    ShowTransactionHistory(user);
                    break;
                case 4:
                    TransferMoney(user);
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("error yeniden yoxlayin.");
                    break;
            }
        }
    }

    static void WithdrawCash(User user)
    {
        Console.WriteLine("cixarmaq istediyiniz miqdari yazin: ");
        decimal amount = decimal.Parse(Console.ReadLine());

        if (user.CreditCard.Balance >= amount)
        {
            user.CreditCard.Balance -= amount;
            Console.WriteLine($"{amount} AZN cixarildi. Yeni balans: {user.CreditCard.Balance} AZN");
        }
        else
        {
            Console.WriteLine("Balansinizda yeteri qedr pul yoxdur");
        }
    }

    static void ShowTransactionHistory(User user)
    {
        Console.WriteLine("tarixce:");
        foreach (var operation in user.Operations)
        {
            Console.WriteLine($"Tarix: {operation.Date}, cekme: {operation.Description}, Miqdar: {operation.Amount} AZN");
        }
    }

    static void TransferMoney(User user)
    {
        Console.Write("Kart nomnresini yazin: ");
        string targetCardNumber = Console.ReadLine();

        User targetUser = users.FirstOrDefault(u => u.CreditCard.Pan == targetCardNumber);

        if (targetUser != null)
        {
            Console.WriteLine($"Hedef istifadeci: {targetUser.Name} {targetUser.Surname}");
            Console.Write("Transfer etmek istediyiniz miqdari: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            if (user.CreditCard.Balance >= amount)
            {
                user.CreditCard.Balance -= amount;
                targetUser.CreditCard.Balance += amount;
                Console.WriteLine($"{amount} AZN transfer edildi. Yeni balans: {user.CreditCard.Balance} AZN");
            }
            else
            {
                Console.WriteLine("Balansinizda yeteri qedr pul yoxdur.");
            }
        }
        else
        {
            Console.WriteLine("Hedef kart tapilmadi , xahis olunur bir daha yoxlayin.");
        }
    }
}

class Card
{
    public string Pan { get; }
    public string Pin { get; }
    public string Cvc { get; }
    public string ExpireDate { get; }
    public decimal Balance { get; set; }

    public Card(string pan, string pin, string cvc, string expireDate, decimal balance)
    {
        Pan = pan;
        Pin = pin;
        Cvc = cvc;
        ExpireDate = expireDate;
        Balance = balance;
    }
}

class User
{
    public string Name { get; }
    public string Surname { get; }
    public Card CreditCard { get; }
    public List<Transaction> Operations { get; } = new List<Transaction>();

    public User(string name, string surname, Card creditCard)
    {
        Name = name;
        Surname = surname;
        CreditCard = creditCard;
    }
}

class Transaction
{
    public DateTime Date { get; }
    public string Description { get; }
    public decimal Amount { get; }

    public Transaction(string description, decimal amount)
    {
        Date = DateTime.Now;
        Description = description;
        Amount = amount;
    }
}
