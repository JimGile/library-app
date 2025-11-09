from django.test import TestCase
from django.urls import reverse
from rest_framework.test import APIClient

from django.contrib.auth import get_user_model


User = get_user_model()


class AuthApiTests(TestCase):
    def setUp(self):
        self.client = APIClient()

    def test_register_and_login(self):
        register_url = reverse('auth-register')
        data = {'username': 'alice', 'email': 'alice@example.com', 'password': 'secret123'}
        resp = self.client.post(register_url, data, format='json')
        self.assertEqual(resp.status_code, 201)
        self.assertIn('token', resp.json())

        login_url = reverse('auth-login')
        resp2 = self.client.post(login_url, {'username': 'alice', 'password': 'secret123'}, format='json')
        self.assertEqual(resp2.status_code, 200)
        self.assertIn('token', resp2.json())
