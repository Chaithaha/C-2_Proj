# CreativeColab 🎮

A collaborative platform that combines game discovery, price tracking, and project management in one unified application.

## What it does

- **🎯 GameFinder**: Search and discover games based on your preferences
- **💰 Price Tracker**: Monitor game prices across different stores
- **📊 Dashboard**: Visual tracking and management of projects, payments, and deadlines
- **👥 Collaboration**: Team-based project management with user roles

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- MySQL (or XAMPP/WAMP)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/Chaithaha/C-2_Proj/
   cd C-2_Proj
   ```

2. **Set up the database**
   - Open phpMyAdmin
   - Create a database called `creativecollabDB`
   - Import `creativecollab_database.sql` from the project root

3. **Configure connection string**
   - Edit `appsettings.json`
   - Update the `DefaultConnection` with your MySQL credentials

4. **Run the application**
   ```bash
   dotnet restore
   dotnet run
   ```

5. **Open in browser**
   - Go to `http://localhost:5193`

## Features

✅ **Games**: Browse, search, and bookmark games  
✅ **Projects**: Create and manage collaborative projects  
✅ **Payments**: Track payments with proper currency formatting  
✅ **Price Monitoring**: Monitor game prices across stores  
✅ **User Management**: Role-based access control  

## Team

- **Pallavi**: Dashboard system
- **Chait**: GameFinder functionality  
- **Dhruv**: Price tracking system

---

*Built with ASP.NET Core 8.0 and MySQL*
