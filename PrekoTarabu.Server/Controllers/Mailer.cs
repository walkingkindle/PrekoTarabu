using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrekoTarabu.Server.Exceptions;
using PrekoTarabu.Server.Models;
using PrekoTarabu.Server.Services;

namespace PrekoTarabu.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Mailer : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly MailerService _mailerService;

        private readonly IConfiguration _configuration;

        public Mailer(MailerService mailerService, IConfiguration configuration, AppDbContext dbContext)
        {
            _mailerService = mailerService;
            _configuration = configuration;
            _dbContext = dbContext;
        } 
        


        // POST api/<Mailer>
        //admin receives emails when someone joins the waitlist
        [HttpPost]
        public async Task<ActionResult<Result>> JoinWaitlist([FromBody]WaitLister waitLister)
        {
            var waitListerExists = await _dbContext.WaitListers.AnyAsync(c => c.HisHerMail == waitLister.HisHerMail);

            if (waitListerExists)
            {
                return Result.Failure(WaitListerErrors.UserExists);
            }
            
            _dbContext.WaitListers.Add(waitLister);
            await _dbContext.SaveChangesAsync();
            
            _mailerService.NotifyAdmin(new WaitListRequester()
                { Email = waitLister.HisHerMail, Message = waitLister.HisHerMessage, Name = waitLister.Name });
            _mailerService.ReachOut(new WaitListRequester()
                { Email = waitLister.HisHerMail, Message = waitLister.HisHerMessage, Name = waitLister.Name });

            return Ok(Result.Success());


        }
        


        private bool WaitListerExist(WaitLister waitLister)
        {
            return _dbContext.WaitListers.Find(waitLister.Id) != null;
        }
    }
}
