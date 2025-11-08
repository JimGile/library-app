from django.test import TestCase
from django.urls import reverse
from rest_framework.test import APIClient

from ..models import Category, Book


class BooksApiTest(TestCase):
    def setUp(self):
        self.client = APIClient()
        self.cat = Category.objects.create(name='Fiction')
        self.book = Book.objects.create(
            title='Test Book', author='Jane Doe', category=self.cat, description='A test book', is_available=True
        )

    def test_list_books(self):
        url = reverse('book-list')
        resp = self.client.get(url)
        self.assertEqual(resp.status_code, 200)
        # Expect results key when paginated
        data = resp.json()
        self.assertIn('results', data)
        self.assertGreaterEqual(len(data['results']), 1)

    def test_retrieve_book(self):
        url = reverse('book-detail', args=[self.book.id])
        resp = self.client.get(url)
        self.assertEqual(resp.status_code, 200)
        data = resp.json()
        self.assertEqual(data['id'], self.book.id)
        self.assertEqual(data['title'], self.book.title)
