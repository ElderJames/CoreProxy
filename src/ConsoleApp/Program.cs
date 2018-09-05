using System;
using System.Reflection;
using CoreProxy;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var poxy1 = (targetInterface)ProxyGenerator.Create(typeof(targetInterface), new SamepleProxy("coreproxy1"));
            poxy1.Write("here was invoked"); //---> "here was invoked by coreproxy1"

            var poxy2 = (targetInterface)ProxyGenerator.Create(typeof(targetInterface), typeof(SamepleProxy), "coreproxy2");
            poxy2.Write("here was invoked"); //---> "here was invoked by coreproxy2"

            var poxy3 = ProxyGenerator.Create<targetInterface, SamepleProxy>("coreproxy3");
            poxy3.Write("here was invoked"); //---> "here was invoked by coreproxy3"
        }
    }


    public class SamepleProxy : IInterceptor
    {
        private string proxyName { get; }

        public SamepleProxy(string name)
        {
            this.proxyName = name;
        }

        public object Intercept(MethodInfo method, object[] parameters)
        {
            Console.WriteLine(parameters[0] + " by " + proxyName);
            return null;
        }
    }

    public interface targetInterface
    {
        void Write(string writesome);
    }
}
