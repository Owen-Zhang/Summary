public class Person{
  public int Id {get;set;}
  public string Name {get;set;}
}

public class App{
  
  public void GetValue(){
    var pTemp = new Person() { Id = 1, Name = "aaaaaa"};
    GetPropertyValue<Person, string>(p => p.Name, p);
  }
  
  public void GetPropertyValue<T, TProperty>(Expression<Func<T, TProperty>> expr, T p)
  {
    Console.WriteLine(expr.Body);
    var value = expr.Compile()(p);
  }
}
