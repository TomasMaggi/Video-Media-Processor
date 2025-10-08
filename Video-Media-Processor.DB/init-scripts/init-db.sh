#!/bin/bash
set -e

echo "Waiting for PostgreSQL to be ready..."
until pg_isready -h $PGHOST -U $PGUSER; do
    sleep 1
done
echo "PostgreSQL is ready"

echo "Checking database state..."

# Check if __EFMigrationsHistory table exists
table_exists=$(psql -h $PGHOST -U $PGUSER -d $PGDATABASE -t -c "SELECT 1 FROM information_schema.tables WHERE table_name = '__EFMigrationsHistory';")

if [ -z "$table_exists" ]; then
    echo "__EFMigrationsHistory table not found. This appears to be a fresh database."
    echo "Applying all migrations in order..."
    
    # Apply all migrations in alphabetical order
    for migration in /migrations/migration_*.sql; do
        migration_name=$(basename "$migration" .sql)
        echo "Applying migration: $migration_name"
        psql -h $PGHOST -U $PGUSER -d $PGDATABASE -f "$migration"
        echo "Migration $migration_name applied successfully"
    done
    
    echo "All migrations applied to fresh database"
else
    echo "__EFMigrationsHistory table found. Checking for pending migrations..."
    
    # Execute migrations that haven't been applied yet
    for migration in /migrations/migration_*.sql; do
        migration_name=$(basename "$migration" .sql)
        migration_id=$(echo "$migration_name" | sed 's/^migration_//')
        
        # Check if this MigrationId exists in __EFMigrationsHistory
        result=$(psql -h $PGHOST -U $PGUSER -d $PGDATABASE -t -c "SELECT 1 FROM \"__EFMigrationsHistory\" WHERE \"MigrationId\" = '$migration_id';")
        
        if [ -z "$result" ]; then
            echo "Applying new migration: $migration_name"
            psql -h $PGHOST -U $PGUSER -d $PGDATABASE -f "$migration"
            echo "Migration $migration_name applied successfully"
        else
            echo "Skipping already applied migration: $migration_name"
        fi
    done
    
    echo "Migration check completed"
fi