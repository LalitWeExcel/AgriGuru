using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace SwarajCustomer_DAL.Implementations
{
    /// <summary>
    /// class that manages all lower level ADO.NET data base access.
    /// </summary>
    public static class Db
    {
        private static readonly string connectionString = SwarajCustomer_Common.CommonMethods.ConStr;

        #region Data Update handlers

        /// <summary>
        /// Executes Update statements in the database.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <param name="getId"></param>
        /// <returns></returns>
        public static int Update(string spName, DbParam[] spParams, bool getId)
        {
            using (DbConnection connection = (new SqlConnection()))
            {
                int retValue = 0;
                connection.ConnectionString = connectionString;

                using (DbCommand command = (new SqlCommand()))
                {
                    command.Connection = connection;
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 180;

                    if (spParams != null)
                        AssignParameters(command, spParams);

                    connection.Open();

                    try
                    {
                        if (getId)
                        {
                            SqlParameter spParameter = new SqlParameter("lngReturn", SqlDbType.Int);
                            spParameter.Direction = ParameterDirection.ReturnValue;
                            command.Parameters.Add(spParameter);

                            command.ExecuteNonQuery();
                            retValue = int.Parse(spParameter.Value.ToString());
                        }
                        else
                        {
                            retValue = command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Dispose();
                    }

                    return retValue;
                }
            }
        }

        public static bool Update(DataSet ds, string spInsert, string spUpdate, string spDelete, DbParam[] spParams, bool getId)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            bool blnReturnValue = true;

            using (SqlConnection connection = (new SqlConnection()))
            {
                connection.ConnectionString = connectionString;

                adapter.InsertCommand = (SqlCommand)AssignParameters(new SqlCommand(spInsert), spParams);
                adapter.UpdateCommand = (SqlCommand)AssignParameters(new SqlCommand(spUpdate), spParams);
                adapter.DeleteCommand = (SqlCommand)AssignParameters(new SqlCommand(spDelete), spParams);

                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
                adapter.DeleteCommand.CommandType = CommandType.StoredProcedure;

                adapter.InsertCommand.Connection = connection;
                adapter.UpdateCommand.Connection = connection;
                adapter.DeleteCommand.Connection = connection;
                try
                {
                    connection.Open();
                    adapter.Update(ds, ds.Tables[0].TableName);
                }
                catch (Exception ex)
                {
                    blnReturnValue = false;
                    throw ex;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }

            return blnReturnValue;
        }

        /// <summary>
        /// Executes Update statements in the database.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <returns></returns>
        public static int Update(string spName, DbParam[] spParams)
        {
            return Update(spName, spParams, false);
        }

        public static int Insert(string spName, DbParam[] spParams, bool getId)
        {
            return Insert(spName, spParams, getId, null);
        }

        /// <summary>
        /// Executes Insert statements in the database. Optionally returns new identifier.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <param name="getId"></param>
        /// <returns></returns>
        public static int Insert(string spName, DbParam[] spParams, bool getId, int? Timeout)
        {
            using (DbConnection connection = (new SqlConnection()))
            {
                int retValue = 0;
                connection.ConnectionString = connectionString;

                using (DbCommand command = (new SqlCommand()))
                {

                    command.Connection = connection;
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;

                    if (spParams != null)
                        AssignParameters(command, spParams);
                    if (Timeout.HasValue)
                        command.CommandTimeout = Timeout.Value;
                    else
                        command.CommandTimeout = 180;

                    connection.Open();

                    try
                    {
                        if (getId)
                        {
                            SqlParameter spParameter = new SqlParameter("retval", SqlDbType.Int);
                            spParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(spParameter);

                            command.ExecuteNonQuery();
                            retValue = int.Parse(spParameter.Value.ToString());
                        }
                        else
                        {
                            retValue = command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Dispose();
                    }

                    return retValue;
                }
            }
        }

        /// <summary>
        /// Executes Insert statements in the database.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        public static void Insert(string spName, DbParam[] spParams)
        {
            Insert(spName, spParams, false, null);
        }

        public static int BulkInstert(string spName, DataTable sourceTable, string tableParamName)
        {
            return BulkInstert(spName, sourceTable, tableParamName, null);
        }

        /// <summary>
        /// Executes Bulk Insert and Update statements in the database.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="sourceTable"></param>
        /// <param name="tableParamName"></param>
        /// <returns></returns>
        public static int BulkInstert(string spName, DataTable sourceTable, string tableParamName, int? Timeout)
        {
            using (SqlConnection connection = (new SqlConnection()))
            {
                int retValue = 0;
                connection.ConnectionString = connectionString;

                using (SqlCommand command = (new SqlCommand()))
                {
                    command.Connection = connection;
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(tableParamName, sourceTable);
                    if (Timeout.HasValue)
                        command.CommandTimeout = Timeout.Value;
                    else
                        command.CommandTimeout = 180;
                    connection.Open();

                    try
                    {
                        retValue = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Dispose();
                    }

                    return retValue;
                }
            }
        }

        public static int BulkInstert2(string spName, DataTable sourceTable, string tableParamName)
        {
            return BulkInstert2(spName, sourceTable, tableParamName, null);
        }

        public static int BulkInstert2(string spName, DataTable sourceTable, string tableParamName, int? Timeout)
        {
            using (SqlConnection connection = (new SqlConnection()))
            {
                int retValue = 0;
                connection.ConnectionString = connectionString;

                using (SqlCommand command = (new SqlCommand()))
                {
                    command.Connection = connection;
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(tableParamName, sourceTable);

                    SqlParameter spParameter = new SqlParameter("retval", SqlDbType.Int);
                    spParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(spParameter);
                    if (Timeout.HasValue)
                        command.CommandTimeout = Timeout.Value;
                    else
                        command.CommandTimeout = 180;
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                        retValue = int.Parse(spParameter.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Dispose();
                    }

                    return retValue;
                }
            }
        }

        public static int BulkInstertWithParams(string spName, DataTable sourceTable, string tableParamName, DbParam[] spParams)
        {
            return BulkInstertWithParams(spName, sourceTable, tableParamName, spParams, null);
        }

        public static int BulkInstertWithParams(string spName, DataTable sourceTable, string tableParamName, DbParam[] spParams, int? Timeout)
        {
            using (SqlConnection connection = (new SqlConnection()))
            {
                int retValue = 0;
                connection.ConnectionString = connectionString;

                using (SqlCommand command = (new SqlCommand()))
                {
                    command.Connection = connection;
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(tableParamName, sourceTable);

                    if (spParams != null)
                        AssignParameters(command, spParams);

                    SqlParameter spParameter = new SqlParameter("retval", SqlDbType.Int);
                    spParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(spParameter);
                    if (Timeout.HasValue)
                        command.CommandTimeout = Timeout.Value;
                    else
                        command.CommandTimeout = 180;
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                        retValue = int.Parse(spParameter.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        connection.Dispose();
                    }

                    return retValue;
                }
            }
        }

        /// <summary>
        ///  Executes Bulk Insert and Populates a DataSet according to a stored procedure and parameters.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="sourceTable"></param>
        /// <param name="tableParamName"></param>
        /// <returns></returns>
        public static DataSet GetDataSetBulk(string spName, DataTable sourceTable, string tableParamName)
        {
            return GetDataSetBulk(spName, sourceTable, tableParamName, null);
        }

        /// <summary>
        /// Executes Bulk Insert and Populates a DataSet according to a stored procedure and parameters.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="sourceTable"></param>
        /// <param name="tableParamName"></param>
        /// <returns></returns>
        public static DataSet GetDataSetBulk(string spName, DataTable sourceTable, string tableParamName, int? Timeout)
        {
            using (SqlConnection connection = (new SqlConnection()))
            {
                connection.ConnectionString = connectionString;

                using (SqlCommand command = (new SqlCommand()))
                {
                    command.Connection = connection;
                    command.CommandText = spName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(tableParamName, sourceTable);
                    connection.Open();
                    if (Timeout.HasValue)
                        command.CommandTimeout = Timeout.Value;
                    else
                        command.CommandTimeout = 180;

                    using (DbDataAdapter adapter = (new SqlDataAdapter()))
                    {
                        adapter.SelectCommand = command;

                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        return ds;
                    }
                }
            }
        }
        #endregion

        #region Data Retrieve Handlers

        /// <summary>
        ///  Populates a DataSet according to a stored procedure and parameters.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string spName, DbParam[] spParams)
        {
            return GetDataSet(spName, spParams, 180);
        }

        /// <summary>
        /// overloaded method to populate a DataSet according to a stored procedure and parameters.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string spName, DbParam[] spParams, int? Timeout)
        {
            using (DbConnection connection = (new SqlConnection()))
            {
                connection.ConnectionString = connectionString;

                using (DbCommand command = (new SqlCommand()))
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spName;
                    if (Timeout.HasValue)
                        command.CommandTimeout = Timeout.Value;
                    else
                        command.CommandTimeout = 500;

                    if (spParams != null)
                    {
                        AssignParameters(command, spParams);
                    }

                    using (DbDataAdapter adapter = (new SqlDataAdapter()))
                    {
                        adapter.SelectCommand = command;

                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// overloaded method to populate a DataTable according to a stored procedure and parameters.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string spName, DbParam[] spParams)
        {
            DataTable dt = null;
            DataSet ds = GetDataSet(spName, spParams);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }

        /// <summary>
        /// overloaded method to populate a DataRow according to a stored procedure and parameters.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <returns></returns>
        public static DataRow GetDataRow(string spName, DbParam[] spParams)
        {
            DataRow row = null;

            DataTable dt = GetDataTable(spName, spParams);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    row = dt.Rows[0];
                }
            }

            return row;
        }

        /// <summary>
        /// Executes a stored procedure and returns a scalar value.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <returns></returns>
        public static object GetScalar(string spName, DbParam[] spParams, int? Timeout)
        {
            using (DbConnection connection = (new SqlConnection()))
            {
                connection.ConnectionString = connectionString;

                using (DbCommand command = (new SqlCommand()))
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spName;

                    if (spParams != null)
                    {
                        AssignParameters(command, spParams);
                    }
                    if (Timeout.HasValue)
                        command.CommandTimeout = Timeout.Value;
                    else
                        command.CommandTimeout = 180;
                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Assigns paramets to DbCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="spParams"></param>
        /// <returns></returns>
        private static DbCommand AssignParameters(DbCommand command, DbParam[] spParams)
        {
            if (spParams != null)
            {
                foreach (DbParam spParam in spParams)
                {
                    SqlParameter spParameter = new SqlParameter();

                    spParameter.SqlDbType = spParam.ParamType;
                    spParameter.ParameterName = spParam.ParamName;
                    spParameter.Value = spParam.ParamValue;
                    spParameter.Direction = spParam.ParamDirection;
                    spParameter.SourceColumn = spParam.ParamSourceColumn;
                    spParameter.Size = spParam.Size;

                    command.Parameters.Add(spParameter);
                }
            }

            return command;
        }

        /// <summary>
        /// Escapes an input string for database processing, that is, 
        /// surround it with quotes and change any quote in the string to 
        /// two adjacent quotes (i.e. escape it). 
        /// If input string is null or empty a NULL string is returned.
        /// </summary>
        /// <param name="s">Input string.</param>
        /// <returns>Escaped output string.</returns>
        public static string Escape(string s)
        {
            if (String.IsNullOrEmpty(s))
                return "NULL";
            else
                return "'" + s.Trim().Replace("'", "''") + "'";
        }

        /// <summary>
        /// Escapes an input string for database processing, that is, 
        /// surround it with quotes and change any quote in the string to 
        /// two adjacent quotes (i.e. escape it). 
        /// Also trims string at a given maximum length.
        /// If input string is null or empty a NULL string is returned.
        /// </summary>
        /// <param name="s">Input string.</param>
        /// <param name="maxLength">Maximum length of output string.</param>
        /// <returns>Escaped output string.</returns>
        public static string Escape(string s, int maxLength)
        {
            if (String.IsNullOrEmpty(s))
                return "NULL";
            else
            {
                s = s.Trim();
                if (s.Length > maxLength) s = s.Substring(0, maxLength - 1);
                return "'" + s.Trim().Replace("'", "''") + "'";
            }
        }

        /// <summary>
        /// converts an object to double value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            double retValue = 0;

            if (value != DBNull.Value)
            {
                double.TryParse(value.ToString(), out retValue);
            }

            return retValue;
        }

        /// <summary>
        /// converts an object to double value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object value)
        {
            decimal retValue = 0;

            if (value != DBNull.Value)
            {
                decimal.TryParse(value.ToString(), out retValue);
            }

            return retValue;
        }

        /// <summary>
        ///  converts an object to integer value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInteger(object value)
        {
            int retValue = 0;

            if (value != DBNull.Value)
            {
                int.TryParse(value.ToString(), out retValue);
            }

            return retValue;
        }

        /// <summary>
        ///  converts an object to long value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object value)
        {
            long retValue = 0;

            if (value != DBNull.Value)
            {
                long.TryParse(value.ToString(), out retValue);
            }

            return retValue;
        }

        /// <summary>
        ///  converts an object to string value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            string retValue = string.Empty;

            if (value != DBNull.Value)
            {
                retValue = Convert.ToString(value);
            }

            return retValue;
        }

        /// <summary>
        ///  converts an object to char value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns> 
        public static char ToChar(object value)
        {
            char retValue = ' ';

            if (value != DBNull.Value)
            {
                char.TryParse(value.ToString(), out retValue);
            }

            return retValue;
        }

        /// <summary>
        ///  converts an object to boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(object value)
        {
            bool retValue = false;

            if (value != DBNull.Value)
            {
                bool.TryParse(value.ToString(), out retValue);
            }

            return retValue;
        }

        /// <summary>
        ///  converts an object to datetime value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value)
        {
            DateTime retValue = new DateTime();

            if (value != DBNull.Value)
            {
                DateTime.TryParse(value.ToString(), out retValue);
            }

            return retValue;
        }

        #endregion
    }
}