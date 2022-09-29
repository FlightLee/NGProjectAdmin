using System;
using System.Security.Cryptography;
using System.Text;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// Hash函数工具类
    /// </summary>
    public class NGHashUtil
    {
        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>输出</returns>
        public static byte[] HashData256(String input)
        {
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            return hash;
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>输出</returns>
        public static String HashToHexString256(String input)
        {
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            return Convert.ToHexString(hash);
        }

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>输出</returns>
        public static byte[] HashData512(String input)
        {
            byte[] hash;
            using (SHA512 sha512 = SHA512.Create())
            {
                hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            return hash;
        }

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>输出</returns>
        public static String HashToHexString512(String input)
        {
            byte[] hash;
            using (SHA512 sha512 = SHA512.Create())
            {
                hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            return Convert.ToHexString(hash);
        }
    }
}
