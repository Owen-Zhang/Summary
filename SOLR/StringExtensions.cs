public static class StringExtensions
    {
        private static readonly Regex formatWithRegex = new Regex(@"\{\{|\{([\w\.\[\]]+)((?:[,:][^}]+)?\})", RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static int ASCII(this char ch)
        {
            return (int)Encoding.ASCII.GetBytes(ch.ToString())[0];
        }

        #region FormatWith

        public static string FormatWith(this string format, params object[] arguments)
        {
            ArgumentValidator.EnsureArgumentNotNull(format, "format");
            return string.Format(format, arguments);
        }

        public static string FormatWith(this string format, object arg0)
        {
            ArgumentValidator.EnsureArgumentNotNull(format, "format");
            var arguments = new List<object>();
            return string.Format(
                formatWithRegex.Replace(format,
                    m =>
                    {
                        string propertyName = m.Groups[1].Value.Trim();
                        if (propertyName.Length == 0)
                            return m.Value;

                        arguments.Add((propertyName == "0" || propertyName == "this") ? arg0 : Eval(arg0, propertyName));
                        return "{" + (arguments.Count - 1) + m.Groups[2].Value;
                    }),
                arguments.ToArray());
        }

        public static string FormatWith(this string format, IFormatProvider formatProvider, params object[] arguments)
        {
            ArgumentValidator.EnsureArgumentNotNull(format, "format");
            ArgumentValidator.EnsureArgumentNotNull(formatProvider, "formatProvider");
            return string.Format(formatProvider, format, arguments);
        }

        public static string FormatWith(this string format, object arg0, object arg1)
        {
            ArgumentValidator.EnsureArgumentNotNull(format, "format");
            return string.Format(format, arg0, arg1);
        }

        public static string FormatWith(this string format, object arg0, object arg1, object arg2)
        {
            ArgumentValidator.EnsureArgumentNotNull(format, "format");
            return string.Format(format, arg0, arg1, arg2);
        }

        #endregion FormatWith

        public static bool EqualsOrdinalIgnoreCase(this string source, string target)
        {
            return string.Equals(source, target, StringComparison.OrdinalIgnoreCase);
        }

        private static object Eval(object arg, string expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            expression = expression.Trim();
            if (expression.Length == 0)
                throw new ArgumentNullException("expression");

            if (arg == null)
                return null;

            object result = arg;
            foreach (string str in expression.Split('.'))
            {
                if (result == null)
                    return result;

                result = result.GetType().GetProperty(str).GetValue(result, new object[0]);
            }

            return result;
        }

        //public static T JsonToObject<T>(this string jsonStr)
        //{
        //    return JsonConvert.DeserializeObject<T>(jsonStr);
        //}

        //public static T XmlToObject<T>(this string jsonStr)
        //{
        //    return ServiceStack.Text.XmlSimpleSerializer.DeserializeFromString<T>(jsonStr);
        //}

        private const string luceneReservedCharacters = "[+\\-&|!(){}\\[\\]^\"~*?:(\\)]";

        /// <summary>
        /// 对 Solr 中的保留字符进行转移
        /// </summary>
        /// <remarks>Solr中有一些特殊字符在 Solr 中有特殊含义，查询关键字中使用这些特殊符号时，需要经过转义，使用 “\” 来转义这些字符</remarks>
        public static string EscapeSolrQueryChars(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            var ms = new Regex(luceneReservedCharacters).Matches(value);
            if (ms == null || ms.Count <= 0)
                return value;

            string val = string.Empty;

            List<string> escapedList = new List<string>();
            foreach (Match s in ms)
            {
                if (!escapedList.Contains(s.Value))
                {
                    escapedList.Add(s.Value);
                    value = value.Replace(s.Value, "\\" + s.Value);
                }
            }

            return value;
        }


        /// <summary>
        /// 转换为 solr 模糊查询字符串。该方法内已经做了特殊字符转义处理
        /// Sample:
        /// input:"a b c"
        /// </summary>
        /// <param name="input">需要进行模糊匹配的查询关键句,多个单词间用空格作为间隔进行单词拆分（如： a b c），将拆分后的单词分别进行模糊匹配。</param>
        /// <param name="keyname">进行匹配的solr列名</param>
        ///<returns>solr 模糊查询匹配语句。Input： 'a b c' 的返回结果示例：'(CustomerName:"a*" OR CustomerName:"b*" OR CustomerName:"c*")'</returns>
        public static string ToSolrFuzzyQueryString(this string input, string keyname)
        {
            char defualtSplitChar = ' ';
            return input.ToSolrFuzzyQueryString(keyname, defualtSplitChar);
        }

        /// <summary>
        /// 转换为 solr 模糊查询字符串。该方法内已经做了特殊字符转义处理
        /// Sample:
        /// input:"a b c"
        /// </summary>
        /// <param name="input">需要进行模糊匹配的查询关键句,多个单词间用分隔符作为间隔进行单词拆分，将拆分后的单词分别进行模糊匹配。</param>
        /// <param name="split">分隔符</param>
        /// <param name="keyname">进行匹配的solr列名</param>
        ///<returns>solr 模糊查询匹配语句。keyname:'CustomerName'  split:',' Input： 'a,b,c' 的返回结果示例：(CustomerName:"a*" OR CustomerName:"b*" OR CustomerName:"c*")</returns>
        public static string ToSolrFuzzyQueryString(this string input, string keyname, char split)
        {
            if (string.IsNullOrWhiteSpace(keyname) || string.IsNullOrWhiteSpace(input))
                return input;

            string queryString = string.Empty;
            input = input.Trim().EscapeSolrQueryChars();
            var Parts = input.Split(split);
            if (Parts.Length > 1)
            {
                queryString += "(";
                foreach (var part in Parts)
                    queryString += " OR " + keyname + ":\"" + part + "*\"";
                queryString = queryString.Remove(1, 4);
                queryString += ")";
            }
            else
            {
                queryString += keyname + ":\"" + input + "*\"";
            }
            return queryString;
        }


        public static void Split<T>(this string value, ref List<T> target, char separator)
        {
            if (value == null)
            {
                target = null;
                return;
            }

            target = Array.ConvertAll<string, T>(
                value.Trim().Split(separator), s =>
                {
                    return Parse<T>(s.Trim());
                }).ToList();
        }

        private static T Parse<T>(string value)
        {
            // Get default value for type so if string
            // is empty then we can return default value.
            T result = default(T);
            if (!string.IsNullOrWhiteSpace(value))
            {
                // we are not going to handle exception here
                // if you need SafeParse then you should create
                // another method specially for that.
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                result = (T)tc.ConvertFrom(value);
            } return result;
        }


        public static bool IsNumeric(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            int number = -1;
            return int.TryParse(value, out number);
        }
    }
