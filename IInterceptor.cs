using System.Reflection;

namespace CoreProxy
{
    public interface IInterceptor
    {
        object Intercept(object target, MethodInfo method, object[] parameters);
    }
}