# Wallet System Expense Management API

## Table of Contents
- [Introduction](#introduction)
- [Sequence diagrams](#sequence-diagrams)
- [Entity Relationship Diagram](#entity-relationship-diagram)

## Introduction
This repository contains the source code of small wallet system.

## Sequence diagrams
### Create Expense
```mermaid
sequenceDiagram
    actor Donald
    Donald->>+ExpenseAPI: POST Expense/Create
    ExpenseAPI->>+IExpenseManager: CreateAsync(ExpenseCreation)
    IExpenseManager->>+IUserManager: FindByIdAsync(int)
    IUserManager->>+IUserRepository: FindAsync(UserQuerry)
    IUserRepository-->>-IUserManager: 
    IUserManager-->>-IExpenseManager: 
    IExpenseManager->>+IExpenseRepository: FindAsync(UserQuerry)
    IExpenseRepository-->>-IExpenseManager: 
    IExpenseManager->>+IExpenseRulesValidator: Validate(ExpenseValidationRequest)
    loop IExpenseRulesValidator to IExpenseRuleCheck
    IExpenseRulesValidator->>+IExpenseRuleCheck: Check(ExpenseValidationRequest)
    IExpenseRuleCheck-->>-IExpenseRulesValidator: 
    end
    IExpenseRulesValidator-->>-IExpenseManager: 
    IExpenseManager->>+IExpenseRepository: AddAsync(ExpenseCreation)
    IExpenseRepository-->>-IExpenseManager: 
    IExpenseManager-->>-ExpenseAPI: 
    ExpenseAPI-->>-Donald:  
```
### Search Expense
```mermaid
sequenceDiagram
    actor Donald
    Donald->>+ExpenseAPI: GET Expense/Search
    ExpenseAPI->>+IExpenseManager: FindAsync(ExpenseQuery)
    IExpenseManager->>+IExpenseRepository: FindAsync(ExpenseQuery)
    IExpenseRepository-->>-IExpenseManager: 
    IExpenseManager-->>-ExpenseAPI: 
    ExpenseAPI-->>-Donald : 
```

## Entity Relationship Diagram
```mermaid
erDiagram
    User{
        int Id PK
        string FirstName
        string LastName
        int CurrencyId FK
    }
    User }o--|| Currency: has
    User ||--o{ Expense: has    
    Currency{
        int Id PK
        string Code
        string Name
        string Symbol
    } 
    Expense }o--|| ExpenseType : is
    Expense {
        int Id PK
        datetime2 DateTime
        string Description
        int TypeId FK
        int UserId FK
        decimal Amount
    }
    ExpenseType {
        int Id PK
        string Label
    }
```
