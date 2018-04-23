/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/28 19:33:11
 * ***********************************************/

namespace Selenium.WebControls.Internal
{
    /// <summary>
    /// Web请求返回状态
    /// </summary>
    public enum WebStatus
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 请求成功并且服务器创建了新的资源
        /// </summary>
        Created = 201,
        /// <summary>
        /// 服务器已接受请求，但尚未处理
        /// </summary>
        ServerAccept = 202,
        /// <summary>
        /// 服务器已成功处理了请求，但返回的信息可能来自另一来源
        /// </summary>
        UnknownAuth = 203,
        /// <summary>
        /// 错误请求
        /// </summary>
        RequestError = 400,
        /// <summary>
        /// 身份验证错误
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        /// 禁止访问
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// 请求的页面不存在
        /// </summary>
        PageMissing = 404,
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        Timeout = 500
    }
}
