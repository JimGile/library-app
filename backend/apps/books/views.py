from rest_framework import viewsets, filters
from rest_framework.pagination import PageNumberPagination
from django.db.models import Q
from django_filters.rest_framework import DjangoFilterBackend

from .models import Book
from .serializers import BookSerializer, BookListSerializer
from .filters import BookFilter


class StandardResultsSetPagination(PageNumberPagination):
    page_size = 10
    page_size_query_param = 'page_size'
    max_page_size = 100


class BookViewSet(viewsets.ReadOnlyModelViewSet):
    """List and retrieve books.

    Features:
    - search (title, author) via `?search=` (DRF SearchFilter)
    - filter by category and is_available via django-filter (`?category=` `?is_available=`)
    - ordering via `?ordering=` (title, author, created_at)
    """
    queryset = Book.objects.select_related('category').all().order_by('-created_at')
    pagination_class = StandardResultsSetPagination
    filter_backends = [DjangoFilterBackend, filters.SearchFilter, filters.OrderingFilter]
    filterset_class = BookFilter
    search_fields = ['title', 'author']
    ordering_fields = ['title', 'author', 'created_at']

    def get_serializer_class(self):
        if self.action == 'list':
            return BookListSerializer
        return BookSerializer
