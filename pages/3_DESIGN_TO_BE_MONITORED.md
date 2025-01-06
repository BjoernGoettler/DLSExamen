# Design to be monitored

### What to do
- Add monitoring from the beginning
- Centralize monitoring

### Monitoring
- Events
- Spans
- Traces


```csharp
public int stringToNumber(string textualNumer)
{
    Log.Information("This is the beginning of a span")

    Log.Information("This is an event")
    var number = (int) tectualNumber;

    Log.Information("The span ends here")
    return number;
}

public int addNumbers(int first, int second)
{
    Log.Information("This is another span")

    Log.Information("This is another event")
    var result = first + second;

    Log.Information("The other span ends now")
    return result;
}

public void main()
{
    Log.Information("This is the start of a trace")

    Log.Information("This is a main event")
    var result = addNumber(stringToNumber('2'), stringToNumer('3'));

    console.WriteLine(result);
    Log.Information("The trace ends here")
}
```

### Loglevels
- Verbose
- Debug
- Information
- Warning
- Error
- Fatal

### Tools for monitoring
- Seq is a centralizes loggin service
- Zipkin can keep track of traces. Jaeger is another example

## Demo
-   MonitorService