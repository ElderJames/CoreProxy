using System.Reflection;

namespace CoreProxy
{
    public interface IInterceptor
    {
        object Intercept(MethodInfo method, object[] parameters);
    }
}