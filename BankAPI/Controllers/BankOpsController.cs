using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankOpsController : ControllerBase
    {
        private double balance = 1000;
        private const double limit = 10000;
        private const double minimum = 100;
        private const double maxWithdrawPercentage = 0.9;

        [HttpGet]
        public ActionResult<double> Get() { return balance; }

        [HttpPost("deposit")]
        public ActionResult<double> deposit([FromBody] double val)
        {
            if (val <= 0) { return BadRequest("You entered 0 didn't you? Or at least something below that"); }

            if (val > limit) { return BadRequest("Cannot deposit more than 10,000$ at a time"); }

            if (val <= limit) 
            { 
                balance+=val;
                return balance;
            }

            else { return BadRequest("I'm sorry but you cannot enter what you just did, you know why, (only numbers please)"); }
        }
        [HttpPost("withdraw")]
        public ActionResult<double> withdraw([FromBody] double val)
        {
            if (val <= 0) { return BadRequest("Withdrawal amount must be a positive number"); }

            if (val > balance) { return BadRequest("Cannot withdraw more than the available balance"); }

            double maxAmount = balance * maxWithdrawPercentage;
            if (val > maxAmount) { return BadRequest("Cannot withdraw more than 90% of the total balance in a single transaction"); }

            if (balance - val < minimum) { return BadRequest("Cannot withdraw, balance will be below 100$"); }

            balance -= val;
            return balance;
        }
    }
}