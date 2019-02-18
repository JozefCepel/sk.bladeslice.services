namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FilterOperator
    {
        public static readonly FilterOperator Eq = new FilterOperator("=");     //Equal
        public static readonly FilterOperator Ne = new FilterOperator("<>");    //<> is the valid SQL according to the SQL-92 standard. http://www.contrib.andrew.cmu.edu/~shadow/sql/sql1992.txt
        public static readonly FilterOperator Gt = new FilterOperator(">");     //GreaterThan
        public static readonly FilterOperator Ge = new FilterOperator(">=");    //GreaterThanOrEqual
        public static readonly FilterOperator Lt = new FilterOperator("<");     //LessThan
        public static readonly FilterOperator Le = new FilterOperator("<=");    //LessThanOrEqual

        public static readonly FilterOperator Ng = new FilterOperator("!>");
        public static readonly FilterOperator Nl = new FilterOperator("!<");
        public static readonly FilterOperator In = new FilterOperator("IN");
        public static readonly FilterOperator NotIn = new FilterOperator("NOT IN");
        public static readonly FilterOperator Like = new FilterOperator("LIKE");
        public static readonly FilterOperator NotLike = new FilterOperator("NOT LIKE");
        public static readonly FilterOperator Null = new FilterOperator("IS NULL");
        public static readonly FilterOperator NotNull = new FilterOperator("IS NOT NULL");
        public static readonly FilterOperator Div = new FilterOperator("/");
        public static readonly FilterOperator Mul = new FilterOperator("*");
        public static readonly FilterOperator Sub = new FilterOperator("-");
        public static readonly FilterOperator Add = new FilterOperator("+");
        public static readonly FilterOperator Mod = new FilterOperator("MOD");
        public static readonly FilterOperator And = new FilterOperator("AND");
        public static readonly FilterOperator Or = new FilterOperator("OR");
        

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterOperator" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public FilterOperator(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        /// <summary>
        /// Indicates whether the operator respresents either 'IS NULL' or 'IS NOT NULL'
        /// </summary>
        public bool IsNullOperator { get { return Value == "IS NULL" || Value == "IS NOT NULL"; } }
    }
}