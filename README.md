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


# Documentation
### Docker
#### Running Docker Containers with Docker Compose

There is a network that contains two containers: one with the MySQL image and the other with the phpMyAdmin image. To run the Docker containers, execute the file `docker-compose.yml`.

#### To Execute `docker-compose.yml` File

Use the following command in the terminal:

```bash
docker-compose up -d
```

## Controllers
This documentation will specify only `TripsController` as other controllers are based on this class, page pagination and error handling. All controllers ar hierarchical.

## Table of Contents
- [Overview](#overview)
- [Endpoints](#endpoints)
  - [GetTrips](#gettrips)
  - [GetTrip](#gettrip)
  - [CreateTrip](#createtrip)
  - [EditTrip](#edittrip)
  - [DeleteTrip](#deletetrip)
- [Models](#models)
- [Pagination Metadata](#pagination-metadata)
- [Error Handling](#error-handling)

## Overview

The `TripsController` exposes RESTful endpoints for managing trips. It supports pagination for trip listings and provides links to resources for easy navigation.

## Endpoints

### GetTrips

- **URL:** `/api/v1/trips`
- **Method:** `GET`
- **Query Parameters:**
  - `pageNumber` (int, required): The page number to retrieve.
  - `pageSize` (int, required): The number of trips per page.

- **Description:** Retrieves a paginated list of trips sorted by creation date.

- **Response:**
  - **Status Code:** `200 OK`
  - **Body:**
    ```json
    [
      {
        "id": 1,
        "name": "Trip to the mountains",
        "description": "A relaxing trip to the mountains.",
        "tripStart": "2024-01-01T00:00:00Z",
        "tripEnd": "2024-01-07T00:00:00Z",
        "creationDate": "2023-12-01T12:00:00Z"
      }
    ]
    ```

### GetTrip

- **URL:** `/api/v1/trips/{tripId}`
- **Method:** `GET`
- **Path Parameters:**
  - `tripId` (int, required): The ID of the trip to retrieve.

- **Description:** Retrieves a specific trip by its ID.

- **Response:**
  - **Status Code:** `200 OK`
  - **Body:**
    ```json
    {
      "id": 1,
      "name": "Trip to the mountains",
      "description": "A relaxing trip to the mountains.",
      "tripStart": "2024-01-01T00:00:00Z",
      "tripEnd": "2024-01-07T00:00:00Z",
      "creationDate": "2023-12-01T12:00:00Z"
    }
    ```

### CreateTrip

- **URL:** `/api/v1/trips`
- **Method:** `POST`
- **Body:**
  ```json
  {
    "name": "Trip to the beach",
    "description": "A fun trip to the beach.",
    "tripStart": "2024-01-15T00:00:00Z",
    "tripEnd": "2024-01-20T00:00:00Z"
  }

### EditTrip

- **URL:** `/api/v1/trips/{tripId}`
- **Method:** `PUT`
- **Path Parameters:**
  - `tripId` (int, required): The ID of the trip to update.

- **Request Body:**
  ```json
  {
    "name": "Updated Trip Name",
    "description": "Updated description.",
    "tripStart": "2024-01-01T00:00:00Z",
    "tripEnd": "2024-01-10T00:00:00Z"
  }

### DeleteTrip

- **URL:** `/api/v1/trips/{tripId}`
- **Method:** `DELETE`
- **Path Parameters:**
  - `tripId` (int, required): The ID of the trip to delete.

- **Description:** Deletes a trip by its ID.

- **Response:**
  - **Status Code:** `204 No Content`

### Example Response

- If the deletion is successful, there will be no content in the response body.

```http
HTTP/1.1 204 No Content
```

## Models

### TripDtoInitial

- **Description:** Represents the data of a trip when retrieved from the API.
- **Properties:**
  - `id` (int): The unique identifier of the trip.
  - `name` (string): The name of the trip.
  - `description` (string): A brief description of the trip.
  - `tripStart` (DateTime): The start date and time of the trip.
  - `tripEnd` (DateTime): The end date and time of the trip.
  - `creationDate` (DateTime): The date and time when the trip was created.

#### Example

```json
{
  "id": 1,
  "name": "Trip to the beach",
  "description": "A fun trip to the beach.",
  "tripStart": "2024-01-15T00:00:00Z",
  "tripEnd": "2024-01-20T00:00:00Z",
  "creationDate": "2023-12-01T12:00:00Z"
}

```

## Pagination Metadata

- **Description:** Represents metadata for paginated responses in the API.
- **Properties:**
  - `totalCount` (int): The total number of trips available.
  - `pageSize` (int): The number of trips returned per page.
  - `currentPage` (int): The current page number in the pagination.
  - `totalPages` (int): The total number of pages available based on the `totalCount` and `pageSize`.
  - `previousPageLink` (string, nullable): A link to the previous page (if available).
  - `nextPageLink` (string, nullable): A link to the next page (if available).

### Example

```json
{
  "totalCount": 100,
  "pageSize": 5,
  "currentPage": 1,
  "totalPages": 20,
  "previousPageLink": null,
  "nextPageLink": "/paths/v1/trips?pageNumber=2&pageSize=5"
}
```

## Error Handling

- **Description:** The API utilizes standard HTTP status codes to indicate the success or failure of requests. Detailed error messages are returned in the response body for various types of errors, allowing clients to understand the issues encountered.

### Common HTTP Status Codes

- **400 Bad Request:** Indicates that the request was malformed or contained validation errors.

#### Example Response

```json
{
  "errors": {
    "Name": [
      "The Name field is required."
    ],
    "TripStart": [
      "The TripStart field must be a valid date."
    ]
  }
}
```

### Travel Planner OpenAPI Documentation

```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "Travel Planner API",
    "version": "v1"
  },
  "paths": {
    "/api/v1/trips/{tripId}/destinations/{destinationId}/Activities": {
      "get": {
        "tags": [
          "Activities"
        ],
        "operationId": "GetActivities",
        "parameters": [
          {
            "name": "PageNumber",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string"
            }
          },
          {
            "name": "destinationId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "post": {
        "tags": [
          "Activities"
        ],
        "operationId": "CreateActivity",
        "parameters": [
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "destinationId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateActivityDto"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateActivityDto"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/CreateActivityDto"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/v1/trips/{tripId}/destinations/{destinationId}/Activities/{activitiesId}": {
      "get": {
        "tags": [
          "Activities"
        ],
        "operationId": "GetActivity",
        "parameters": [
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "destinationId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "activitiesId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "put": {
        "tags": [
          "Activities"
        ],
        "operationId": "EditActivity",
        "parameters": [
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "destinationId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "activitiesId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateActivityDto"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateActivityDto"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateActivityDto"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "delete": {
        "tags": [
          "Activities"
        ],
        "operationId": "DeleteActivity",
        "parameters": [
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "destinationId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "activitiesId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/v1/trips/{tripId}/Destinations": {
      "get": {
        "tags": [
          "Destinations"
        ],
        "operationId": "GetDestinations",
        "parameters": [
          {
            "name": "PageNumber",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "PageSize",
            "in": "query",
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "string"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "post": {
        "tags": [
          "Destinations"
        ],
        "operationId": "CreateDestination",
        "parameters": [
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateDestinationDto"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateDestinationDto"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/CreateDestinationDto"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    },
    "/api/v1/trips/{tripId}/Destinations/{destinationId}": {
      "get": {
        "tags": [
          "Destinations"
        ],
        "operationId": "GetDestination",
        "parameters": [
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "destinationId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "put": {
        "tags": [
          "Destinations"
        ],
        "operationId": "EditDestination",
        "parameters": [
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "destinationId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateDestinationDto"
              }
            },
            "text/json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateDestinationDto"
              }
            },
            "application/*+json": {
              "schema": {
                "$ref": "#/components/schemas/UpdateDestinationDto"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      },
      "delete": {
        "tags": [
          "Destinations"
        ],
        "operationId": "DeleteDestination",
        "parameters": [
          {
            "name": "tripId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          },
          {
            "name": "destinationId",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "OK"
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "CreateActivityDto": {
        "type": "object",
        "properties": {
          "Name": {
            "type": "string"
          },
          "Description": {
            "type": "string"
          },
          "Price": {
            "type": "number",
            "format": "double"
          },
          "StartDate": {
            "type": "string",
            "format": "date-time"
          },
          "EndDate": {
            "type": "string",
            "format": "date-time"
          }
        }
      },
      "UpdateActivityDto": {
        "type": "object",
        "properties": {
          "Name": {
            "type": "string"
          },
          "Description": {
            "type": "string"
          },
          "Price": {
            "type": "number",
            "format": "double"
          },
          "StartDate": {
            "type": "string",
            "format": "date-time"
          },
          "EndDate": {
            "type": "string",
            "format": "date-time"
          }
        }
      },
      "CreateDestinationDto": {
        "type": "object",
        "properties": {
          "Name": {
            "type": "string"
          },
          "Country": {
            "type": "string"
          },
          "City": {
            "type": "string"
          }
        }
      },
      "UpdateDestinationDto": {
        "type": "object",
        "properties": {
          "Name": {
            "type": "string"
          },
          "Country": {
            "type": "string"
          },
          "City": {
            "type": "string"
          }
        }
      }
    }
  }
}
```
