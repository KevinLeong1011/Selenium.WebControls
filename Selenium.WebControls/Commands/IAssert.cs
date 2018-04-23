/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:00:56
 * ***********************************************/

namespace Selenium.WebControls.Commands
{
    /// <summary>
    /// 断言接口
    /// </summary>
    public interface IAssert
    {
        /// <summary>
        /// 相等断言
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        void AreEqual(object expected, object actual, string message, params object[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="message"></param>
        void AreEqual(object expected, object actual, string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        void AreEqual(object expected, object actual);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="delta"></param>
        void AreEqual(double expected, double actual, double delta);

        /// <summary>
        /// 实际字符串与期望值相等，可以指定是否忽略大小写
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="ignoreCase"></param>
        void AreEqual(string expected, string actual, bool ignoreCase);

        void AreEqual(string expected, string actual, bool ignoreCase, string message);

        void AreEqual(string expected, string actual, bool ignoreCase, string message, params object[] parameters);

        void AreNotEqual(string notExpected, string actual, bool ignoreCase);

        void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message);

        void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message, params object[] parameters);

        void IsTrue(bool condition);

        void IsTrue(bool condition, string message);

        void IsTrue(bool condition, string message, params object[] parameters);

        void IsFalse(bool condition);

        void IsFalse(bool condition, string message);

        void IsFalse(bool condition, string message, params object[] parameters);


        void Fail();

        void Fail(string message);

        void Fail(string message, params object[] parameters);
        
        
    }
}
