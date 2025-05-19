using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YallaR7la.Data.Models;
using YallaR7la.DtoModels;

namespace YallaR7la.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AcountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        //[HttpPost("Regesteration")]
        //public async Task<IActionResult> AddNewUser([FromForm] MdlUser newUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using var stream = new MemoryStream();
        //        await newUser.ImageData.CopyToAsync(stream);
        //        var user = new User
        //        {
        //            UserName = newUser.Email,
        //            Name = newUser.Name,

        //            Email = newUser.Email,
        //            PhoneNumper = newUser.PhoneNumper,
        //            City = newUser.City,
        //            Prefrance = newUser.Prefrance,
        //            BirthDate = newUser.BirthDate,
        //            ImageData = stream.ToArray(),
        //            UniqueIdImage = Guid.NewGuid()

        //        };


        //        IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
        //        if (result.Succeeded)
        //            return Ok("Sucsess");
        //        else
        //        {
        //            foreach (var item in result.Errors)
        //            {
        //                ModelState.AddModelError("", item.Description);
        //            }
        //        }
        //    }
        //    return BadRequest(ModelState);
        //}

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(MdlLogin login)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User user = await _userManager.FindByEmailAsync(login.Email);
        //        if (user != null)
        //        {
        //            if (await _userManager.CheckPasswordAsync(user, login.Password))
        //            {
        //                return Ok("token");
        //            }
        //            else
        //            {
        //                return Unauthorized();
        //            }

        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "User Email is not valid!");
        //        }
        //    }
        //    return BadRequest(ModelState);
        //}

       

    }
}
