public static void RegisterValidators(this Container container, ReuseScope scope, params Assembly[] assemblies)
{
    foreach (var assembly in assemblies)
    {
        foreach (var validator in assembly.GetTypes()
        .Where(t => t.IsOrHasGenericInterfaceTypeOf(typeof(IValidator<>))))
        {
            RegisterValidator(container, validator, scope);
        }
    }
}

public static void RegisterValidator(this Container container, Type validator, ReuseScope scope=ReuseScope.None)
{
    var baseType = validator.BaseType;
    if (validator.IsInterface || baseType == null) return;
    while (!baseType.IsGenericType)
    {
        baseType = baseType.BaseType;
    }

    var dtoType = baseType.GetGenericArguments()[0];
    var validatorType = typeof(IValidator<>).MakeGenericType(dtoType);

    container.RegisterAutoWiredType(validator, validatorType, scope);
}
