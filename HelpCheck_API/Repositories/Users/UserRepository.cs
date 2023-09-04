using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Dtos.Users;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(FilterUserDto filterUserDto)
        {
            List<User> lists = new List<User>();

            if (filterUserDto == null || string.IsNullOrWhiteSpace(filterUserDto.search))
            {
                var users = _context.UserEntities.Where(w => w.RoleID.Value == 1 || w.RoleID.Value == 2 || w.RoleID == 3 || w.RoleID.Value == 4 || w.RoleID == 6).AsQueryable();
                lists = await users.ToListAsync();
            }
            else
            {
                var users = (from u in _context.UserEntities
                               where u.RoleID != null && (u.RoleID == 1 || u.RoleID == 2 || u.RoleID == 3 || u.RoleID == 4) || u.RoleID == 6 ||
                               !string.IsNullOrWhiteSpace(u.FirstName) && u.FirstName.Contains(filterUserDto.search) ||
                               !string.IsNullOrWhiteSpace(u.LastName) && u.LastName.Contains(filterUserDto.search) ||
                               !string.IsNullOrWhiteSpace(u.IDCard) && u.IDCard.Contains(filterUserDto.search)
                               select u).AsQueryable();
                lists = await users.ToListAsync();
            }
            return lists;
        }

        public async Task<User> GetUserInfoByUserNameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            var user = await _context.UserEntities.Where(u => u.IDCard == username.Replace("-", "").Trim() || u.Email == username.Trim() || u.PhoneNo == username.Replace("-", "").Trim() || u.Hn == username.Trim() || u.UserName == username.Trim())?.FirstOrDefaultAsync();
            if (user is null)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> CheckUserExists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            User user = await _context.UserEntities.Where(u => u.IDCard == username || u.Email == username || u.PhoneNo == username || u.UserName == username)?.FirstOrDefaultAsync<User>();
            if (user is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> CreateUserAsync(User user)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                await _context.UserEntities.AddAsync(user);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Constant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> UpdateUserAsync(User user)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                user.UpdatedDate = DateTime.Now;
                _context.UserEntities.Update(user);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();

                return Constant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<User> GetUserByUserIDAsync(string userId)
        {
            var users = await _context.UserEntities.ToListAsync();

            if (users is null)
            {
                return new User();
            }

            User user = users.Where(u => u.UserID == userId).FirstOrDefault();
            return user;
        }

        public async Task<string> DeleteUserAsync(User user)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                _context.UserEntities.Remove(user);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();

                return Constant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<User> GetUserByAccessTokenAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken)) return new User();

            var users = await _context.UserEntities.ToListAsync();

            if (users is null)
            {
                return new User();
            }

            User user = users.Where(w => w.Token == accessToken)?.FirstOrDefault();
            return user;
        }

        public async Task<User> GetUserByIDAsync(int id)
        {
            return await _context.UserEntities.Where(w => w.ID == id)?.FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.UserEntities.Where(w => !string.IsNullOrWhiteSpace(w.IDCard) && string.IsNullOrEmpty(w.TitleName)).ToListAsync();
        }
    }
}
