
CREATE ROLE cats_api_service LOGIN PASSWORD 'super_secure_pa$$word_3000';

-- =========================================================
-- Schema usage
-- =========================================================
GRANT USAGE ON SCHEMA auth TO cats_api_service;
GRANT USAGE ON SCHEMA cats TO cats_api_service;

-- =========================================================
-- Sequence permissions
-- =========================================================
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA auth TO cats_api_service;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA cats TO cats_api_service;

-- =========================================================
-- Table permissions
-- =========================================================
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE auth.users TO cats_api_service;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE cats.cats TO cats_api_service;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE cats.posts TO cats_api_service;
GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE cats.posts_cats TO cats_api_service;

