using System;
using System.Collections.Generic;

namespace HelpCheck_API.Constants
{
    public class CheckDigit
    {
        public static bool CheckDigitIDCard(string pid)
        {
            if (!string.IsNullOrWhiteSpace(pid) && pid.Length == 13)
            {
				char[] numberChars = pid.ToCharArray();

				int total = 0;
				int mul = 13;
				int mod = 0, mod2 = 0;
				int nsub = 0;
				int numberChars12 = 0;

				for (int i = 0; i < numberChars.Length - 1; i++)
				{
					int num = 0;
					int.TryParse(numberChars[i].ToString(), out num);

					total = total + num * mul;
					mul = mul - 1;
				}

				mod = total % 11;
				nsub = 11 - mod;
				mod2 = nsub % 10;
				int.TryParse(numberChars[12].ToString(), out numberChars12);

				if (mod2 != numberChars12)
					return false;
				else
					return true;
            }
            return false;
        }
    }
}
