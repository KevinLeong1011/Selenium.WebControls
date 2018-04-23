/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 22:49:05
 * ***********************************************/
namespace Selenium.WebControls.Tracking
{
    /// <summary>
    /// Log标记
    /// </summary>
    public enum TrackTag
    {
        /// <summary>
        /// Test Case. 测试用例
        /// </summary>
        Case,
        /// <summary>
        /// Precondition. 前置条件
        /// </summary>
        Condition,
        /// <summary>
        /// 测试行为
        /// </summary>
        Action,
        /// <summary>
        /// 断言
        /// </summary>
        Assert,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 用户封装的行为开始标记
        /// </summary>
        UserActionStart,
        /// <summary>
        /// 用户封装的校验开始标记
        /// </summary>
        UserAssertStart,
        /// <summary>
        /// 用户封装执行结束标记
        /// </summary>
        UserEnd
    }
}