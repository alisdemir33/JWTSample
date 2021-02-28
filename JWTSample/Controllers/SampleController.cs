using JWTSample.Auth;
using JWTSample.AuxClass;
using JWTSample.Entities;
using JWTSample.Models;
//using JWTSample.ContosoModels;
using JWTSample.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JWTSample.Controllers
{
    //Authorize attribute ile bu sınıfı sadece yetkisi yani tokenı olan kişilerin girmesini söylüyorum.
    [Authorize]
    [ApiController]
    //Routing için mesela /Sample/GetSummaries olarak ayarladım.
    [Route("[controller]/[action]")]
    public class SampleController : ControllerBase
    {
        private static readonly string[] Summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        private readonly IUserService<TokenUser> _userService;
        public SampleController(IUserService<TokenUser> userService) => _userService = userService;

        //Burada da AllowAnonymous attribute nü kullanarak bu seferde bu metoda herkesin erişebileceğini söylüyorum.
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateModel authenticateModel)
        {            
            try
            {
                ServiceResult<TokenUser> serviceResult = _userService.IlanAuthenticate(authenticateModel.Username, authenticateModel.Password);
                return Ok(serviceResult);
            }
            catch (Exception exc) {
                return   Ok(new ServiceResult<TokenUser>() { isSuccessfull = false, ResultCode = 1, ResultData = null, ResultExplanation = "Beklenmeyen bir hata oluştu"+exc.Message });
            }
            //return Ok(new { Username = user.Value.username, Token = user.Value.token });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RefreshToken([FromBody] RefreshTokenEntity refreshToken)
        {
            try
            {
                ServiceResult <TokenUser> serviceResult  = _userService.IlanRefreshTokenLogin(refreshToken.RefreshToken);
                return Ok(serviceResult);

            }
            catch (Exception exc) {
                return Ok(new ServiceResult<TokenUser>() { isSuccessfull = false, ResultCode = 1, ResultData = null, ResultExplanation = "Beklenmeyen bir hata oluştu" + exc.Message });
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Signup([FromBody] SignupModel signupModel)
        {
            ServiceResult<object> signupResult=null;
            try
            {
                signupResult = _userService.Signup(signupModel);
                string ss = signupModel.name;
                return Ok(signupResult);
            }
            catch (Exception exc) {
                return Ok(signupResult);
            }

            //return Ok(new { Username = user.Value.username, Token = user.Value.token });

        }         
    
        [HttpGet]
        public IActionResult GetIlanList()
        {
            ServiceResult<IseAlimTalebi[]> result = null;
            try
            {
                result = _userService.IlanListesi();
                return Ok(result);
            }catch(Exception exc)
            {
                return Ok(result);
            }
        }

        
        [HttpPost]
        public IActionResult SaveApplication([FromBody] IsBasvurusuBilgileri basvuru) {
            ServiceResult<string> result = null;
            try
            {
                result = _userService.SaveApplication(basvuru);
                return Ok(result);
            }
            catch (Exception exc)
            {
                result = new ServiceResult<string>() { isSuccessfull = false, ResultCode = 2, ResultData = null, ResultExplanation = "Beklenneyen Hata Oluştu!" + exc.Message };
                return Ok(result);
            }
        }


        /* Deneme 

        [HttpGet]
        public IActionResult GetUserList()
        {
            List<Users> items = _userService.getUserList();
            return Ok(items);
        }

        [HttpGet]
        public IActionResult GetIngredients()
        {            
            return Ok(_userService.GetIngredients());
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(_userService.GetOrders());
        }

        [HttpGet]
        public IActionResult GetSummaries() => Ok(Summaries);
        
         */
    }
}
