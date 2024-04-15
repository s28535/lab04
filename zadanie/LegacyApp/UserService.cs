using System;

namespace LegacyApp
{
    public class UserService
    {
        private bool IsDataCorrect(string firstName, string lastName, string email)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            return email.Contains("@") && email.Contains(".");
        }

        public int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }

        private void CalculateCreditLimit(User user)
        {
            var clientType = user.Client.Type;

            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);

                switch (clientType)
                {
                    case "VeryImportantClient":
                        user.HasCreditLimit = false;
                        break;
                    case "ImportantClient":
                        creditLimit = creditLimit * 2;
                        SetCreditLimit(user, creditLimit);
                        break;
                    default:
                        SetCreditLimit(user, creditLimit);
                        break;
                }
            }
        }
        
        private void SetCreditLimit(User user, int creditLimit)
        {
            user.HasCreditLimit = true;
            user.CreditLimit = creditLimit;
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsDataCorrect(firstName, lastName, email))
                return false;

            int age = CalculateAge(dateOfBirth);
            if (age < 21)
                return false;

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            CalculateCreditLimit(user);

            if (user.HasCreditLimit && user.CreditLimit < 500)
                return false;

            UserDataAccess.AddUser(user);
            return true;
        }
    }
}
