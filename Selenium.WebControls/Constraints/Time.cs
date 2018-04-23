/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/5 2:45:11
 * ***********************************************/
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using System;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class Time
    {
        /// <summary>
        /// 校验时间在期望值之前
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext<DateTime>, bool> Before(DateTime expected)
        {
            return delegate (AssertContext<DateTime> context)
            {
                context.Command += "TimeBefore";
                return EnvManager.Auto ? context.Data < expected : true;
            };
        }

        /// <summary>
        /// 校验时间在期望值之后
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext<DateTime>, bool> After(DateTime expected)
        {
            return delegate (AssertContext<DateTime> context)
            {
                context.Command += "TimeAfter";
                return EnvManager.Auto ? context.Data > expected : true;
            };
        }

        /// <summary>
        /// 校验时间在期望值之间
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Func<AssertContext<DateTime>, bool> Between(DateTime min, DateTime max)
        {
            return delegate (AssertContext<DateTime> context)
            {
                context.Command += "TimeBetween";
                return EnvManager.Auto ? context.Data > min && context.Data < max : true;
            };
        }
    }
}
