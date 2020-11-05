using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountDAO accountDAO;

        public AccountController(IAccountDAO accountDAO)
        {
            this.accountDAO = accountDAO;
        }

        //[HttpGet("balance")]
        //[Authorize]
        //public ActionResult<Account> GetAccount()
        //{
        //    int id = GetUserId();

        //    Account acc = accountDAO.GetAccount(id);
        //    if (acc == null)
        //    {
        //        return NotFound();
        //    }
        //    return acc;
        //}
        [HttpGet("current_user")]
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
