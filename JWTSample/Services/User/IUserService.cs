using JobWcfService;
using JWTSample.Auth;
using JWTSample.AuxClass;
using JWTSample.Models;
//using JWTSample.ContosoModels;
using JWTSample.ViewModels;
using System;
using System.Collections.Generic;

namespace JWTSample.Services.User
{
    public interface IUserService<T>
    {
        // (string username, string token)? Authenticate(string username, string password);     
        TokenUser Authenticate(string username, string password);
        ServiceResult<TokenUser> IlanAuthenticate(string username, string password);
        ServiceResult<object> Signup(SignupModel model);
        ServiceResult<TokenUser> IlanRefreshTokenLogin(string refreshTokend);
        List<Users> getUserList();
        Ingredients GetIngredients();
        List<Order> GetOrders();
        ServiceResult<IseAlimTalebi[]> IlanListesi();
        ServiceResult<Application[]> BasvuruListesi(string TCNo);
        ServiceResult<string> SaveApplication(IsBasvurusuBilgileri basvuru);
    }
}
