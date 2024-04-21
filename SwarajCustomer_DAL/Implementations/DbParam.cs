using System;
using System.Data;

namespace SwarajCustomer_DAL.Implementations
{
    /// <summary>
    /// class used to pass the database parameters
    /// </summary>
    public class DbParam
    {
        private object _ParamValue;

        /// <summary>
        /// default constructor for DbParam class
        /// </summary>
        public DbParam() { }

        /// <summary>
        /// overloaded constructor for DbParam class
        /// </summary>
        /// <param name="paramName">Parameter Name</param>
        /// <param name="paramValue">Parameter Value</param>
        /// <param name="paramType">Parameter Type</param>
        public DbParam(string paramName, object paramValue, SqlDbType paramType) : this()
        {
            this.ParamDirection = ParameterDirection.Input;
            this.ParamName = paramName;
            this.ParamType = paramType;
            this.ParamValue = paramValue;
        }

        /// <summary>
        /// overloaded constructor for DbParam class
        /// </summary>
        /// <param name="paramName">Parameter Name</param>
        /// <param name="paramValue">Parameter Value</param>
        /// <param name="paramSourceColumn">Parameter Source Columns</param>
        /// <param name="paramType">Parameter Type</param>
        public DbParam(string paramName, string paramValue, string paramSourceColumn, SqlDbType paramType) : this(paramName, paramValue, paramType)
        {
            this.ParamSourceColumn = paramSourceColumn;
        }

        /// <summary>
        /// overloaded constructor for DbParam class
        /// </summary>
        /// <param name="paramName">Parameter Name</param>
        /// <param name="paramValue">Parameter Value</param>
        /// <param name="paramType">Parameter Type</param>
        /// <param name="paramDirection">Parameter Direction</param>
        public DbParam(string paramName, string paramValue, SqlDbType paramType, ParameterDirection paramDirection) : this(paramName, paramValue, paramType)
        {
            this.ParamDirection = paramDirection;
        }

        /// <summary>
        /// overloaded constructor for DbParam class
        /// </summary>
        /// <param name="paramName">Parameter Name</param>
        /// <param name="paramValue">Parameter Value</param>
        /// <param name="paramSourceColumn">Parameter Source Columns</param>
        /// <param name="paramType">Parameter Type</param>
        /// <param name="paramDirection">Parameter Direction</param>
        /// <param name="Size">Size</param>
        public DbParam(string paramName, string paramValue, string paramSourceColumn, SqlDbType paramType, ParameterDirection paramDirection, int Size) : this(paramName, paramValue, paramType, paramDirection)
        {
            this.ParamSourceColumn = paramSourceColumn;
            this.Size = Size;
        }

        /// <summary>
        /// gets or sets the parameter name
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// gets or sets the parameter value
        /// </summary>
        public object ParamValue
        {
            get { return _ParamValue; }
            set
            {
                if (value == null)
                {
                    _ParamValue = DBNull.Value;
                }
                else
                {
                    _ParamValue = value;
                }
            }
        }

        /// <summary>
        /// gets or sets the parameter source column
        /// </summary>
        public string ParamSourceColumn { get; set; }

        /// <summary>
        /// gets or sets the parameter type
        /// </summary>
        public SqlDbType ParamType { get; set; }

        /// <summary>
        /// gets or sets the parameter direction
        /// </summary>
        public ParameterDirection ParamDirection { get; set; }

        /// <summary>
        /// Get or set Size
        /// </summary>
        public int Size { get; set; }
    }
}