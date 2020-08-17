using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VSS2Git
{
    public static class ExceptionHandler
    {
        /// <summary>
        /// Add item to exception Data collection
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void AddData(Exception ex, string key, object value)
        {
            string str;
            if (value == null)
            {
                str = "(null)";
            }
            else if (value == DBNull.Value)
            {
                str = "(DBNull)";
            }
            else
            {
                str = value.ToString();
            }
            ex.Data.Add(ex.Data.Count.ToString() + " " + key, str);
        }

        private static void AppendPropValue(StringBuilder errorDesc, string name, object val)
        {
            IEnumerable ev = null;
            if (!(val is String))
            {
                ev = val as IEnumerable;
            }
            if (ev != null)
            {
                int i = 0;
                foreach (var v in ev)
                {
                    errorDesc.AppendFormat("{0}[{1}] = {2}\r\n", name, i++, (v ?? "(null)"));
                }
            }
            else
            {
                errorDesc.AppendFormat("{0} = {1}\r\n", name, (val ?? "(null)"));
            }
        }

        public static string UnwindExceptions(Exception ex)
        {
            try
            {
                if (ex == null)
                {
                    return "Null exception object";
                }

                StringBuilder errorDesc = new StringBuilder();
                Exception e = ex;
                string stack;

                while (e != null)
                {
                    errorDesc.AppendFormat("{0}\r\n", e.Message);
                    errorDesc.AppendFormat("Type: {0}\r\n", e.GetType().Name);
                    if (!String.IsNullOrEmpty(e.Source))
                    {
                        errorDesc.AppendFormat("Module: {0}\r\n", e.Source);
                    }
                    if (e.TargetSite != null)
                    {
                        errorDesc.AppendFormat("Method name: {0}\r\nMethod declaration: {1}\r\n",
                            e.TargetSite.DeclaringType + "." + e.TargetSite.Name, e.TargetSite);
                    }

                    // properties
                    bool title = false;
                    foreach (PropertyInfo prop in e.GetType().GetProperties())
                    {
                        string name = prop.Name;
                        // exclude the base Exception properties
                        if (name != "Data" && name != "HelpLink" && name != "HResult" && name != "InnerException"
                            && name != "Message" && name != "Source" && name != "StackTrace" && name != "TargetSite")
                        {
                            object val = prop.GetValue(e, null);
                            if (!title)
                            {
                                errorDesc.AppendLine("\r\n--------------  Properties:\r\n");
                                title = true;
                            }
                            AppendPropValue(errorDesc, name, val);
                        }
                    }

                    // error data collection
                    if (e.Data.Count != 0)
                    {
                        errorDesc.AppendLine("\r\n--------------  Error data:\r\n");
                        // Append the data collection.
                        foreach (DictionaryEntry de in e.Data)
                        {
                            AppendPropValue(errorDesc, de.Key.ToString(), de.Value);
                        }
                    }
                    stack = e.StackTrace;
                    if (!String.IsNullOrEmpty(stack))
                    {
                        errorDesc.AppendFormat("\r\n--------------  Stack trace:\r\n{0}", stack);
                    }

                    if (e.InnerException != null)
                    {
                        // if there's an inner exception, print a seperator.
                        errorDesc.AppendLine("\r\n\r\n*******************************************\r\nInner exception:\r\n\r\n");
                    }
                    // process next exception
                    e = e.InnerException;
                }

                return errorDesc.ToString();
            }
            catch
            {
                return "Error unwinding exception";
            }
        }
    }
}
