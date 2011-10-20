using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace CM.Core
{
    public static class Data
    {
        public static DataTable GetDataTable(string connectionName, string procName, params object[] parameterVals)
        {
            var ds = GetDataSet(connectionName, procName, parameterVals);
            return ds.Tables[0];
        }

        public static DataSet GetDataSet(string connectionName, string procName, params object[] parameterVals)
        {
            var db = DatabaseFactory.CreateDatabase(connectionName);
            var dbCommand = PrepCommand(db, procName, parameterVals);
            return db.ExecuteDataSet(dbCommand);
        }

        public static int ExecuteNonQuery(string connectionName, string procName, params object[] parameterVals)
        {
            var db = DatabaseFactory.CreateDatabase(connectionName);
            var dbCommand = PrepCommand(db, procName, parameterVals);
            return db.ExecuteNonQuery(dbCommand);
        }

        private static DbCommand PrepCommand(Database db, string procName, params object[] parameterVals)
        {
            DbCommand dbCommand = db.GetStoredProcCommand(procName);
            dbCommand.CommandTimeout = 180;
            db.DiscoverParameters(dbCommand);

            int numVals = parameterVals.Length;
            /// Start at 1; skip 0 which is RETURN_VALUE.
            for (int i = 1; i < dbCommand.Parameters.Count; i++)
            {
                /// parameterVals should have same # elements, but index is offset by 1 compared to dbCommand.Parameters.
                int valIndex = i - 1;
                object value = valIndex < numVals ? parameterVals[valIndex] : null;
                db.SetParameterValue(dbCommand, dbCommand.Parameters[i].ParameterName, value);
            }

            return dbCommand;
        }
    }
}
