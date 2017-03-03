private static string SplitWithChar(decimal total)
        {
            //ToString("#,##0.00") 这样也可以达到那样的效果

            var result = string.Empty;

            if (total == 0) return "0";
            result = total.ToString().Trim();
            int subIndex = result.IndexOf('-');
            string prefix = string.Empty;
            if (subIndex == 0)
            {
                result = result.Substring(1);
                prefix = "-";
            }

            int pointIndex = result.IndexOf('.');
            string pointData = string.Empty;
            if (pointIndex > 0)
            {
                var temp = result;
                result = temp.Substring(0, pointIndex);
                pointData = temp.Substring(pointIndex);
            }

            int splitCount = (result.Length - 1) / 3;
            int splitLeft = result.Length % 3;
            int splitStart = splitLeft == 0 ? 3 : splitLeft;

            StringBuilder positiveStr = new StringBuilder(result);
            for (int i = 1; i <= splitCount; i++)
            {
                positiveStr.Insert(splitStart + 3 * (splitCount - i), ",");
            }

            if (!string.IsNullOrEmpty(pointData))
                positiveStr.Append(pointData);

            if (!string.IsNullOrEmpty(prefix))
                positiveStr.Insert(0, prefix);

            return positiveStr.ToString();
        }
