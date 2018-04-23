/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/5 2:42:31
 * ***********************************************/
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 数值约束判定
    /// </summary>
    public class Num
    {
        /// <summary>
        /// 至少
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<T>>, bool> AtLeast<T>(int n)
        {
            return delegate (AssertContext<IEnumerable<T>> context)
            {
                context.Command += "AtLeast";
                context.Parameters.Add(n);
                return EnvManager.Auto ? context.Data.Count() >= n : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定给定的文本内容中的数值至少等于期望值
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> AtLeast(double expected)
        {
            return delegate (AssertContext context)
            {
                context.Command += "NUmberAtLeast";
                context.Parameters.Add(expected);
                if (!EnvManager.Auto) return true;
                double[] nums = context.Data.GetNumbers();
                if (nums.Length == 0)
                {
                    context.Message = $"给定的文本内容中不包含数值，原内容：{context.Data}";
                }
                return nums[0] >= expected;
            };
        }

        /// <summary>
        /// 至多
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<T>>, bool> AtMost<T>(int n)
        {
            return delegate (AssertContext<IEnumerable<T>> context)
            {
                context.Command += "AtMost";
                context.Parameters.Add(n);
                if (!EnvManager.Auto) return true;
                context.Data.ThrowIfNull("context.Data");
                return context.Data.Count() <= n;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定给定的文本内容中的数值最大等于期望值
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> AtMost(double expected)
        {
            return delegate (AssertContext context)
            {
                context.Command += "NumberAtMost";
                context.Parameters.Add(expected);
                if (!EnvManager.Auto) return true;
                double[] nums = context.Data.GetNumbers();
                if (nums.Length == 0)
                {
                    context.Message = $"给定的文本内容中不包含数值，原内容：{context.Data}";
                }
                return nums[0] <= expected;
            };
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<T>>, bool> MoreThan<T>(int n)
        {
            return delegate (AssertContext<IEnumerable<T>> context)
            {
                context.Command += "MoreThan";
                context.Parameters.Add(n);
                return EnvManager.Auto ? context.Data.Count() > n : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定给定的文本内容中的数值大于期望值
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> MoreThan(double expected)
        {
            return delegate (AssertContext context)
            {
                context.Command += "NumberMoreThan";
                context.Parameters.Add(expected);
                if (!EnvManager.Auto) return true;
                double[] nums = context.Data.GetNumbers();
                if (nums.Length == 0)
                {
                    context.Message = $"给定的文本内容中不包含数值，原内容：{context.Data}";
                }
                return nums[0] > expected;
            };
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<T>>, bool> LessThan<T>(int n)
        {
            return delegate (AssertContext<IEnumerable<T>> context)
            {
                context.Command += "LessThan";
                context.Parameters.Add(n);
                return EnvManager.Auto ? context.Data.Count() < n : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定给定的文本内容中的数值小于期望值
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> LessThan(double expected)
        {
            return delegate (AssertContext context)
            {
                context.Command += "NumberLessThan";
                context.Parameters.Add(expected);
                if (!EnvManager.Auto) return true;
                double[] nums = context.Data.GetNumbers();
                if (nums.Length == 0)
                {
                    context.Message = $"给定的文本内容中不包含数值，原内容：{context.Data}";
                }
                return nums[0] < expected;
            };
        }

        /// <summary>
        /// 介于最小值和最大值之间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<T>>, bool> Between<T>(int min, int max)
        {
            return delegate (AssertContext<IEnumerable<T>> context)
            {
                context.Command += "CountBetween";
                context.Parameters.Add(min);
                context.Parameters.Add(max);
                int count = context.Data.Count();
                return EnvManager.Auto ? count < max && count > min : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定给定的文本内容中的数值在期望区间内
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> Between(double min, double max)
        {
            return delegate (AssertContext context)
            {
                context.Command += "NumberBetween";
                context.Parameters.Add(min);
                context.Parameters.Add(max);
                if (!EnvManager.Auto) return true;
                double[] nums = context.Data.GetNumbers();
                if (nums.Length == 0)
                {
                    context.Message = $"给定的文本内容中不包含数值，原内容：{context.Data}";
                }
                return nums[0] < max && nums[0] > min;
            };
        }
    }
}
