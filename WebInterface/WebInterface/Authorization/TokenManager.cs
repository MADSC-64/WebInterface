using System;
using System.Collections.Generic;

namespace WebInterface.Authorization
{
    public class TokenManager
    {
        static List<Token> activeTokens = new List<Token>();

        /// <summary>
        /// Creates New Token To Allow Acess To More Restricted Parts
        /// </summary>
        /// <param name="singleUse">If True token will be deleted after single use</param>
        /// <param name="expirationDelay">determines how long will it take To expire</param>
        /// <param name="accessType">determines what content type can it access</param>
        /// <returns>The Token ID To be sent back to client</returns>
        public static string CreateToken(bool singleUse, int expirationDelay, string accessType)
        {
            var token = new Token();

            string tokenData = GenerateRandomString();

            token.tokenData = tokenData;
            token.singleUse = singleUse;

            token.expirationTime = DateTime.UtcNow.Ticks + expirationDelay;

            token.accessType = accessType;

            Console.WriteLine($"Creating Token:{tokenData} access:{accessType} single use:{singleUse}");

            activeTokens.Add(token);

            return tokenData;
        }


        public static bool TestToken(string tokenData, string accessType)
        {
            long utcTimeNow = DateTime.UtcNow.Ticks;

            foreach (var token in activeTokens)
            {
                if (token.expirationTime < utcTimeNow) activeTokens.Remove(token);

                if (token.tokenData == tokenData && token.accessType == accessType)
                {
                    Console.WriteLine("Token Found");

                    if (token.singleUse) activeTokens.Remove(token);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Generates A Random Non Repeating Token Code
        /// </summary>
        static string GenerateRandomString()
        {
            string guid = Guid.NewGuid().ToString("N");
            while (char.IsDigit(guid[0]))
                guid = Guid.NewGuid().ToString("N");

            return guid;
        }

        public class Token
        {
            public string tokenData;
            public string accessType;
            public long expirationTime;
            public bool singleUse;
        }
    }
}
