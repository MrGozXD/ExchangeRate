# ExchangeRate

## Project Overview

The ExchangeRate project is a C# application that fetches exchange rates from the Frankfurter API and exports them in multiple file formats (JSON, CSV, XML, and sequential/fixed-width format).

## Class Diagram

```mermaid
classDiagram
    class Program {
        -Main(string[] args) Task
    }

    class ExchangeRateService {
        -httpClient : HttpClient
        +GetExchangeRateAsync(baseCurrency: string) Task~List~RateResponse~~
    }

    class RateResponse {
        +Date : string
        +Base : string
        +Quote : string
        +Rate : decimal
    }

    class File {
        #baseFileName : string
        #extension : string
        +BaseFileName : string
        +Extension : string
        +FullFileName : string
        +FullFilePath : string
        +GetProjectRoot()* string
        +WriteAsync(rateResponses: List~RateResponse~)* Task
    }

    class FileJSON {
        +FileJSON(date: DateTime)
        +WriteAsync(rateResponses: List~RateResponse~)* Task
    }

    class FileCSV {
        +FileCSV(date: DateTime)
        +WriteAsync(rateResponses: List~RateResponse~)* Task
    }

    class FileXML {
        +FileXML(date: DateTime)
        +WriteAsync(rateResponses: List~RateResponse~)* Task
    }

    class RatesOutput {
        +RateItems : List~RateItem~
    }

    class RateItem {
        +FromTo : string
        +Rate : decimal
    }

    class FileSequential {
        +FileSequential(date: DateTime)
        +WriteAsync(rateResponses: List~RateResponse~)* Task
    }

    %% Relationships
    Program ..> ExchangeRateService : uses
    Program ..> FileJSON : creates
    Program ..> FileCSV : creates
    Program ..> FileXML : creates
    Program ..> FileSequential : creates
    ExchangeRateService --> RateResponse : returns
    
    FileJSON --|> File : inherits
    FileCSV --|> File : inherits
    FileXML --|> File : inherits
    FileSequential --|> File : inherits
    
    FileXML --> RatesOutput : uses
    RatesOutput --> RateItem : contains

    %% Styling
    class File:::abstract
    class RateResponse:::model
    class RatesOutput:::model
    class RateItem:::model
    
    classDef abstract stroke:#d9534f,stroke-width:2px,stroke-dasharray: 5 5
    classDef model stroke:#0275d8,stroke-width:2px,fill:#e7f3ff
```

## Architecture

### Core Components

- **ExchangeRateService**: Handles API communication with Frankfurter API to fetch exchange rate data
  - Inner class `RateResponse` represents a single exchange rate with date, base currency, quote currency, and rate value

- **File (Abstract Base Class)**: Defines the contract for file writers
  - Manages file naming convention: `Cotations-YYYYMMDD.<extension>`
  - Provides abstract method `WriteAsync()` for subclasses to implement format-specific serialization

- **File Format Implementations**:
  - **FileJSON**: Exports rates as JSON objects
  - **FileCSV**: Exports rates as comma-separated values with header
  - **FileXML**: Exports rates as XML with nested structure
  - **FileSequential**: Exports rates as fixed-width format with padded integers and decimals

- **Program**: Entry point that orchestrates the workflow
  - Fetches exchange rates via `ExchangeRateService`
  - Creates writer instances for all supported formats
  - Executes async write operations
