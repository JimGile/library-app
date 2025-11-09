from rest_framework.routers import DefaultRouter
from django.urls import path, include

router = DefaultRouter()

# Register book endpoints
from apps.books.views import BookViewSet
router.register(r'books', BookViewSet, basename='book')

# Auth endpoints
from apps.members.views import RegisterView, LoginView


urlpatterns = [
    path('', include(router.urls)),
    path('auth/register/', RegisterView.as_view(), name='auth-register'),
    path('auth/login/', LoginView.as_view(), name='auth-login'),
]
