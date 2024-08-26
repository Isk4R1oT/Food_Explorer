using Food_Explorer.Entities;

namespace Food_Explorer.Models
{
    public interface IUserBuilder
    {
        IUserBuilder Name(string name);
        IUserBuilder Email(string email);
        IUserBuilder Password(string password);
        User Create();
    }
    public static class UserFactory
    {
        public static User CreateUser(UserType userType)
        {
            switch (userType)
            {
                case UserType.Client:
                    return new Client();
                case UserType.Admin:
                    return new Admin();
                default:
                    throw new ArgumentException($"Invalid UserType: {userType}");
            }
        }
    }
    class UserBuilder : IUserBuilder
    {
        private readonly User _user;
        public UserBuilder(UserType userType)
        {
            _user = UserFactory.CreateUser(userType);
        }

        public User Create()
        {
            return _user;
        }

        public IUserBuilder Email(string email)
        {
            _user.Email = email;
            return this;
        }

        public IUserBuilder Name(string name)
        {
            _user.Name = name;
            return this;
        }

        public IUserBuilder Password(string password)
        {
            _user.Password = password;
            return this;
        }

    }

}
