#region Using
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using log4net;
using PostSharp.Extensibility;
using PostSharp.Laos; 
#endregion

namespace LogSqlAspects
{
    [Serializable]
    public class LogAdoNetAttribute : OnMethodInvocationAspect
    {
        #region Compile-time

        public override bool CompileTimeValidate(MethodBase method)
        {
            bool isValid = typeof(IDbCommand).IsAssignableFrom(method.DeclaringType);
            if (isValid)
            {
                MessageSource.MessageSink.Write(
                    new Message(SeverityType.ImportantInfo, "LogAdoNetApplied",
                                string.Format("Intercepted calls to {0} {1}", method.DeclaringType.FullName, method.Name), method.Name));
            }
            else
            {
                MessageSource.MessageSink.Write(
                    new Message(SeverityType.ImportantInfo, "LogAdoNetInvalidApplication",
                                string.Format("Attempted interception of calls to {0} {1} was invalid", method.DeclaringType.FullName, method.Name), method.Name));
            }
            return isValid;
        }

        #endregion


        #region Runtime

        [NonSerialized]
        private ILog _log;

        public override void RuntimeInitialize(MethodBase method)
        {
            _log = LogManager.GetLogger(method.DeclaringType);
        }

        public override void OnInvocation(MethodInvocationEventArgs eventArgs)
        {
            if (_log.IsInfoEnabled)
                LogCommand((IDbCommand)eventArgs.Instance);

            eventArgs.Proceed();
        }

        private void LogCommand(IDbCommand command)
        {
            string paramString = String.Join(", ", command.Parameters.Cast<DbParameter>().Select(param => string.Format("{0}='{1}'", param.ParameterName, param.Value)).ToArray());
            _log.Info(String.Format("Executing {0} {1}", command.CommandText, paramString));
        }

        #endregion
    }
}