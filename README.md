# Travel planner
Web project for university module **T120B165 Saityno taikomųjų programų projektavimas**.


### System purpose
The purpose of this Travel Planning System is to provide a comprehensive, user-friendly platform for organizing and managing trips. It allows users to plan multi-day itineraries with detailed day plans and activities, while also ensuring flexibility and accessibility for different user roles. By offering hierarchical structuring of trips, day plans, and activities, the system enables users to break down complex travel arrangements into manageable parts, streamlining the entire planning process. Great for travelling enthusiasts and travel agencies.

**System is designed to**: 
- **Simplify trip planning** by enabling users to create, update, and view detailed itineraries.
- **Ensure effective management** of trip-related information, with role-based permissions to ensure appropriate access and control over trip data.
- **Encourage efficient organization of travel experiences**, from high-level trip details down to specific day-by-day activities.
  

### Functional requirements
#### User roles and authentication
- The system must allow to register with a unique email and password.
- The system must allow user to login with JWT or oauth2 authenticaton.
- System will support three roles: **Guest**, **member** and **administrator**.

#### Guest
- Can only view public trips, destinations and activities.
- Can view other people made trips.

#### Member
- Can create, update, delete their own trips, activities, destinations.

#### Administrator
- Can perform all CRUD operations on member's data.
- Can give roles to other users.

#### Trip management
- The system must allow a **Member** or **Administrator** to create a new trip by entering details such as trip name, description, start date, and end date.
-  The system must allow users to view a list of all trips.
-  The system must allow Members to view the details of a specific trip.
-  The system must allow a Member to update the details of their own trip.
-  The system must allow a Member to delete their own trip.
-  The system must allow an Administrator to update or delete any user's trip.

#### Activity management
- The system must allow a Member or Administrator to add one or more activities to a specific destination.
- The system must allow users to view a list of all activities for a specific destination.
- The system must allow users to view the details of a specific activity within a destination.
- The system must allow a Member to update the details of their own activity within a destination.
- The system must allow a Member to delete their own activity within a destination.
- The system must allow an Administrator to update or delete any activity within any destination.

### UML Diagram
![UMLDiagram](https://github.com/user-attachments/assets/1ebc95a7-4052-42bb-a3a9-4ddcad8b635c)

### Technologies used
- Programming language: C#, .NET.
- Database: MySQL.
- Front-end: Vue.js.
- Server: Docker.


## Documentation
### Docker
#### Running Docker Containers with Docker Compose

There is a network that contains two containers: one with the MySQL image and the other with the phpMyAdmin image. To run the Docker containers, execute the file `docker-compose.yml`.

#### To Execute `docker-compose.yml` File

Use the following command in the terminal:

```bash
docker-compose up -d

