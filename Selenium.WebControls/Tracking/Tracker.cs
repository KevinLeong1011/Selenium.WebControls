/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:24:26
 * ***********************************************/
using Selenium.WebControls.Environments;
using System.Collections.Generic;

namespace Selenium.WebControls.Tracking
{
    /// <summary>
    /// 
    /// </summary>
    public class Tracker
    {
        private static Tracker instance = new Tracker();

        private Tracker()
        {
            records = new List<TrackText>();
        }

        private List<TrackText> records { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public static void Log(TrackTag tag, string message)
        {
            instance.records.Add(new TrackText { Tag = tag, Text = message });
        }

        public static void Clear()
        {
            instance.records.Clear();
        }

        /// <summary>
        /// 生成用例
        /// </summary>
        public static void WriteCase()
        {
            if (generator == null)
            {
                generator = EnvManager.GetCaseGenerator();
            }
            generator.Write(instance.records);
            Tracker.Clear();
        }

        private static ICaseGenerator generator;
    }
}
