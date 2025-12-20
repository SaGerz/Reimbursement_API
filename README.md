# Reimbursement Management API

Backend REST API for a reimbursement management system with role-based access control.
This project is currently under active development.

---

## Roles
- **Employee**: Submit reimbursement requests
- **Manager**: Approve or reject reimbursement requests
- **Finance**: Process payments *(planned)*

---

## Implemented Features
### Employee
- Submit reimbursement request
- View own reimbursement list
- View reimbursement detail and approval notes

### Manager
- View pending reimbursement requests
- Approve reimbursement
- Reject reimbursement with remarks
- View approval history

---

## Planned Features (Roadmap)
- Finance payment processing
- Upload payment proof
- Financial reports (monthly / per employee)
- Dashboard summary for all roles

---

## Tech Stack
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication

---

## Database Overview
Main tables used in this project:
- Users
- Reimbursements
- Categories
- ApprovalHistories

The system uses:
- **Snapshot state** in `Reimbursements`
- **Audit log** in `ApprovalHistories`

---

## How to Run
1. Clone this repository
2. Update connection string in `appsettings.json`
3. Run database migration

```bash
dotnet ef database update
