import django_filters

from .models import Book


class BookFilter(django_filters.FilterSet):
    category = django_filters.NumberFilter(field_name='category_id')
    is_available = django_filters.BooleanFilter(field_name='is_available')

    class Meta:
        model = Book
        fields = ['category', 'is_available']
