System.Collections.ArrayList fruits = new System.Collections.ArrayList(4);
fruits.Add("Mango");
fruits.Add("Orange");
fruits.Add("Apple");
fruits.Add(3.0);
fruits.Add("Banana");


IEnumerable<string> query1 = fruits.OfType<string>();
foreach (string fruit in query1)
{
    Console.WriteLine(fruit);
}

/*result: 
Mango
Orange
Apple
Banana
*/

/*
可以应用于GetAttributes()之后.OfType<ABSAttribute>(), 只查询应用了某种Attribute的
*/

