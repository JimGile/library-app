from django.db import transaction


def atomic_reservation():
    """Context manager helper for reservation atomic operations.

    Usage:
        with atomic_reservation():
            # update book availability and create reservation
    """
    return transaction.atomic()
