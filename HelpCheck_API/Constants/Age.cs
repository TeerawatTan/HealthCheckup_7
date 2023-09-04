using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Constants
{
    public class Calculate
    {
        public static int CalculateAge(DateTime dob)
        {
            int age = 0;
            try
            {
                age = DateTime.Now.AddYears(-dob.Year).Year;
                return age;
            }
            catch (Exception)
            {
                return age;
            }
        }
    }
}
