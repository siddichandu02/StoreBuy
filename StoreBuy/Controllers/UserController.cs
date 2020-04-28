using StoreBuy.Domain;
using StoreBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;



namespace StoreBuy.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        
        IGenericRepository<Users> UserRepository = null;
        IOTPRepository OTPRepository = null;
        public UserController(IGenericRepository<Users> UserRespository,IOTPRepository OTPRepository )
        {
            this.OTPRepository = OTPRepository;
            this.UserRepository = UserRespository;
        }

        [HttpGet]
        [Route("GetAllUserDetails")]
        public IEnumerable<Users> GetAllUserDetails()
        {
            try
            {
                return UserRepository.GetAll();
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }

        [HttpGet]
        [Route("GetById")]
        public Users GetById(long UserId)
        {
            try
            {
                return UserRepository.GetById(UserId);
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }


        [HttpGet]
        [Route("Login")]
        public IHttpActionResult GetLogin(long UserId, string UserPassword)
        {
            try
            {
                Users userDetails = UserRepository.GetById(UserId);
                if (userDetails != null)
                {
                    UserPassword = GetHashCode(UserPassword);
                    if (UserPassword.Equals(userDetails.UserPassword))
                    {
                        return Ok(UserId);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(long UserId, string OldPassword, string NewPassword)
        {
            try
            {
                Constraint Check = new Constraint();
                if (Check.ValidatePassword(NewPassword))
                {
                    Users user = UserRepository.GetById(UserId);
                    if (user != null)
                    {
                        OldPassword = GetHashCode(OldPassword);
                        if (OldPassword.Equals(user.UserPassword))
                        {
                            NewPassword = GetHashCode(NewPassword);
                            user.UserPassword = NewPassword;
                        }
                        else
                            return BadRequest();
                        UserRepository.InsertOrUpdate(user);
                        return Ok(UserId);
                    }
                }
                return BadRequest();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }



        [HttpPost]
        [Route("api/User/Register")]
        public IHttpActionResult PostRegister([FromBody]Users user)
        {
            try
            {
                var password = user.UserPassword;
                var email = user.Email;
                Constraint check = new Constraint();
                if (check.ValidatePassword(password) == true && check.ValidateEmail(email) == true)
                {
                    user.UserPassword = GetHashCode(password);
                    user.Phone = GetHashCode(user.Phone);
                    UserRepository.InsertOrUpdate(user);
                    return Ok(user);
                }
                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("HashPassword")]
        public string GetHashCode(string password)
        {
            var Provider = new SHA1CryptoServiceProvider();
            var Encoding = new UnicodeEncoding();
            var ResultaArray = Provider.ComputeHash(Encoding.GetBytes(password));
            string HashCode = Convert.ToBase64String(ResultaArray, 0, ResultaArray.Length);
            return HashCode;
        }

        [HttpPut]
        [Route("Put")]
        public IHttpActionResult Put([FromBody]Users user)
        {
            try
            {
                UserRepository.InsertOrUpdate(user);
                return BadRequest();
            }
            catch
            {
                return Ok(user);
            }
        }

        [HttpGet]
        [Route("GetForgotPassword")]
        public HttpResponseMessage GetForgotPassword(long UserId)
        {
            Users UserDetails = UserRepository.GetById(UserId);
            Random random = new Random();
            
            var OTP= random.Next(100000, 999999);
            OTPRepository.InsertOTP(UserId,OTP);

            var EmailSent=SendEmail(UserDetails.Email,UserId);
            if (EmailSent)
            {
                return Request.CreateResponse(HttpStatusCode.Found, "Successfully sent");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User with UserId:" + UserId + " not found");
            }
        }
        [HttpGet]
        [Route("VerifyOTP")]
        public HttpResponseMessage VerifyOTP(long UserId,long OTPReceived)
        {
            DeleteOTP();
            var OTP = OTPRepository.ReturnByUserId(UserId);
            if(OTP==-1)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "OTP Expired");
            }
            if(OTPReceived==OTP)
            {
                return Request.CreateResponse(HttpStatusCode.Found);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Found,"Entered OTP is not valid");

            }

        }

        public void DeleteOTP()
        {
            OTPRepository.DeleteExpiredOTP();
        }
        [HttpPost]
        [Route("ResetPassword")]
        public IHttpActionResult ResetPassword(long UserId,string NewPassword)
        {
            try
            {
                
                Constraint Check = new Constraint();
                if (Check.ValidatePassword(NewPassword))
                {
                    NewPassword = GetHashCode(NewPassword);
                    Users User = UserRepository.GetById(UserId);
                    if (User != null)
                    {
                        User.UserPassword = NewPassword;
                        UserRepository.InsertOrUpdate(User);
                        return Ok(UserId);
                    }
                }
                return BadRequest();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

       
        public bool SendEmail(string Email,long UserId)
        {
            try
            {
                var OTP = OTPRepository.GetById(UserId);
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("siddichandu03@gmail.com");
                message.To.Add(new MailAddress(Email));
                message.Subject = "OrderDetails";
                string MailBody = "Your OTP is " + OTP.CurrentOtp;
                message.Body = MailBody;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new NetworkCredential("siddichandu03@gmail.com", "chandu@12345");
                smtp.EnableSsl = true;
                smtp.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
