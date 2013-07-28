using System;
using System.Reflection;

namespace SquidEyes.Generic
{
    public static class ModuleHelper
    {
        public static string GetModuleName(this Assembly caller)
        {
            object[] objects = caller.
                GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            if (objects.Length == 0)
                return "(Unknown)";
            else
                return ((AssemblyTitleAttribute)objects[0]).Title;
        }

        public static Version GetModuleVersion(this Assembly caller)
        {
            return caller.GetName().Version;
        }
    }
}
