/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 22:58:48
 * ***********************************************/
using Autofac;
using Selenium.WebControls.Commands;

namespace Selenium.WebControls.Utils
{
    /// <summary>
    /// 使用依赖倒置实现的数据工厂
    /// </summary>
    public class DataFactory
    {
        private static DataFactory instance = new DataFactory();

        private IContainer container;

        private ContainerBuilder builder;

        private DataFactory()
        {
            builder = new ContainerBuilder();
        }

        /// <summary>
        /// 注册<see cref="IAssert"/>实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterAssert<T>()
        {
            instance.builder.RegisterType<T>().As<IAssert>();
        }

        /// <summary>
        /// 创建<see cref="IAssert"/>实体类对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResolveAssert<T>()
        {
            if (instance.container == null)
            {
                instance.container = instance.builder.Build();
            }
            return instance.container.Resolve<T>();
        }

    }
}
