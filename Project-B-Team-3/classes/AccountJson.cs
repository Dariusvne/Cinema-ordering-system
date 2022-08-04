using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectB
{
    // An account that is initialized for every member that has an account on the
    // system.
    public class Account
    {
        [JsonPropertyName("Firstname")]
        public string Firstname { get; set; }

        [JsonPropertyName("Lastname")]
        public string Lastname { get; set; }

        [JsonPropertyName("Creditcard")]
        public string Creditcard { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }

    // This class will be used to load and hold the list of accounts on the system
    internal class AccountHandler
    {
        private List<Account> _accounts;
        private string AccountJsonName = @$"Data{Path.DirectorySeparatorChar}Accounts.json";

        public AccountHandler()
        {
            if (File.Exists(AccountJsonName))
            {
                if (new FileInfo(AccountJsonName).Length != 0)
                {
                    Load();
                }
                else
                {
                    _accounts = new List<Account>();
                }
            }
            else
            {
                File.Create(AccountJsonName);
                _accounts = new List<Account>();
            }
        }

        public void Add(string firstname, string lastname, string email, string password, string creditcard)
        {
            Account new_account = new Account();
	        new_account.Firstname = firstname;
	        new_account.Lastname = lastname;
	        new_account.Email = email;
            new_account.Password = password;
            new_account.Creditcard = creditcard;

            _accounts.Add(new_account);
            Save();
        }

        public int Remove(string email)
        {
            Load();
            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].Email == email)
                {
                    _accounts.RemoveAt(i);
                    Save();
                    return i;
                }
            }
            Save();
            return -1;


        }

        public Account Exists(string email, string password)
        {
            foreach (Account account in _accounts)
            {
                if (account.Email.Equals(email))
                {
                    if (account.Password.Equals(password))
                    {
                        return account;
                    }
                }
            }
            return null;
        }

        public bool EmailExists(string email) //true: if account exists, false: if account doesn't exist
        {
            foreach (Account account in _accounts)
            {
                if (account.Email.Equals(email))
                {
                    return true;
                }
            }
            return false;
        }

        public void ChangePassword(string email, string password, string confirmPassword)
        {
            if (PasswordCheck(password,confirmPassword))
            {
                foreach (Account account in _accounts)
                {
                    if (account.Email.Equals(email))
                    {
                        account.Password = password;
                    }
                }
                Save();
            }
        }

        public bool PasswordCheck(string password, string confirmPassword)
        {
            if (password == confirmPassword)
            {
                return true;
            }
            return false;
        }

        // Loads all the existing accounts in the Accounts.json into the accounts list
        private void Load()
        {
            string json = File.ReadAllText(AccountJsonName);
            _accounts = JsonSerializer.Deserialize<List<Account>>(json);
        }

        public void Save()
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            File.WriteAllText(AccountJsonName, JsonSerializer.Serialize(_accounts, options: options));
        }


    }

}
