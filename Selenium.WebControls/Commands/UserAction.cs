/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/2 23:20:00
 * ***********************************************/

namespace Selenium.WebControls.Commands
{
    /// <summary>
    /// 用户行为基类
    /// </summary>
    public abstract class UserAction
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="selenium"></param>
        /// <param name="desc"></param>
        /// <param name="args"></param>
        protected UserAction(WebSelenium selenium, string desc, params object[] args)
        {
            Selenium = selenium;
            Description = string.Format(desc, args);
            Args = args;
        }

        #endregion Constructors

        #region Attributes

        /// <summary>
        /// 用户行为描述
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// 参数
        /// </summary>
        protected object[] Args { get; private set; }

        /// <summary>
        /// Selenium
        /// </summary>
        protected WebSelenium Selenium { get; private set; }

        #endregion Attributes

        #region Public Methods

        /// <summary>
        /// 执行
        /// </summary>
        public abstract void Run();

        #endregion Public Methods
    }
}
