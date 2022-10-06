
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NGProjectAdmin.Common.Global;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NGProjectAdmin.WebApi.AppCode.JwtSecurity
{
    [Serializable]
    public class JwtAuthentication
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Required]
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public String Password { get; set; }

        /// <summary>
        /// 盐
        /// </summary>
        [Required]
        public String Salt { get; set; }

        /// <summary>
        /// 获取jwt口令
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>JwtSecurityToken</returns>
        public static JwtSecurityToken GetJwtSecurityToken(String userName)
        {
            //创建claim
            var authClaims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub,userName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
               };
            IdentityModelEventSource.ShowPII = true;
            //签名秘钥 
            var ecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(NGAdminGlobalContext.JwtSettings.SecurityKey));
            var token = new JwtSecurityToken(
                   issuer: NGAdminGlobalContext.JwtSettings.Issuer,
                   audience: NGAdminGlobalContext.JwtSettings.Audience,
                   expires: DateTime.Now.AddMinutes(NGAdminGlobalContext.JwtSettings.TokenExpiration),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(ecurityKey, SecurityAlgorithms.HmacSha256)
                   );
            return token;
        }
    }
}
