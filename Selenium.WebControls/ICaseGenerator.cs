/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 22:48:03
 * ***********************************************/
using Selenium.WebControls.Tracking;
using System.Collections.Generic;

namespace Selenium.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICaseGenerator
    {
        void Write(List<TrackText> logs);
    }
}
