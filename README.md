# PaparaApp Project API

## Overview

PaparaApp Project API is a RESTful web service designed for managing flats, payments, and user interactions within a residential management context. It offers endpoints for tenants and managers to handle payments, dues, flat assignments, and user authentication.

## Features

- **User Authentication**: Separate login endpoints for apartment managers and tenants.
- **Flat Management**: Add, update, and delete flats.
- **Payment Handling**: Process single and bulk payments, manage due and unpaid debts.
- **Tenant Management**: Assign tenants to flats and manage tenant details.


## Usage

After starting the API, you can access the following main endpoints:

- **/api/Flats**: Manage flats (Add, Update, Delete).
- **/api/Payments**: Handle payments and debts for tenants and blocks.
- **/api/Users**: User authentication, tenant creation, and flat assignment.

Use tools like Postman or Swagger to test the API endpoints.

## Authorization

Endpoints are protected with role-based authorization. Ensure you use the appropriate token obtained from the login endpoints to access the protected resources.


