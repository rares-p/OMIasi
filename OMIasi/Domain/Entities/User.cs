using System.Net.Mail;
using Domain.Common;
using Domain.Data;

namespace Domain.Entities
{
    public class User : AuditableEntity
    {
        private User(string username, string passwordHash, string firstName, string lastName, string email, UserRole role)
        {
            Id = Guid.NewGuid();
            Username = username;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
        }

        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }

        public static Result<User> Create(string username, string passwordHash, string firstName, string lastName, string email, UserRole role)
        {
	        if (string.IsNullOrWhiteSpace(username))
		        return Result<User>.Failure("Username is required");

			if (string.IsNullOrWhiteSpace(firstName))
                return Result<User>.Failure("First Name is required");

            if (string.IsNullOrWhiteSpace(lastName))
                return Result<User>.Failure("Last Name is required");

            if (string.IsNullOrWhiteSpace(passwordHash))
                return Result<User>.Failure("Last Name is required");

            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new MailAddress(email);
            }
            catch
            {
                return Result<User>.Failure("Invalid email address");
            }

            return Result<User>.Success(new User(username, passwordHash, firstName, lastName, email, role));
        }

        public Result<User> UpdateUsername(string username)
        {
	        if (string.IsNullOrWhiteSpace(username))
		        return Result<User>.Failure("Username is required");
	        Username = username;
	        return Result<User>.Success(this);
        }

		public Result<User> UpdateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result<User>.Failure("First Name is required");
            FirstName = firstName;
            return Result<User>.Success(this);
        }

        public Result<User> UpdateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                return Result<User>.Failure("Last Name is required");
            LastName = lastName;
            return Result<User>.Success(this);
        }

        public Result<User> UpdatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                return Result<User>.Failure("Password is required");
            PasswordHash = passwordHash;
            return Result<User>.Success(this);
        }

        public Result<User> UpdateEmail(string email)
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new MailAddress(email);
            }
            catch
            {
                return Result<User>.Failure("Invalid email address");
            }
            Email = email;
            return Result<User>.Success(this);
        }

        public Result<User> UpdateRole(UserRole role)
        {
            Role = role;
            return Result<User>.Success(this);
        }
    }
}
