from locust import HttpUser, task, between

class ApiUser(HttpUser):
    wait_time = between(0.1, 1)  # Wait time between requests, 0.1 to 1 second

    # JWT token for authorization
    jwt_token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJleHAiOjE3Mzk2MzI5NDYsImlzcyI6IkpXVElzc3VlciIsImF1ZCI6IkpXVEF1ZGllbmNlIn0.33UHzdVkSTSzXhnD84IpmJbd70ffrt6-Y6qJQJzwmDw'

    @task
    def get_data(self):
        headers = {
            'accept': '*/*',
            'Authorization': f'Bearer {self.jwt_token}'  # Adding the JWT token here
        }
        self.client.get("/api/external/data", headers=headers, verify=False)  # Disable SSL verification
