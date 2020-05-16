﻿using SikkimGov.Platform.Models.DomainModels;

namespace SikkimGov.Platform.DataAccess.Repositories.Contracts
{
    public interface IUserRepository
    {
        User GetUserByUserName(string userName);

        bool IsUserExists(string userName);

        User SaveUser(User user);

        void DeleteUser(long userId);

        void DeleteUserByEmailId(string emailId);
    }
}
