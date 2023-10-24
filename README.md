# data-scraper
This application serves as an automated data collector, fetching data from public APIs and storing them into Azure Storage, specifically within both a Table and a Blob.
Key Features:

- #### Data Fetching & Storage: Every minute, the application fetches a random public API data from a specified endpoint.

  - If the fetch is successful or fails, a log is generated and stored in an Azure Table which details the success or failure status.
  - The complete payload from the API, whether it's data or an error message, is stored within an Azure Blob for record-keeping and potential further analysis.

- #### API Endpoints:

  - Log Retrieval: The application offers a GET API endpoint where users can list all the fetch logs for a specific time frame. By providing a 'from' and 'to' timestamp, users can filter logs to get an idea about the fetch status during that period.
  - Payload Retrieval: Another GET API call is available to retrieve the full payload from the Azure Blob based on a specific log entry. This ensures that users can review the actual data fetched during any logged attempt.
