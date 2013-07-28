using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SquidEyes.Generic
{
    public static partial class ExceptionExtenders
    {
        public static string ToSingleLine(this Exception error)
        {
            var sb = new StringBuilder();

            sb.Append(error.GetType().Name);

            sb.Append(": ");

            sb.Append(error.Message.ToSingleLine());

            sb.Append(" (StackTrace: ");

            var stackTrace = new StackTrace(error);

            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);

                if (i > 0)
                    sb.Append(", ");

                sb.Append(frame.GetMethod().GetInterface());
            }

            sb.Append(")");

            return sb.ToString();
        }

        public static string GetInterface(this MethodBase methodBase)
        {
            var sb = new StringBuilder();

            if (methodBase is MethodInfo)
            {
                var methodInfo = (MethodInfo)methodBase;

                if (methodInfo.ReturnParameter == null)
                {
                    sb.Append("System.Void");
                }
                else
                {
                    sb.Append(methodInfo.ReturnParameter.
                        ParameterType.ToString());
                }

                sb.Append(' ');
            }

            if (methodBase.DeclaringType == null)
                sb.Append("{EmittedType}");
            else
                sb.Append(methodBase.DeclaringType.ToString());

            if (methodBase is MethodInfo)
            {
                sb.Append('.');

                sb.Append(methodBase.Name);
            }

            sb.Append('(');

            int count = 0;

            foreach (var param in methodBase.GetParameters())
            {
                count += 1;

                if (count > 1)
                    sb.Append(", ");

                sb.Append(param.ParameterType.ToString().Replace('+', '.'));

                if (!string.IsNullOrWhiteSpace(param.Name))
                {
                    sb.Append(' ');

                    sb.Append(param.Name);
                }
            }

            sb.Append(')');

            return sb.ToString();
        }
    }
}
