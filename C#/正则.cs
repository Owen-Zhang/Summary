//使用前面匹配到的东西用\group号
Regex regexNumber = new Regex(@"^(\d+)([A-Z|a-z]+)\1\2$");
            string number = "132368adfdfSUUUY132368adfdfSUUUY";
            if (regexNumber.IsMatch(number))
                Console.WriteLine("success");
            else
                Console.WriteLine("faild");
