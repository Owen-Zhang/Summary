MyFlags myFlag = MyFlags.Flag2 | MyFlags.Flag3;
Console.WriteLine(myFlag);

if ((myFlag & MyFlags.Flag4) == MyFlags.Flag4)
Console.WriteLine("Flag2 exists");
                
                
[FlagsAttribute]
        enum MyFlags
        {
            Flag1 = 0,    //000  
            Flag2 = 1,    //001  
            Flag3 = 2,    //010  
            Flag4 = 4     //100  
        };
