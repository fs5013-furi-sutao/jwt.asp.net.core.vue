using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// ユーザ認証を行う
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/Users/login
        ///     {
        ///        "Username": "yamamotor",
        ///        "Password": "password",
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>ユーザ情報とトークン</returns>
        /// <response code="201">ユーザ情報とトークン</response>
        /// <response code="400">ユーザー名またはパスワードが正しくありません</response>    
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "ユーザー名またはパスワードが正しくありません" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                EMail = user.EMail,
                AccessToken = tokenString,
            });
        }

        /// <summary>
        /// ユーザ登録を行う
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/Users/register
        ///     {
        ///        "Username": "yamamotor",
        ///        "Password": "password",
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>メッセージ</returns>
        /// <response code="200">ユーザを登録しました</response>
        /// <response code="400">ユーザ "Username" はすでに登録されています</response>    
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            User user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok(new
                {
                    Message = $"ユーザ {user.Username} を登録しました"
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                Console.WriteLine(ex);
                return BadRequest(new { message = $"ユーザ {user.Username} はすでに登録されています" });
            }
        }

        /// <summary>
        /// 一般公開ページのメッセージを返却
        /// </summary>
        /// <returns>メッセージ</returns>
        /// <response code="200">一般公開ページ</response>
        [AllowAnonymous]
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("一般公開ページ");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}
