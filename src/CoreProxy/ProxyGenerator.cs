using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreProxy
{
    public class ProxyGenerator : DispatchProxy
    {
        private Func<MethodInfo, object[], object> _invokeFunction;

        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <typeparam name="TTarget">所要代理的接口类型</typeparam>
        /// <param name="invokeFunction">拦截函数</param>
        /// <returns></returns>
        public static TTarget Create<TTarget>(Func<MethodInfo, object[], object> invokeFunction)
        {
            var target = DispatchProxy.Create<TTarget, ProxyGenerator>();
            (target as ProxyGenerator)._invokeFunction = invokeFunction;
            return target;
        }

        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <param name="targetType">所要代理的接口类型</param>
        /// <param name="invokeFunction">拦截函数</param>
        /// <returns></returns>
        public static object Create(Type targetType, Func<MethodInfo, object[], object> invokeFunction)
        {
            var method = typeof(DispatchProxy).GetMethod("Create").MakeGenericMethod(targetType, typeof(ProxyGenerator));
            var target = method.Invoke(null, null);
            (target as ProxyGenerator)._invokeFunction = invokeFunction;
            return target;
        }

        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <param name="targetType">所要代理的接口类型</param>
        /// <param name="interceptor">拦截器</param>
        /// <returns>代理实例</returns>
        public static object Create(Type targetType, IInterceptor interceptor)
        {
            return Create(targetType, interceptor.Intercept);
        }

        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <param name="targetType">所要代理的接口类型</param>
        /// <param name="interceptorType">拦截器类型</param>
        /// <param name="parameters">拦截器构造函数参数值</param>
        /// <returns>代理实例</returns>
        public static object Create(Type targetType, Type interceptorType, params object[] parameters)
        {
            var interceptor = Activator.CreateInstance(interceptorType, parameters) as IInterceptor;
            return Create(targetType, interceptor.Intercept);
        }

        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <typeparam name="TTarget">所要代理的接口类型</typeparam>
        /// <param name="interceptorType">拦截器类型</param>
        /// <param name="parameters">拦截器构造函数参数值</param>
        /// <returns></returns>
        public static TTarget Create<TTarget>(Type interceptorType, params object[] parameters)
        {
            var interceptor = Activator.CreateInstance(interceptorType, parameters) as IInterceptor;
            return Create<TTarget>(interceptor.Intercept);
        }

        /// <summary>
        /// 创建代理实例 TTarget:所要代理的接口类型 TInterceptor:拦截器类型
        /// </summary>
        /// <param name="parameters">拦截器构造函数参数值</param>
        /// <returns>代理实例</returns>
        public static TTarget Create<TTarget, TInterceptor>(params object[] parameters) where TInterceptor : IInterceptor
        {
            var interceptor = Activator.CreateInstance(typeof(TInterceptor), parameters) as IInterceptor;
            return Create<TTarget>(interceptor.Intercept);
        }

        /// <summary>
        /// 创建代理实例 TTarget:所要代理的接口类型 TInterceptor:拦截器类型
        /// </summary>
        /// <param name="interceptor">IInterceptor接口</param>
        /// <returns>代理实例</returns>
        public static TTarget Create<TTarget>(IInterceptor interceptor)
        {
            return Create<TTarget>(interceptor.Intercept);
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return this._invokeFunction(targetMethod, args);
        }
    }
}
