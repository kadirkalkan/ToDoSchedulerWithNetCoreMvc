using PitonProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PitonProject.Models.Managers
{
    public class External
    {
        DatabaseContext context = new DatabaseContext();
        public string GenerateSha256(string yourString)
        {
            string returnValue = "";
            try
            {
                returnValue = yourString != null ? string.Join("", new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(yourString))) : "";
            }
            catch (Exception ex)
            {
                addLog(Enums.EnumClass.LogType.error, ex.ToString(), "External", "GenerateSha256");
            }
            return returnValue;
        }

        public bool IsAccessible(string pageName, List<WebPage> forbiddenPageList)
        {
            bool redirectToIndex = true;
            foreach (WebPage page in forbiddenPageList)
            {
                if (page.PageName.Equals(pageName))
                {
                    redirectToIndex = false;
                    break;
                }
            }
            return redirectToIndex;
        }

        public bool addLog(Enums.EnumClass.LogType logTypeValue = Enums.EnumClass.LogType.info, string logText = "", string className = "-", string methodName = "-")
        {
            bool returnValue = false;
            className = String.IsNullOrEmpty(className) ? "-" : className;
            methodName = String.IsNullOrEmpty(methodName) ? "-" : methodName;
            try
            {
                string logTypeText = logTypeValue.ToString();
                LogType logType = context.LogTypes.FirstOrDefault(x => x.LogTypeText == logTypeText);
                if (logType != null && logText != null && logText.Length > 0)
                {
                    Log log = new Log();
                    log.LogType = logType;
                    log.LogText = logText;
                    log.ClassName = className;
                    log.MethodName = methodName;
                    log.CreatedTime = DateTime.Now;
                    context.Logs.Add(log);
                    returnValue = context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        internal string GetFormattedTimeString(DateTime time, string type = "MM / dd / yyyy HH:mm")
        {
            type = String.IsNullOrEmpty(type) ? "MM / dd / yyyy HH:mm" : type;
            return time.ToString(type);
        }
    }
}
