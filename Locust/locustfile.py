from locust import HttpUser, task, between

class ApiUser(HttpUser):
    wait_time = between(0.01, 0.1)  # Wait time between requests, 0.1 to 1 second

    # JWT token for authorization
    jwt_token = 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJtZW1iZXJfaWQiOiI5MjM5NjAiLCJGaXJzdE5hbWUiOiIiLCJMYXN0TmFtZSI6IiIsIkVtYWlsIjoiMDk3Mjk4MDEyMSIsInVzZXJJZCI6IjIwODY1MSIsImZkdGsiOiIlMmYxcVNiUGtyMnRITnFRdnNMQVBvWE1lY2VWVEJDQnNRdUdoJTJibkZCOTFqZyUzZCIsIm5pY2tOYW1lIjoi5rGC6IG36ICFaG9mZm1hbiIsInNleCI6IjEiLCJuYmYiOjE3Mzk2MzkxMDMsImV4cCI6MTc0MjIzMTEwMywiaXNzIjoiQXV0aEJpbGwifQ._GYNFTk3TVhQ1bL51btr0MipqFAM24gtfD4iInrQ2yM'

    @task
    def get_data(self):
        headers = {
            'accept': '*/*',
            'Authorization': f'{self.jwt_token}'  # Adding the JWT token here
        }
        self.client.get("/api/v1/NewCourse/GetCourseStatus/E25011765", headers=headers, verify=False)  # Disable SSL verification
