from django.db import models
from django.contrib.auth.models import AbstractUser


class Member(AbstractUser):
    # Reuse Django's auth user for username/password/email management
    membership_type = models.CharField(max_length=50, blank=True)
    membership_start = models.DateField(null=True, blank=True)
    membership_end = models.DateField(null=True, blank=True)
    membership_status = models.CharField(max_length=50, blank=True)
    balance = models.DecimalField(max_digits=8, decimal_places=2, default=0.0)

    def __str__(self):
        return self.get_full_name() or self.username
