START TRANSACTION;
ALTER TABLE "Uploads" ADD "Queries" text NOT NULL DEFAULT '';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251008214519_Migration_20251008_184452', '9.0.9');

COMMIT;

