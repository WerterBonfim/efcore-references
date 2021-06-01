# Diagnostics

* 1 - O que é Diagnostic Source?



## 1 - O que é Diagnostic Source?

DiagnosticSource é um padrão observer e seu papal principal é notificar seus
assintantes, ele funciona basicamente como um pipeline de notifcações, e temos o
uso dele basicamente em tudo do ecosistema .NET.



## 2 - Exemplo

```c#

private static void ExemploDiagnostics()
{
    DiagnosticListener.AllListeners.Subscribe(new MyInterceptorListener());

    using var db = new ApplicationContext();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    _ = db.Departamentos
        .Where(x => x.Id > 0)
        .ToArray();
}

public class MyInterceptor : IObserver<KeyValuePair<string, object>>
{
    private static readonly Regex _tableRegex =
        new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))", 
            RegexOptions.Multiline | 
            RegexOptions.IgnoreCase | 
            RegexOptions.Compiled);
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(KeyValuePair<string, object> value)
    {
        if (value.Key == RelationalEventId.CommandExecuting.Name)
        {
            var command = ((CommandEventData) value.Value).Command;
            
            if (command.CommandText.Contains("WITH (NOLOCK)"))
                return;

            command.CommandText = _tableRegex.Replace(
                command.CommandText, 
                "${tableAlias} WITH (NOLOCK)");

            Console.WriteLine(command.CommandText);
        }
    }
}

public class MyInterceptorListener : IObserver<DiagnosticListener>
{
    private readonly MyInterceptor _interceptor = new();
    
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(DiagnosticListener listener)
    {
        //Console.WriteLine(listener.Name);
        
        if (listener.Name == DbLoggerCategory.Name)
        {
            listener.Subscribe(_interceptor);
        }
    }

    
}
```
