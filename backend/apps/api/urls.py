from rest_framework.routers import DefaultRouter
from django.urls import path, include

router = DefaultRouter()

# Register book endpoints
from apps.books.views import BookViewSet
router.register(r'books', BookViewSet, basename='book')

urlpatterns = [
    path('', include(router.urls)),
]
