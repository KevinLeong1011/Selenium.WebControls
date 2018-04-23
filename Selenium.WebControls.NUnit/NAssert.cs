/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/3 2:20:19
 * ***********************************************/
using NUnit.Framework;
using Selenium.WebControls.Commands;
using System;

namespace Selenium.WebControls.NUnit
{
    /// <summary>
    /// 
    /// </summary>
    public class NAssert : IAssert
    {
        public void AreEqual(object expected, object actual, string message, params object[] parameters)
        {
            Assert.AreEqual(expected, actual, message, parameters);
        }

        public void AreEqual(object expected, object actual, string message)
        {
            Assert.AreEqual(expected, actual, message);
        }

        public void AreEqual(object expected, object actual)
        {
            Assert.AreEqual(expected, actual);
        }

        public void AreEqual(double expected, double actual, double delta)
        {
            Assert.AreEqual(expected, actual, delta);
        }

        public void AreEqual(string expected, string actual, bool ignoreCase)
        {
            if (ignoreCase)
            {
                Assert.AreEqual(expected.ToLower(), actual.ToLower());
            }
            else
            {
                Assert.AreEqual(expected, actual);
            }
        }

        public void AreEqual(string expected, string actual, bool ignoreCase, string message)
        {
            if (ignoreCase)
            {
                Assert.AreEqual(expected.ToLower(), actual.ToLower(), message);
            }
            else
            {
                Assert.AreEqual(expected, actual, message);
            }
        }

        public void AreEqual(string expected, string actual, bool ignoreCase, string message, params object[] parameters)
        {
            if (ignoreCase)
            {
                Assert.AreEqual(expected.ToLower(), actual.ToLower(), message, parameters);
            }
            else
            {
                Assert.AreEqual(expected, actual, message, parameters);
            }
        }

        public void AreNotEqual(string notExpected, string actual, bool ignoreCase)
        {
            if (ignoreCase)
            {
                Assert.AreNotEqual(notExpected.ToLower(), actual.ToLower());
            }
            else
            {
                Assert.AreNotEqual(notExpected, actual);
            }
        }

        public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message)
        {
            if (ignoreCase)
            {
                Assert.AreNotEqual(notExpected.ToLower(), actual.ToLower(), message);
            }
            else
            {
                Assert.AreNotEqual(notExpected, actual, message);
            }
        }

        public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message, params object[] parameters)
        {
            if (ignoreCase)
            {
                Assert.AreNotEqual(notExpected.ToLower(), actual.ToLower(), message, parameters);
            }
            else
            {
                Assert.AreNotEqual(notExpected, actual, message, parameters);
            }
        }

        public void Fail()
        {
            Assert.Fail();
        }

        public void Fail(string message)
        {
            Assert.Fail(message);
        }

        public void Fail(string message, params object[] parameters)
        {
            Assert.Fail(message, parameters);
        }

        public void IsFalse(bool condition)
        {
            Assert.IsFalse(condition);
        }

        public void IsFalse(bool condition, string message)
        {
            Assert.IsFalse(condition, message);
        }

        public void IsFalse(bool condition, string message, params object[] parameters)
        {
            Assert.IsFalse(condition, message, parameters);
        }

        public void IsTrue(bool condition)
        {
            Assert.IsTrue(condition);
        }

        public void IsTrue(bool condition, string message)
        {
            Assert.IsTrue(condition, message);
        }

        public void IsTrue(bool condition, string message, params object[] parameters)
        {
            Assert.IsTrue(condition, message, parameters);
        }
    }
}
