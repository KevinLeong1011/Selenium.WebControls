/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/5 15:21:03
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.WebControls.Tracking
{
    /// <summary>
    /// 
    /// </summary>
    public class Logger
    {
        #region Constructors

        public Logger()
        {
            string location = Path.Combine(IOHelper.GetCurrentLocation(),"Results");
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

        }

        #endregion Constructors

        #region Attributes



        #endregion Attributes

        #region Public Methods



        #endregion Public Methods

        #region Fields



        #endregion Fields
    }
}
