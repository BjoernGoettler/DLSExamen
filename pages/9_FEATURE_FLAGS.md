# Feature flags

Fullfills "Design to be turned off"

## Implementations
### Native
dotnet offers it out of the box

```csharp
using Microsoft.FeatureManagement;

builder.Services.AddFeatureManagement();
```

Can be configured
```json
{
    "FeatureManagement": {
        "FeatureA": true, // Feature flag set to on
        "FeatureB": false, // Feature flag set to off
        "FeatureC": {
            "EnabledFor": [
                {
                    "Name": "Percentage",
                    "Parameters": {
                        "Value": 50
                    }
                }
            ]
        }
    }
}
```


### Centralised alternative:
FeatureHub is used to demo [Green Software](12_GREEN_SOFTWARE.md)