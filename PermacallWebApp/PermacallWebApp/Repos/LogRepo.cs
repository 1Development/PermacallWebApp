using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;
using PermacallWebApp.Models;

namespace PermacallWebApp.Repos
{
    public class LogRepo
    {
        public enum LogCategory
        {
            Request,
            Error
        }
        public static bool Log(string logDestription, LogCategory category, string ip, string username)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"desc", logDestription},
                {"cate", category.ToString()},
                {"ip", ip },
                {"username", username }
            };
            var result = DB.MainDB.InsertQuery("INSERT INTO LOG(DESCRIPTION, CATEGORY, IP, USERNAME) VALUES (?, ?, ?, ?)", parameters);

            return result;
        }

        public static Dictionary<uint, string> GetChannelUsageData(int timeframeInDays)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"timeframe", timeframeInDays},
            };
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT channel.channelID, ROUND((COUNT(channel.channelID) / (SELECT SUM(count) FROM (SELECT Count(TSChannelID) as `count` FROM tslog_loggeduser WHERE TSUserID NOT IN (1, 10) GROUP BY TSChannelID) as `tbl`))*100,2) AS \"Usage\" FROM tslog_tsuser tsuser INNER JOIN tslog_loggeduser log ON log.TSUserID = tsuser.TSUserID AND tsuser.TSUserID NOT IN (1, 10) AND log.`Timestamp` > DATE_SUB(CURDATE(), INTERVAL ? DAY) RIGHT OUTER JOIN tslog_channel channel ON log.TSChannelID = channel.channelID WHERE channel.channelName NOT LIKE \"[spacer%]\" AND channel.channelName NOT LIKE \"Working/Learning\" AND channel.channelName NOT LIKE \"Strikes\" GROUP BY channel.channelID ORDER BY COUNT(channel.channelID) DESC", parameters);

            Dictionary<uint,string> resultDict = new Dictionary<uint, string>();
            foreach (var dbResult in result)
            {
                resultDict[dbResult.Get("channelID").ToUInt()] = dbResult.Get("Usage");
            }

            return resultDict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeframeInDays"></param>
        /// <returns>
        /// Dictionary(TSUserID, Dictionary(TSChannelID, UsagePercentage))
        /// </returns>
        public static Dictionary<uint, Dictionary<uint, string>> GetIndividualChannelUsageData(int timeframeInDays)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"timeframe", timeframeInDays},
                {"timeframe2", timeframeInDays}
            };
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT tsuser.CustomName, tsuser.TSUsername, channel.channelName, ROUND(COUNT(log.TSChannelID) / TotTime.total*100,2) as PercInChannel, ROUND(COUNT(log.TSChannelID)*5/60,2) as TimeInChannel FROM tslog_loggeduser log INNER JOIN tslog_channel channel ON channel.channelID = log.TSChannelID INNER JOIN tslog_tsuser tsuser ON tsuser.TSUserID = log.TSUserID INNER JOIN (SELECT TSUserID, Count(*) as total FROM tslog_loggeduser WHERE `Timestamp` > DATE_SUB(CURDATE(), INTERVAL ? DAY) GROUP BY TSUserID ORDER BY TSUserID) as `TotTime` on log.TSUserID = TotTime.TSUserID WHERE log.TSChannelID = channel.channelID AND log.TSUserID = tsuser.TSUserID AND log.TSUserID = TotTime.TSUserID AND log.`Timestamp` > DATE_SUB(CURDATE(), INTERVAL ? DAY) GROUP BY log.TSUserID, log.TSChannelID ORDER BY TotTime.total DESC, TotTime.TSUserID, PercInChannel DESC", parameters);

            Dictionary<uint, Dictionary<uint, string>> resultDict = new Dictionary<uint, Dictionary<uint, string>>();

            foreach (var dbResult in result)
            {
                resultDict[dbResult.Get("TSUserID").ToUInt()][dbResult.Get("channelID").ToUInt()] = dbResult.Get("PercInChannel");
            }

            return resultDict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeframeInDays"></param>
        /// <returns>
        /// Dictionary(TSUserID, Dictionary(TSChannelID, UsagePercentage))
        /// </returns>
        public static Dictionary<string, Dictionary<string, string>> GetNamedIndividualChannelUsageData(int timeframeInDays)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"timeframe", timeframeInDays},
                {"timeframe2", timeframeInDays}
            };
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT tsuser.CustomName, tsuser.TSUsername, channel.channelName, ROUND(COUNT(log.TSChannelID) / TotTime.total*100,2) as PercInChannel, ROUND(COUNT(log.TSChannelID)*5/60,2) as TimeInChannel FROM tslog_loggeduser log INNER JOIN tslog_channel channel ON channel.channelID = log.TSChannelID INNER JOIN tslog_tsuser tsuser ON tsuser.TSUserID = log.TSUserID INNER JOIN (SELECT TSUserID, Count(*) as total FROM tslog_loggeduser WHERE `Timestamp` > DATE_SUB(CURDATE(), INTERVAL ? DAY) GROUP BY TSUserID ORDER BY TSUserID) as `TotTime` on log.TSUserID = TotTime.TSUserID WHERE log.TSChannelID = channel.channelID AND log.TSUserID = tsuser.TSUserID AND log.TSUserID = TotTime.TSUserID AND log.`Timestamp` > DATE_SUB(CURDATE(), INTERVAL ? DAY) GROUP BY log.TSUserID, log.TSChannelID ORDER BY TotTime.total DESC, TotTime.TSUserID, PercInChannel DESC", parameters);

            Dictionary<string, Dictionary<string, string>> resultDict = new Dictionary<string, Dictionary<string, string>>();

            foreach (var dbResult in result)
            {
                resultDict[dbResult.Get("TSUserID")][dbResult.Get("channelID")] = dbResult.Get("PercInChannel");
            }

            return resultDict;
        }

        public static Dictionary<string, Dictionary<string, string>> GetTimeData(int timeframeInDays)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"timeframe", timeframeInDays},
            };
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT HOUR(log.`Timestamp`), tsuser.TSUserID, tsuser.TSUsername, count(*) FROM tslog_tsuser tsuser INNER JOIN tslog_loggeduser log ON log.TSUserID = tsuser.TSUserID AND tsuser.TSUserID NOT IN (1, 10) AND log.`Timestamp` > DATE_SUB(CURDATE(), INTERVAL ? DAY) RIGHT OUTER JOIN tslog_channel channel ON log.TSChannelID = channel.channelID GROUP BY HOUR(log.`Timestamp`), tsuser.TSUserID", parameters);

            Dictionary<string, Dictionary<string, string>> resultDict = new Dictionary<string, Dictionary<string, string>>();

            foreach (var dbResult in result)
            {
                resultDict[dbResult.Get("TSUserID")][dbResult.Get("channelID")] = dbResult.Get("PercInChannel");
            }

            return resultDict;
        }
    }
}