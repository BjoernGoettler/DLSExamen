# Availability

One way of defining availability is as a measurement for how often a service answers when requested.
In a perfect world any service will answer any time we request it.

A SLA is a formal way of agreeing on availability

### Why?

- It's an architectural principle (see [principles](1_PRINCIPLES_FOR_LARGE_SYSTEMS.md))

### How?

- An X-axis split is obvious (see [Scalability Cube](4_SCALABILITY_CUBE.md))

## Pros/Cons

### Pros

- Predictable systems
- Happy users

### Cons

- We need additional resources
- Complexity, and therefore maintainance goes up

### Demo

PongerService is easy to scale via Docker Compose
