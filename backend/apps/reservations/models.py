from django.db import models
from django.conf import settings


class Reservation(models.Model):
    STATUS_RESERVED = 'reserved'
    STATUS_RETURNED = 'returned'
    STATUS_CANCELLED = 'cancelled'

    STATUS_CHOICES = [
        (STATUS_RESERVED, 'Reserved'),
        (STATUS_RETURNED, 'Returned'),
        (STATUS_CANCELLED, 'Cancelled'),
    ]

    book = models.ForeignKey('books.Book', on_delete=models.CASCADE, related_name='reservations')
    member = models.ForeignKey(settings.AUTH_USER_MODEL, on_delete=models.CASCADE, related_name='reservations')
    start_date = models.DateField(auto_now_add=True)
    due_date = models.DateField()
    return_date = models.DateField(null=True, blank=True)
    status = models.CharField(max_length=20, choices=STATUS_CHOICES, default=STATUS_RESERVED)
    late_fee = models.DecimalField(max_digits=8, decimal_places=2, default=0)

    created_at = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return f"Reservation({self.book}, {self.member}, {self.status})"
