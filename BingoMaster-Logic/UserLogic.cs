using AutoMapper;
using BingoMaster_Entities;
using BingoMaster_Logic.Exceptions;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoMaster_Logic
{
    public class UserLogic : IUserLogic
    {
        #region Fields

        private readonly BingoMasterDbContext _context;
        private readonly IPasswordLogic _passwordLogic;
        private readonly IPlayerLogic _playerLogic;
        private readonly ITokenLogic _tokenLogic;
        private readonly IMapper _mapper;

        #endregion

        public UserLogic(BingoMasterDbContext context, IPasswordLogic passwordLogic, IPlayerLogic playerLogic, ITokenLogic tokenLogic, IMapper mapper)
        {
            _context = context;
            _passwordLogic = passwordLogic;
            _playerLogic = playerLogic;
            _tokenLogic = tokenLogic;
            _mapper = mapper;
        }

        public AuthenticatedUserModel Authenticate(AuthenticateUserModel authenticateUserModel)
        {
            if (!MinimalRequiredAuthenticationInformationProvided(authenticateUserModel))
            {
                throw new ArgumentException("No email address or password provided");
            }

            var user = GetUserByEmailAddress(authenticateUserModel.EmailAddress);

            if (user == null)
            {
                return null;
            }

            if (!_passwordLogic.VerifyPassword(authenticateUserModel.Password, user.Hash, user.Salt))
            {
                throw new Exception("Login failed");
            }

            var model = _mapper.Map<AuthenticatedUserModel>(user);
            model.Token = _tokenLogic.GenerateToken(user);

            return model;
        }

        public UserModel GetUserById(Guid id)
        {
            var user = _context.Users.Find(id);

            return _mapper.Map<UserModel>(user);
        }

        public UserModel Register(RegisterUserModel registerUserModel)
        {
            if (!MinimalRequiredRegistrationInformationProvided(registerUserModel))
            {
                throw new ArgumentException("No email address, playername or password provided");
            }

            if (!_playerLogic.PlayerNameUnique(registerUserModel.PlayerName) || !EmailAddressUnique(registerUserModel.EmailAddress))
            {
                throw new UserAlreadyExistsException("Player or user already exists");
            }

            var passwordStrength = _passwordLogic.GetPasswordStrength(registerUserModel.Password);

            if (passwordStrength != PasswordStrength.Strong && passwordStrength != PasswordStrength.VeryStrong)
            {
                throw new ArgumentException("Provided password too weak");
            }

            var newUser = ConstructNewUserWithPassword(registerUserModel);

            _context.Add(newUser);
            _context.SaveChanges();

            return _mapper.Map<UserModel>(newUser);
        }

        public bool EmailAddressUnique(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                throw new ArgumentException("No email address provided");
            }

            var user = _context.Users.FirstOrDefault(user => user.EmailAddress == emailAddress);

            if (user != null)
            {
                return false;
            }

            return true;
        }

        public void Update(UserModel userModel)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == userModel.Id);

            if (user == null)
            {
                throw new KeyNotFoundException("Entity does not exists");
            }

            _context.Entry(user).CurrentValues.SetValues(userModel);
            _context.SaveChanges();
        }

        private User ConstructNewUserWithPassword(RegisterUserModel registerUserModel)
        {
            var salt = _passwordLogic.GetRandomSalt();
            var hashedPassword = _passwordLogic.GetHashedPassword(registerUserModel.Password, salt);

            return new User
            {
                EmailAddress = registerUserModel.EmailAddress,
                FirstName = registerUserModel.FirstName,
                LastName = registerUserModel.LastName,
                Salt = Convert.ToBase64String(salt),
                Hash = hashedPassword,
                Player = new Player
                {
                    Name = registerUserModel.PlayerName
                }
            };
        }

        private bool MinimalRequiredRegistrationInformationProvided(RegisterUserModel registerUserModel)
        {
            return registerUserModel != null
                && !string.IsNullOrWhiteSpace(registerUserModel.EmailAddress)
                && !string.IsNullOrWhiteSpace(registerUserModel.Password)
                && !string.IsNullOrWhiteSpace(registerUserModel.PlayerName);
        }

        private bool MinimalRequiredAuthenticationInformationProvided(AuthenticateUserModel authenticateUserModel)
        {
            return authenticateUserModel != null && !string.IsNullOrWhiteSpace(authenticateUserModel.EmailAddress) && !string.IsNullOrWhiteSpace(authenticateUserModel.Password);
        }

        private User GetUserByEmailAddress(string emailAddress)
        {
            return _context.Users
                .Include(user => user.Player)
                .FirstOrDefault(user => user.EmailAddress == emailAddress);
        }
    }
}
