r"""Seed script to populate sample categories, books and a demo member.

This script is safe to import and idempotent (uses get_or_create). It is written so it
can be executed directly from the backend folder or via the repository root.

Examples (PowerShell):

  # from repository root
  & ".\.venv\Scripts\python.exe" backend\scripts\seed_data.py

  # or, from the backend directory
  Push-Location backend; & "..\.venv\Scripts\python.exe" -m pip install -r requirements.txt; & ".\.venv\Scripts\python.exe" manage.py migrate; & ".\.venv\Scripts\python.exe" scripts/seed_data.py; Pop-Location

The script adds the parent "backend" directory to sys.path when executed directly so
the Django project package can be imported reliably.
"""

import os
import sys

# Ensure the Django project package (the parent "backend" directory) is on sys.path
# when the script is executed directly (so imports like "backend.settings" work).
HERE = os.path.dirname(os.path.abspath(__file__))
PROJECT_DIR = os.path.dirname(HERE)
if PROJECT_DIR not in sys.path:
    sys.path.insert(0, PROJECT_DIR)

os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'backend.settings')

import django


def seed():
    # Import and configure Django when running the seeder
    django.setup()

    from apps.books.models import Category, Book
    from django.contrib.auth import get_user_model

    print('Seeding categories...')
    categories = [
        ('Fiction', 'Fiction books'),
        ('Nonfiction', 'Informative books'),
        ('Science', 'Science and technology'),
        ('History', 'Historical works'),
        ('Children', 'Books for children'),
        ('Mystery', 'Mystery & thriller'),
        ('Fantasy', 'Fantasy & speculative fiction'),
        ('Biography', 'Biographies and memoirs'),
    ]
    for name, desc in categories:
        Category.objects.get_or_create(name=name, defaults={'description': desc})

    print('Seeding books...')
    # Map categories to lists of (title, author)
    books_by_category = {
        'Fiction': [
            ('The First Book', 'A. Writer'),
            ('Another Story', 'B. Author'),
            ('Novel of Things', 'C. Storyteller'),
        ],
        'Nonfiction': [
            ('Understanding Stuff', 'D. Thinker'),
            ('Practical Guide', 'E. Helper'),
            ('The Real World', 'F. Reporter'),
            ('In-Depth Analysis', 'X. Researcher'),
        ],
        'Science': [
            ('Intro to Chemistry', 'G. Chemist'),
            ('Space and Time', 'H. Physicist'),
            ('Biology Basics', 'I. Biologist'),
        ],
        'History': [
            ('Old Empires', 'J. Historian'),
            ('Revolutions', 'K. Scholar'),
            ('Modern Times', 'L. Analyst'),
        ],
        'Children': [
            ('Fun with Numbers', 'M. KidAuthor'),
            ('Bedtime Tales', 'N. Storyteller'),
            ('ABC Adventures', 'O. Illustrator'),
        ],
        'Mystery': [
            ('The Hidden Clue', 'P. Detective'),
            ('Night Whispers', 'Q. Noir'),
            ('Shadow Trace', 'W. Sleuth'),
        ],
        'Fantasy': [
            ('Dragon Rider', 'R. Mage'),
            ('The Lost Kingdom', 'S. Bard'),
            ('Wizards & Wares', 'T. Enchanter'),
        ],
        'Biography': [
            ('Life of X', 'U. Biographer'),
            ('Memoirs of Y', 'V. Memoirist'),
            ('Journey of Z', 'W. Historian'),
        ],
    }

    created = 0
    for cat_name, entries in books_by_category.items():
        try:
            cat = Category.objects.get(name=cat_name)
        except Category.DoesNotExist:
            print(f'Category {cat_name} missing, skipping')
            continue
        for title, author in entries:
            _, created_flag = Book.objects.get_or_create(
                title=title,
                author=author,
                category=cat,
                defaults={'description': 'Sample seeded book', 'is_available': True},
            )
            if created_flag:
                created += 1

    print(f'Seeded {created} new books')

    user = get_user_model()
    if not user.objects.filter(username='admin').exists():
        print('Creating admin user (username: admin, password: admin)')
        user.objects.create_superuser('admin', 'admin@example.com', 'admin')

    print('Seeding completed')


if __name__ == '__main__':
    # When executed as a script, perform seeding. Importing this module will be safe.
    seed()
