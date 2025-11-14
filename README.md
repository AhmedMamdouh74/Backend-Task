# BackendTask – IP Blocking & GeoLocation API

## 📌 Overview
This project is a .NET Core Web API developed as part of the **Dotnet Developer Test Assignment**.  
It provides endpoints to manage blocked countries, validate IP addresses, and integrate with third‑party geolocation APIs (e.g., ipapi.co or ipgeolocation.io).  
The application uses **in‑memory storage** (no database) and follows a **4‑layer architecture** for clean separation of concerns.

---

## 🚀 Features & Endpoints

### 1. Add a Blocked Country
- **Endpoint:** `POST /api/countries/block`
- **Input:** `{ "countryCode": "US" }`
- **Action:** Adds a country to the blocked list.
- **Validation:** Prevents duplicates.

### 2. Delete a Blocked Country
- **Endpoint:** `DELETE /api/countries/block/{countryCode}`
- **Action:** Removes a country from the blocked list.
- **Error Handling:** Returns `404` if not found.

### 3. Get All Blocked Countries
- **Endpoint:** `GET /api/countries/blocked`
- **Features:** Pagination (`page`, `pageSize`) and search/filter by code or name.

### 4. Find My Country via IP Lookup
- **Endpoint:** `GET /api/ip/lookup?ipAddress={ip}`
- **Action:** Calls third‑party API to fetch country details (code, name, ISP).
- **Validation:** Ensures valid IP format.
- **Note:** If `ipAddress` is omitted, caller IP is used via `HttpContext`.

### 5. Verify If IP is Blocked
- **Endpoint:** `GET /api/ip/check-block`
- **Action:**
  1. Fetch caller IP automatically.
  2. Lookup country via third‑party API.
  3. Check if country is blocked.
  4. Log the attempt.

### 6. Log Blocked Attempts
- **Endpoint:** `GET /api/logs/blocked-attempts`
- **Features:** Paginated list of blocked attempts.
- **Log Entry Includes:** IP, timestamp, country code, blocked status, user agent.

### 7. Temporarily Block a Country
- **Endpoint:** `POST /api/countries/temporal-block`
- **Input:** `{ "countryCode": "EG", "durationMinutes": 120 }`
- **Action:** Blocks a country for a duration (1–1440 minutes).
- **Background Service:** Runs every 5 minutes to remove expired temporal blocks.
- **Validation:** Duration range, valid country code, prevent duplicates.

---

## ⚙️ Project Structure (4 Layers)

