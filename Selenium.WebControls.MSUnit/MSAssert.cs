/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/27 23:05:24
 * ***********************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selenium.WebControls.Commands;
using System;

namespace Selenium.WebControls.MSUnit
{
    public class MSAssert : IAssert
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
            Assert.AreEqual(expected, actual, ignoreCase);
        }

        public void AreEqual(string expected, string actual, bool ignoreCase, string message)
        {
            Assert.AreEqual(expected, actual, ignoreCase, message);
        }

        public void AreEqual(string expected, string actual, bool ignoreCase, string message, params object[] parameters)
        {
            Assert.AreEqual(expected, actual, ignoreCase, message, parameters);
        }

        public void AreNotEqual(string notExpected, string actual, bool ignoreCase)
        {
            Assert.AreNotEqual(notExpected, actual, ignoreCase);
        }

        public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message)
        {
            Assert.AreNotEqual(notExpected, actual, ignoreCase, message);
        }

        public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message, params object[] parameters)
        {
            Assert.AreNotEqual(notExpected, actual, ignoreCase, message, parameters);
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
