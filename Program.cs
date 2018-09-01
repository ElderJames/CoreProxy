using System;
using System.Reflection;

namespace CoreProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            // var poxy = (targetInterface)ProxyGenerator.Create(typeof(targetInterface), new SamepleProxy());
            // 或
            //var poxy = (targetInterface)ProxyGenerator.Create(typeof(targetInterface), typeof(SamepleProxy));
            // 或
            var poxy = (targetInterface)ProxyGenerator.Create<targetInterface, SamepleProxy>();
            poxy.Write("here is invoked by coreproxy");
        }
    }


    public class SamepleProxy : IInterceptor
    {
        public object Intercept(object target, MethodInfo method, object[] parameters)
        {
            Console.WriteLine(parameters[0]);
            return null;
        }
    }

    public interface targetInterface
    {
        void Write(string writesome);
    }
}
