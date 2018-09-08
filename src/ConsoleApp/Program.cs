using System;
using System.Reflection;
using CoreProxy;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var poxy1 = (targetInterface)ProxyGenerator.Create(typeof(targetInterface), new SampleProxy("coreproxy1"));
            poxy1.Write("here was invoked"); //---> "here was invoked by coreproxy1"

            var poxy2 = (targetInterface)ProxyGenerator.Create(typeof(targetInterface), typeof(SampleProxy), "coreproxy2");
            poxy2.Write("here was invoked"); //---> "here was invoked by coreproxy2"

            var poxy3 = ProxyGenerator.Create<targetInterface, SampleProxy>("coreproxy3");
            poxy3.Write("here was invoked"); //---> "here was invoked by coreproxy3"
            
            var poxy4 = ProxyGenerator.Create<targetInterface>(new SampleProxy("coreproxy4"));
            poxy4.Write("here was invoked"); //---> "here was invoked by coreproxy4"
        }
    }


    public class SampleProxy : IInterceptor
    {
        private string proxyName { get; }

        public SampleProxy(string name)
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
