# Agenda BRD

Business Requirements Document

[[_TOC_]]

## Use Case Diagram

![Agenda](/uploads/e079ba64215105858cdb52d32a3ccd36/Agenda.jpg)

### Truth Table

(Y) Full; (L) Limited; (R) Read-only

| Module | Admin | User | Dev | Anonymous |
|---|---|---|---|---|
| Account | Y | Y | Y |  |
| Attendance | Y | L | L |  |
| Authentication |  |  |  | Y |
| Dashboard | Y |  |  |  |
| Event Category | Y | R | R | R |
| Event | Y | R | L |  |
| Group Category | Y | R | R | R |
| Location | Y | R | R | R |
| Member | Y |  | Y |  |

## Workflow Diagram

![workflow](/uploads/013d3876a865aaa6969770db273100e9/workflow.jpg)

## Flow Charts

### Registration

![flowchart_-_registration](/uploads/130466a1a1be746b0c36ad1cf51d3630/flowchart_-_registration.jpg)

### Calendar

![Agenda](/uploads/2b92878ea84abbd1b32921c52a2e2e2b/Agenda.jpg)