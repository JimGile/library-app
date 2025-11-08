from rest_framework import serializers

from .models import Book, Category


class CategorySerializer(serializers.ModelSerializer):
    class Meta:
        model = Category
        fields = ("id", "name")


class BookSerializer(serializers.ModelSerializer):
    category = CategorySerializer(read_only=True)

    class Meta:
        model = Book
        fields = (
            "id",
            "title",
            "author",
            "category",
            "is_available",
            "description",
            "created_at",
        )
        read_only_fields = ("id", "created_at")


class BookListSerializer(serializers.ModelSerializer):
    # lightweight serializer for list endpoints
    category = serializers.CharField(source='category.name', read_only=True)

    class Meta:
        model = Book
        fields = ("id", "title", "author", "category", "is_available")
