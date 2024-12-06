# 12: Green software

## Baggrund
Henrik Bærbak Christensen har skrevet et manifest: https://baerbak.cs.au.dk/papers/gaf.html
Energieffektivitet handler om ikke at brænde mere strøm af end nødvendigt. Man kan måle direkte på hvor mange watt en applikation bruger

## Overordnet framework:
### Processer:
- Mål og eksperimentér
- Prioriter "effort"
- Øg opmærksomheden om green software

### Taktikker:
- "Shutdown when idle":
    - Sørg for at services lukker ned, når de ikke er i brug
    - SKAT's kø system kan fx slukkes alt andet end 2x1 uge hvert år

-   Undlas at bruge unødige ressourcer:
    - Lad være med at pingponge over nettet, når det ikke er nødvendigt
    - Overvej hvor online en backup behøver at være
    - Istedet for at køre batch jobs baseret op cron-jobs, kunne de køre når der var overskudsstrøm

- "Bulk Fetch data":
    - Hent flere data end enkelte records:
    - Man kan hente en hel email-tråd på én gang, fremfor at hente hver mail i tråden hver gang

- Brug en effektiv teknologi:
    - Ny hardware yder bedre og bruger mindre strøm


## Diskussion:

De taktiske emner supplerer hinanden:
- Reducer netværkstrafik, øges fx "Bulk Fetch Data"
- Nyere hardware yder bedre, man ny hardware bliver hurtigt gammelt, så hvornår kan man tillade sig at skifte den.
- Man kan overveje at "left shifte" sin hardware. Den udskiftede web server, kan fx få nyt liv som backup-controller, den udskiftede backup-controller kan hoste intra-net osv


## Praktiske exempler:
- Vi har arbejdet med Bloodpressure Measurement, hvor der er konkrete eksempler på implementering med fokus på Green Software