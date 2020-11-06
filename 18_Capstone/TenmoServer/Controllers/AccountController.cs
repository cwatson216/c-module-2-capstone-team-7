﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Parsing.ExpressionVisitors;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountDAO accountDAO;
        private IUserDAO userDAO;

        public AccountController(IAccountDAO accountDAO, IUserDAO userDAO)
        {
            this.accountDAO = accountDAO;
            this.userDAO = userDAO;
        }

        [HttpGet("/hello")]
        public ActionResult Hello()
        {
            return Ok(new { Message = "hello" });
        }


        [HttpGet("user")]
        [Authorize]
        public ActionResult<Account> GetAccount()
        {
            int id = GetUserId();

            Account acc = accountDAO.GetAccount(id);
            if (acc == null)
            {
                return NotFound();
            }
            return acc;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> list = userDAO.GetUsers();

            if (list == null)
            {
                return NotFound();
            }
            return list;
        }

        [HttpGet("transfers")]
        [Authorize]
        public ActionResult<List<Transfer>> GetTransfers()
        {
            int id = GetUserId();

            List<Transfer> list = accountDAO.GetTransfers(id);

            if (list == null)
            {
                return NotFound();
            }
            return list;
        }

        [HttpPut] // transfers/transferId
        [Authorize]
        public ActionResult Transfer(Transfer transfer)
        {
            int int1 = transfer.UserId;
            int int2 = transfer.TransferId;
            decimal dec3 = transfer.Amount;

            accountDAO.Transfer(int1, int2, dec3);
            return Ok();
        }

        [HttpGet("current_user")] // balance - display balance off of account
        [Authorize]
        public ActionResult<decimal> GetBalance()
        {
            int id = GetUserId();

            Account acc = accountDAO.GetAccount(id);
            if (acc == null)
            {
                return NotFound();
            }
            return acc.Balance;
        }
        public int GetUserId()
        {
            System.Security.Claims.Claim claim = User.Claims.Where(c => c.Type == "sub").FirstOrDefault();
            if (claim == null)
            {
                return 0;
            } else
            {
                return Convert.ToInt32(claim.Value);
            }
        }
    }

}
