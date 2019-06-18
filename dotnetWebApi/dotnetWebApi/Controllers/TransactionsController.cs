using System;
using System.Collections.Generic;
using System.Linq;
using dotnetWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace dotnetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private AppDataBaseContext _ctx;
        public TransactionsController(AppDataBaseContext ctx)
        {
            _ctx = ctx;
        }

        // GET /api/transactions
        [Authorize]
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public List<TransactionExt> Get()
        {
            User user = _ctx.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            return _ctx.Transactions.Where(x => x.OwnerUserId == user.Id).Union(_ctx.Transactions.Where(x => x.TargetUserId == user.Id))
                .Join(_ctx.Users,
                    transaction => transaction.TargetUserId,
                    u => u.Id,
                    (transaction, u) =>
                         new TransactionExt
                         {
                             Id = transaction.Id,
                             Date = transaction.Date,
                             OwnerUserId = transaction.OwnerUserId,
                             TargetUserId = transaction.TargetUserId,
                             TargetUserName = u.Username,
                             TargetUserBalance = u.Balance,
                             Amount = transaction.Amount,
                             Balance = transaction.Balance

                         }).ToList();
        }

        // POST api/transactions
        [Authorize]
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public Transaction Post(Transaction transaction)
        {
            User user = _ctx.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            User targetUser = _ctx.Users.FirstOrDefault(x => x.Id == transaction.TargetUserId);
            
            transaction.OwnerUserId = user.Id;
            transaction.Date = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            transaction.Balance = user.Balance - transaction.Amount;
         
            user.Balance = transaction.Balance;
            targetUser.Balance += transaction.Amount;

            _ctx.Transactions.Add(transaction);
            _ctx.Users.Update(user);
            _ctx.Users.Update(targetUser);
            _ctx.SaveChanges();

            return transaction;
        }
    }
}