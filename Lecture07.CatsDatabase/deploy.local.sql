CREATE DATABASE cats_db_local;

\connect cats_db_local

-- Schemas
\ir schema_objects/schemas/auth/auth.sql
\ir schema_objects/schemas/cats/cats.sql

-- Tables
\ir schema_objects/schemas/auth/tables/users.sql
\ir schema_objects/schemas/cats/tables/cats.sql
\ir schema_objects/schemas/cats/tables/posts.sql
\ir schema_objects/schemas/cats/tables/posts_cats.sql

-- Security
\ir database_objests/security/roles/cats_api_service.sql

-- Post deployment data
\ir scripts/post_deployment/users.script.sql
\ir scripts/post_deployment/cats.script.sql
\ir scripts/post_deployment/posts.script.sql
\ir scripts/post_deployment/posts_cats.script.sql