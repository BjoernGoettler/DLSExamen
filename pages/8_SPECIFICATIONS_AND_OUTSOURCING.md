# Specifications and Outsourcing

## Outsourcing

- Aquire skill(s) you dont have yourself
- Remove work from yourself


## Specifications

### Unformal specifikation

Unformal specifications leaves the interpretation in the hand of the developer

Pros:
-   You don't need to know programming yourself
-   It opens up for a dialog between developer and outsorcer

Cons:
- It opens up for a potential "blame game"
- Misunderstandings might be caught late, if at all


```csharp
I need a method that converts the hour of a date to a number

int ReturnHourFromDate(someDate);

```

### Formal specifications
Formal specifications dictate most to the developer

Pros:
- You can write your test before accepting code
- Misunderstandings can be caught early

Cons:
- It is quite time consuming
- The specification can become so detailed, that writing the code instead can seem simpler

```csharp
//Only allow existing dates as input
// Allowed date span is January 1st 2024 to December 31st 2024
int ReturnHourFromDate(DateTime existingDate)
{
    //Return the hour in 24 Hour format
    //Make sure the hour is based in Denmark

    return existingDate.24HourFormat;
}
```
